using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hello.Repo;
using Hello.Utils;
using Hello.Bot.TweetTypes;

namespace Hello.Bot
{
    public class TweetProcessor
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

                HiFiveTweet hiFiveTweet = processedTweet as HiFiveTweet;
                if (hiFiveTweet != null)
                {
                    ProcessTweet(user, hiFiveTweet);
                    tweet.Processed = true;
                    _repo.SubmitChanges();
                    continue;
                }
            }
        }

        public void ProcessTweet(User user, HiFiveTweet tweet)
        {
            _repo.HiFives.InsertOnSubmit(new HiFive
                {
                    HiFiverUser = user,
                    HiFivee = tweet.Friend
                });
        }

        public void ProcessTweet(User user, HelloTweet tweet)
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

        public void ProcessTweet(User user, SatTweet tweet)
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

        public void ProcessTweet(User user, ClaimTweet tweet)
        {
            /*
             * The following line would throw an exception if multiple results for the same token,
             * but this is taken care of by a unique constraint in the DB
             */
            Token token = _repo.Tokens.Where(t => t.Token1 == tweet.Token).SingleOrDefault();

            if (token != null)
            {
                /* 
                 * Only process if they haven't yet redemed this token
                 * and the total number of redemptions hasn't exceeded the limit
                 */
                if (_repo.Redemptions.Where(r => r.User == user && r.TokenID == token.TokenID).Count() == 0
                    && _repo.Redemptions.Where(r => r.TokenID == token.TokenID).Count() < token.AllowedRedemptions)
                {
                    _repo.Redemptions.InsertOnSubmit(new Redemption
                                                         {
                                                             Created = DateTime.Now,
                                                             Username = user.Username,
                                                             TokenID = token.TokenID
                                                         });

                    CreditPoints(user, token.Campaign.Value, "Token:" + token.Token1);
                }
            }
        }

        public void ProcessTweet(User user, MetTweet tweet)
        {
            List<string> befriendees = user.Befrienders.Select(f => f.Befriendee).ToList();

            foreach (string friend in tweet.Friends)
            {
                User friendUser = _repo
                    .Users
                    .SingleOrDefault(u => u.Username == friend);

                // Add the user if they don't already exist
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

                // Add the friendship if it doesn't already exist
                if (!befriendees.Contains(friend))
                {
                    _repo.Friendships.InsertOnSubmit(
                        new Friendship
                        {
                            Befriender = user.Username,
                            Befriendee = friend
                        });

                    // If the reverse friendship exists, credit points
                    if (_repo.Friendships.Where(f => f.Befriendee == user.Username && f.Befriender == friend).Count() > 0)
                    {
                        CreditPoints(user, 10, "Mutual meeting");
                        CreditPoints(friendUser, 10, "Mutual meeting");
                    }
                }
            }
        }

        public void ProcessTweet(User user, MessageTweet tweet)
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

        public void CreditPoints(User user, int points, string details)
        {
            _repo.Points.InsertOnSubmit(
                new Point
                {
                    User = user,
                    Amount = points,
                    Issued = DateTime.Now,
                    Details = details
                });
        }
    }
}
