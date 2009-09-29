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
                        Username = tweet.Username,
                        Created = DateTime.Now,
                    };
                    _repo.Users.InsertOnSubmit(user);
                }

                // Update user's details
                if (String.IsNullOrEmpty(tweet.ImageURL))
                    user.ImageURL = Settings.DefaultImageURL;
                else
                    user.ImageURL = tweet.ImageURL;
                user.Followers = tweet.Followers;
                user.Updated = DateTime.Now;

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
                    _repo.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, user);
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
            // Prevent users from Hi5ing themselves
            if (user.Username == tweet.Friend)
                return;

            Event currentEvent = _repo
                .Events
                .Where(e => e.Start <= DateTime.Now
                         && e.End >= DateTime.Now)
                .SingleOrDefault();

            // Only accept Hi5s for the current event
            if (currentEvent != null)
            {
                // You can only Hi5 each person once per event and there is a total cap of hifives per person per event
                if (_repo.HiFives.Where(h => h.HiFiver == user.Username && h.EventID == currentEvent.EventID).Count() < currentEvent.HiFiveLimit 
                    && _repo.HiFives.Where(h => h.HiFiver == user.Username && h.EventID == currentEvent.EventID 
                        && h.HiFivee == tweet.Friend).Count() == 0)
                {
                    User hiFivee = EnsureUser(tweet.Friend);

                    _repo
                        .HiFives
                        .InsertOnSubmit(
                            new HiFive
                            {
                                Event = currentEvent,
                                HiFiver = user.Username,
                                HiFivee = tweet.Friend
                            });

                    CreditPoints(hiFivee, Settings.Points.HiFive, "Hi5: " + user.Username);
                }
            }
        }

        public void ProcessTweet(User user, HelloTweet tweet)
        {
            if (_repo
                .UserTypes
                .Any(t => t.UserTypeID == tweet.UserType))
            {
                user.UserTypeID = tweet.UserType;
            }
            user.ShadowAccount = false;

            foreach (var tagName in tweet.Tags)
            {
                var tag = _repo
                    .Tags
                    .SingleOrDefault(t => t.Username == user.Username
                                       && t.Name == tagName);
                if (tag == null)
                {
                    // Add new tag
                    tag = new Tag
                    {
                        Created = DateTime.Now,
                        Name = tagName,
                        Username = user.Username
                    };

                    _repo
                        .Tags
                        .InsertOnSubmit(tag);
                }
                else
                {
                    // or update old tag
                    tag.Created = DateTime.Now;
                }
            }
        }

        public void ProcessTweet(User user, SatTweet tweet)
        {
            // The logic for this isn't great - need to make sure we're really dealing with the current session
            Session session = _repo
                .Sessions
                .Where(s => s.Finish > DateTime.Now) // hasn't finished
                .OrderBy(s => s.Start)
                .FirstOrDefault();

            Seat seat = _repo
                .Seats
                .SingleOrDefault(s => s.Code == tweet.SeatCode);

            if (session != null && seat != null)
            {
                // If there's someone already in the seat then remove them from that seat.
                // There really should only be one of these, but this isn't enforced in DB so
                // clean all up just to be sure.
                var previousSitters = _repo
                    .Sats
                    .Where(s => s.SessionID == session.SessionID
                             && s.SeatID == seat.SeatID);
                _repo
                    .Sats
                    .DeleteAllOnSubmit(previousSitters);

                Sat currentSat = _repo
                    .Sats
                    .Where(s => s.SessionID == session.SessionID
                             && s.Username == user.Username)
                    .SingleOrDefault();

                // If they've already got a seat for the session, move them, rather than creating a new record
                // Also, only grant points the first time they sit down!
                if (currentSat == null)
                {
                    _repo
                        .Sats
                        .InsertOnSubmit(new Sat
                                            {
                                                Username = user.Username,
                                                SessionID = session.SessionID,
                                                SeatID = seat.SeatID
                                            });
                    CreditPoints(user, Settings.Points.Sat, "Sat in seat: " + session.SessionID);
                }
                else
                {
                    currentSat.SeatID = seat.SeatID;
                }
            }
        }

        public void ProcessTweet(User user, ClaimTweet tweet)
        {
            // The following line would throw an exception if multiple results for the same token,
            // but this is taken care of by a unique constraint in the DB
            Token token = _repo
                .Tokens
                .Where(t => t.Code == tweet.Token)
                .SingleOrDefault();

            if (token != null)
            {
                // Only process if they haven't yet redeemed this token
                // and the total number of redemptions hasn't exceeded the limit
                if (_repo.Redemptions.Where(r => r.User == user && r.TokenID == token.TokenID).Count() == 0
                    && _repo.Redemptions.Where(r => r.TokenID == token.TokenID).Count() < token.AllowedRedemptions)
                {
                    _repo.Redemptions.InsertOnSubmit(new Redemption
                                                         {
                                                             Created = DateTime.Now,
                                                             Username = user.Username,
                                                             TokenID = token.TokenID
                                                         });

                    CreditPoints(user, token.Campaign.Value, "Token: " + token.Code);
                }
            }
        }

        private User EnsureUser(string username)
        {
            User user = _repo
                    .Users
                    .SingleOrDefault(u => u.Username == username);

            // Add the user if they don't already exist
            if (user == null)
            {
                user = new User
                {
                    Username = username,
                    ImageURL = Settings.DefaultImageURL,
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    ShadowAccount = true
                };
                _repo.Users.InsertOnSubmit(user);
            }

            return user;
        }

        public void ProcessTweet(User user, MetTweet tweet)
        {
            List<string> befriendees = user
                .Befrienders
                .Select(f => f.Befriendee)
                .ToList();

            foreach (string friend in tweet.Friends)
            {
                // Prevent user from meeting themselves
                if (user.Username == friend)
                    continue;

                User friendUser = EnsureUser(friend);

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
                    if (_repo
                        .Friendships
                        .Any(f => f.Befriendee == user.Username && f.Befriender == friend))
                    {
                        CreditPoints(user, Settings.Points.Met, "Mutual meeting");
                        CreditPoints(friendUser, Settings.Points.Met, "Mutual meeting");
                    }
                }

                // Submit changes up to this point as we need to be ready to recognise reflexive meetings in the same batch.
                _repo.SubmitChanges();
            }
        }

        public void ProcessTweet(User user, MessageTweet tweet)
        {
            if (user.Message == null)
            {
                user.Message = new Message { Username = user.Username };
                _repo.Messages.InsertOnSubmit(user.Message);
            }

            user.Message.Offensive = false;
            user.Message.Text = tweet.Message;
        }

        public void CreditPoints(User user, int points, string details)
        {
            _repo
                .Points
                .InsertOnSubmit(
                    new Point
                    {
                        User = user,
                        Amount = points,
                        Issued = DateTime.Now,
                        Details = details.Length > 20 ? details.Substring(0, 20) : details
                    });
        }
    }
}
