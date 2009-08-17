using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hello.Repo;
using Hello.Utils;
using Hello.Bot.TweetTypes;

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
            // Unprocessed tweets
            var tweets = _repo
                .QueuedTweets
                .Where(t => !t.Processed);

            foreach (QueuedTweet tweet in tweets)
            {
                ProcessedTweet processedTweet = TweetParser.Parse(tweet);

                // Not interested in this tweet... move along...
                if (processedTweet == null)
                {
                    tweet.Processed = true;
                    _repo.SubmitChanges();
                    continue;
                }

                User user = _repo
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

                HelloTweet helloTweet = processedTweet as HelloTweet;
                if (helloTweet != null)
                {
                    ProcessTweet(user, helloTweet);
                    tweet.Processed = true;
                    _repo.SubmitChanges();
                    continue;
                }

                SatTweet satTweet = processedTweet as SatTweet;
                if (satTweet != null)
                {
                    ProcessTweet(user, satTweet);
                    tweet.Processed = true;
                    _repo.SubmitChanges();
                    continue;
                }

                ClaimTweet claimTweet = processedTweet as ClaimTweet;
                if (claimTweet != null)
                {
                    ProcessTweet(user, claimTweet);
                    tweet.Processed = true;
                    _repo.SubmitChanges();
                    continue;
                }

                MetTweet metTweet = processedTweet as MetTweet;
                if (metTweet != null)
                {
                    ProcessTweet(user, metTweet);
                    tweet.Processed = true;
                    _repo.SubmitChanges();
                    continue;
                }

                MessageTweet messageTweet = processedTweet as MessageTweet;
                if (messageTweet != null)
                {
                    ProcessTweet(user, messageTweet);
                    tweet.Processed = true;
                    _repo.SubmitChanges();
                    continue;
                }
            }
        }

        private void ProcessTweet(User user, HelloTweet tweet)
        {
            user.UserTypeID = tweet.UserType;
            user.ShadowAccount = false;

            if (tweet.Tags.Count > 0)
            {
                var oldTags = _repo
                    .Tags
                    .Where(t => t.Username == user.Username)
                    .Select(t => t.Name);
                _repo
                    .Tags
                    .InsertAllOnSubmit(
                        tweet
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

        private void ProcessTweet(User user, SatTweet tweet)
        {
            var sessions = _repo
                        .Sessions
                        .Where(
                            s => s.Start < DateTime.Now.AddHours(1) // starts with an hours time
                              && s.Finish > DateTime.Now            // hasn't finished
                        );

            Seat seat = _repo
                .Seats
                .SingleOrDefault(s => s.Code == tweet.SeatCode);

            if (sessions.Count() == 1 && seat != null)
            {
                Session session = sessions.First();

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

        private void ProcessTweet(User user, ClaimTweet tweet)
        {
            var tokens = _repo
                        .Tokens
                        .Where(t => t.Token1 == tweet.Token);

            if (tokens.Count() == 1)
            {
                Token token = tokens.First();

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

        private void ProcessTweet(User user, MetTweet tweet)
        {
            foreach (string friend in tweet.Friends)
            {
                User friendUser = _repo
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

            List<string> befriendees = user.Befrienders.Select(f => f.Befriendee).ToList();

            _repo
                .Friendships
                .InsertAllOnSubmit(
                    tweet
                        .Friends
                        .Where(f => !befriendees.Contains(f))
                        .Select(friend => new Friendship
                        {
                            Befriender = user.Username,
                            Befriendee = friend
                        })
                );
        }

        private void ProcessTweet(User user, MessageTweet tweet)
        {
            Message message = user.Message;

            if (message == null)
            {
                message = new Message { Username = user.Username };
                _repo.Messages.InsertOnSubmit(message);
            }

            message.Offensive = false;
            message.Text = tweet.Message;
        }
    }
}
