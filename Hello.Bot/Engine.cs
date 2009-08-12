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
                        Username = s.FromUserScreenName.ToLower(),
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
                        Username = s.User.ScreenName.ToLower(),
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
                        Username = s.SenderScreenName.ToLower(),
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
                        ImageURL = user.ImageURL,
                        Username = user.Username,
                        Created = DateTime.Now,
                        Updated = DateTime.Now
                    };
                    _repo.Users.InsertOnSubmit(user);
                }
                
                var processedTweet = TweetProcessor.Process(tweet.Message);

                var helloTweet = processedTweet as HelloTweet;
                if (helloTweet != null)
                {
                    user.UserTypeID = helloTweet.UserType;
                    user.ShadowAccount = false;

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
                                        Username = user.Username
                                    })
                            );
                }

                var satTweet = processedTweet as SatTweet;
                if (satTweet != null)
                {
                    var sessions = _repo
                        .Sessions
                        .Where(
                            s => s.Start < DateTime.Now.AddHours(1) // starts with an hours time
                              && s.Finish > DateTime.Now            // hasn't finished
                        );

                    var seat = _repo
                        .Seats
                        .SingleOrDefault(s => s.Code == satTweet.SeatCode);

                    if (sessions.Count() == 1 && seat != null)
                    {
                        var session = sessions.First();

                        _repo
                            .Sats
                            .InsertOnSubmit(new Sat
                            {
                                Username = user.Username,
                                SessionID = session.SessionID,
                                SeatID = seat.SeatID
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
                                Username = user.Username,
                                TokenID = token.TokenID
                            });
                    }
                }

                var metTweet = processedTweet as MetTweet;
                if (metTweet != null)
                {
                    foreach (var friend in metTweet.Friends)
                    {
                        var friendUser = _repo
                            .Users
                            .SingleOrDefault(u => u.Username == friend);

                        if (friendUser == null)
                        {
                            friendUser = new User
                            {
                                Username = friend,
                                ImageURL = Settings.DefaultImageURL,
                                Created = DateTime.Now,
                                Updated = DateTime.Now,
                                ShadowAccount = true
                            };
                            _repo.Users.InsertOnSubmit(friendUser);
                        }
                    }

                    _repo
                        .Friendships
                        .InsertAllOnSubmit(
                            metTweet
                                .Friends
                                .Select(friend => new Friendship
                                {
                                    Befriender = user.Username,
                                    Befriendee = friend
                                })
                        );
                }

                var messageTweet = processedTweet as MessageTweet;
                if (messageTweet != null)
                {
                    var message = user.Message ?? new Message { Username = user.Username };

                    message.Offensive = false;
                    message.Text = messageTweet.Message;
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
