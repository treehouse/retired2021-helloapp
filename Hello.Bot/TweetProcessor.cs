using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hello.Repo;
using Hello.Utils;

namespace Hello.Bot
{
    class TweetProcessor
    {
        private HelloRepoDataContext _repo;

        public TweetProcessor(HelloRepoDataContext repo)
        {
            _repo = repo;
        }

        public void ProcessTweets()
        {
            // ignore our tweets and vias
            var myTweets = _repo
                .QueuedTweets
                .Where(t => !t.Processed
                         && t.Username == Settings.TwitterBotUsername);
            foreach (var tweet in myTweets)
                tweet.Processed = true;

            var viaTweets = _repo
                .QueuedTweets
                .Where(t => !t.Processed
                         && t.Message.Contains("via @"));
            foreach (var tweet in viaTweets)
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
                var processedTweet = TweetParser.Parse(tweet.Message);

                // Not interested in this tweet... move along...
                if (processedTweet == null)
                {
                    tweet.Processed = true;
                    _repo.SubmitChanges();
                    continue;
                }

                var user = _repo
                    .Users
                    .SingleOrDefault(u => u.Username == tweet.Username);

                if (user == null)
                {
                    user = new User
                    {
                        ImageURL = tweet.ImageURL.Replace("_normal.", "_bigger."), // we want the larger twitter image if it exists
                        Username = tweet.Username,
                        Created = DateTime.Now,
                        Updated = DateTime.Now
                    };
                    _repo.Users.InsertOnSubmit(user);
                }

                var helloTweet = processedTweet as HelloTweet;
                if (helloTweet != null)
                {
                    user.UserTypeID = helloTweet.UserType;
                    user.ShadowAccount = false;

                    if (helloTweet.Tags.Count > 0)
                    {
                        var oldTags = _repo
                            .Tags
                            .Where(t => t.Username == user.Username)
                            .Select(t => t.Name);
                        _repo
                            .Tags
                            .InsertAllOnSubmit(
                                helloTweet
                                    .Tags
                                    .Where(t => !oldTags.Contains(t))
                                    .Select(tag => new Tag
                                    {
                                        Created = DateTime.Now,
                                        Name = tag,
                                        Username = user.Username
                                    })
                            );
                    }
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

                    var befriendees = user.Befrienders.Select(f => f.Befriendee).ToList();

                    _repo
                        .Friendships
                        .InsertAllOnSubmit(
                            metTweet
                                .Friends
                                .Where(f => !befriendees.Contains(f))
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
                    var message = user.Message;

                    if (message == null)
                    {
                        message = new Message { Username = user.Username };
                        _repo.Messages.InsertOnSubmit(message);
                    }

                    message.Offensive = false;
                    message.Text = messageTweet.Message;
                }

                tweet.Processed = true;

                _repo.SubmitChanges();
            }
        }
    }
}
