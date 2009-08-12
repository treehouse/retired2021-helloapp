using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hello.Repo;
using Dimebrain.TweetSharp.Fluent;
using Dimebrain.TweetSharp.Extensions;

namespace Hello.Bot
{
    public class Engine
    {
        private const long MIN_TWEET_ID = 2; // twitter doesn't like 0, and we do a lastID - 1 below, so... 2, it's the magic number

        private HelloRepoDataContext _repo;

        public Engine(HelloRepoDataContext repo)
        {
            _repo = repo;
        }

        public IFluentTwitter GetTwitterRequest()
        {
            return FluentTwitter.CreateRequest()
                                .AuthenticateAs(
                                    Settings.TwitterBotUsername,
                                    Settings.TwitterBotPassword);
        }

        public void QueueHashTagged()
        {
            var lastID = GetLastTideMark("HashTagged");
            var seenLastTweet = false;
            var maxID = 0L;
            var page = 1;

            while (!seenLastTweet)
            {
                var request = GetTwitterRequest()
                    .Search()
                    .Query()
                    .ContainingHashTag(Settings.TwitterHashTag)
                    .Since(lastID - 1)
                    .Skip(page)
                    .Take(int.MaxValue)
                    .Request();
                var statuses = request
                    .AsSearchResult()
                    .Statuses;

                var tweetsToQueue = statuses
                    .Where(s => s.Id != lastID)
                    .Select(s => new QueuedTweet
                    {
                        Username = s.FromUserScreenName,
                        Message = s.Text,
                        Created = DateTime.Now,
                        ImageURL = s.ProfileImageUrl
                    });

                StoreTweets(tweetsToQueue);

                seenLastTweet = lastID == MIN_TWEET_ID || statuses.Any(s => s.Id == lastID);
                maxID = Math.Max(maxID, statuses.Max(s => s.Id));
                page++;
            }

            MarkTide("HashTagged", maxID);
        }

        public void QueueMentions()
        {
            var lastID = GetLastTideMark("Mentions");
            var seenLastTweet = false;
            var maxID = 0L;
            var page = 1;

            while (!seenLastTweet)
            {
                var request = GetTwitterRequest()
                    .Statuses()
                    .Mentions()
                    .Since(lastID - 1)
                    .Skip(page)
                    .Take(int.MaxValue)
                    .Request();
                var statuses = request
                    .AsStatuses();
                
                var queuedTweets = statuses
                    .Where(s => s.Id != lastID)
                    .Select(s => new QueuedTweet
                    {
                        Username = s.User.ScreenName,
                        Message = s.Text,
                        Created = DateTime.Now,
                        ImageURL = s.User.ProfileImageUrl
                    });

                StoreTweets(queuedTweets);

                seenLastTweet = lastID == MIN_TWEET_ID || statuses.Any(s => s.Id == lastID);
                maxID = Math.Max(maxID, statuses.Max(s => s.Id));
                page++;
            }

            MarkTide("Mentions", maxID);
        }

        public void QueueDirectMessages()
        {
            var lastID = GetLastTideMark("DirectMessages");
            var seenLastTweet = false;
            var maxID = 0L;
            var page = 1;

            while (!seenLastTweet)
            {
                var request = GetTwitterRequest()
                    .DirectMessages()
                    .Received()
                    .Since(lastID - 1)
                    .Skip(page)
                    .Take(int.MaxValue)
                    .Request();
                var statuses = request
                    .AsDirectMessages();

                var queuedTweets = statuses
                    .Where(s => s.Id != lastID)
                    .Select(s => new QueuedTweet
                    {
                        Username = s.SenderScreenName,
                        Message = s.Text,
                        Created = DateTime.Now,
                        ImageURL = s.Sender.ProfileImageUrl
                    });

                StoreTweets(queuedTweets);

                seenLastTweet = lastID == MIN_TWEET_ID || statuses.Any(s => s.Id == lastID);
                maxID = Math.Max(maxID, statuses.Max(s => s.Id));
                page++;
            }
            MarkTide("DirectMessages", maxID);
        }

        private void StoreTweets(IEnumerable<QueuedTweet> tweets)
        {
            if (tweets.Count() > 0)
            {
                _repo.QueuedTweets.InsertAllOnSubmit(tweets);
                _repo.SubmitChanges();
            }
        }

        //public void SendDirectMessage(string username, string message)
        //{
        //    var response = GetTwitterRequest()
        //        .Statuses()
        //        .Update(String.Format("d {0} {1}", username, message))
        //        .Request();
        //}

        public void ProcessTweets()
        {
            // ignore tweets that we sent
            var myTweets = _repo
                .QueuedTweets
                .Where(t => !t.Processed
                         && t.Username == Settings.TwitterBotUsername);
            foreach (var tweet in myTweets)
                tweet.Processed = true;

            _repo.SubmitChanges();

            // Unprocessed tweets
            var tweets = _repo
                .QueuedTweets
                .Where(t => !t.Processed);

            // Grab the UserTypes only once
            var userTypes = _repo
                .UserTypes
                .Select(ut => ut.UserTypeID)
                .ToList();

            foreach (var tweet in tweets)
            {
                var user = _repo
                    .Users
                    .SingleOrDefault(u => u.Username == tweet.Username);

                if (user == null)
                {
                    user = new User
                    {
                        ImageURL = tweet.ImageURL,
                        Username = tweet.Username,
                        Created = DateTime.Now,
                        Updated = DateTime.Now
                    };
                    _repo.Users.InsertOnSubmit(user);
                }
                

                var processedTweet = TweetProcessor.Process(tweet.Message);

                var metTweet = processedTweet as MetTweet;
                if (metTweet != null)
                {
                    _repo
                        .Friendships
                        .InsertAllOnSubmit(
                            metTweet
                                .Friends
                                .Select(friend => new Friendship
                                {
                                    Befriender = tweet.Username,
                                    Befriendee = friend
                                })
                        );
                }

                var messageTweet = processedTweet as MessageTweet;
                if (messageTweet != null)
                {
                    var message = user.Message ?? new Message { Username = tweet.Username };

                    message.Offensive = false;
                    message.Text = messageTweet.Message;
                }

                var helloTweet = processedTweet as HelloTweet;
                if (helloTweet != null)
                {
                    user.UserTypeID = helloTweet.UserType;

                    if (helloTweet.Tags.Count > 0)
                        _repo
                            .Tags
                            .InsertAllOnSubmit(
                                helloTweet
                                    .Tags
                                    .Select(tag => new Tag
                                    {
                                        Created = DateTime.Now,
                                        Name = tag,
                                        Username = tweet.Username
                                    })
                            );
                }

                var sitTweet = processedTweet as SatTweet;
                if (sitTweet != null)
                {
                    var sessions = _repo
                        .Sessions
                        .Where(
                            s => s.Start < DateTime.Now.AddHours(1) // starts with an hours time
                              && s.Finish > DateTime.Now            // hasn't finished
                        );

                    if (sessions.Count() == 1)
                    {
                        var session = sessions.First();

                        _repo
                            .Sats
                            .InsertOnSubmit(new Sat
                            {
                                SessionID = session.SessionID
                            });
                    }
                }

                var claimTweet = processedTweet as ClaimTweet;
                if (claimTweet != null)
                {
                    var tokens = _repo
                        .Tokens
                        .Where(t => t.Token1 == claimTweet.Token);

                    if (tokens.Count() == 1)
                    {
                        var token = tokens.First();

                        _repo
                            .Redemptions
                            .InsertOnSubmit(new Redemption
                            {
                                Created = DateTime.Now,
                                Username = tweet.Username,
                                TokenID = token.TokenID
                            });
                    }
                }
                tweet.Processed = true;

                _repo.SubmitChanges();
            }
        }

        private long GetLastTideMark(string name)
        {
            var lastTideMark = _repo
                .TideMarks
                .Where(m => m.Name == name)
                .OrderByDescending(m => m.TimeStamp)
                .FirstOrDefault();

            var lastID = MIN_TWEET_ID;
            if (lastTideMark != null)
                lastID = lastTideMark.LastID;

            return lastID;
        }

        private void MarkTide(string name, long lastID)
        {
            _repo
                .TideMarks
                .InsertOnSubmit(new TideMark
                {
                    LastID = lastID,
                    TimeStamp = DateTime.Now,
                    Name = name
                });

            _repo.SubmitChanges();
        }
    }
}
