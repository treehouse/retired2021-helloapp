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
            // TODO: Check max id

            var statuses = GetTwitterRequest()
                .Search()
                .Query()
                .ContainingHashTag(Settings.TwitterHashTag)
                //.Since(123456789)
                //.Skip(2)
                .Take(int.MaxValue)
                .Request()
                .AsSearchResult()
                .Statuses
                .Select(s => new QueuedTweet
                {
                    Username = s.FromUserScreenName,
                    Message = s.Text
                });

            // TODO: record max id

            StoreTweets(statuses);
        }

        public void QueueMentions()
        {
            // TODO: Check max id

            var statuses = GetTwitterRequest()
                .Statuses()
                .Mentions()
                //.Since(123456789)
                .Request()
                .AsStatuses()
                .Select(s => new QueuedTweet
                {
                    Username = s.User.ScreenName,
                    Message = s.Text
                });

            // TODO: record max id

            StoreTweets(statuses);
        }

        public void QueueDirectMessages()
        {
            // TODO: Check max id

            var statuses = GetTwitterRequest()
                .DirectMessages()
                .Received()
                //.Since()
                .Request()
                .AsDirectMessages()
                .Select(s => new QueuedTweet
                {
                    Username = s.SenderScreenName,
                    Message = s.Text
                });

            // TODO: record max id

            StoreTweets(statuses);
        }

        private void StoreTweets(IEnumerable<QueuedTweet> tweets)
        {
            _repo.QueuedTweets.InsertAllOnSubmit(
                tweets.Select(t => new QueuedTweet
                {
                    Username = t.Username,
                    Message = t.Message,
                    Created = DateTime.Now
                })
            );

            _repo.SubmitChanges();
        }

        public void SendDirectMessage(string username, string message)
        {
            var response = GetTwitterRequest()
                .Statuses()
                .Update(String.Format("d {0} {1}", username, message))
                .Request();
        }

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

            var tweets = _repo
                .QueuedTweets
                .Where(t => !t.Processed);

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
                    user = new User
                    {
                        Username = tweet.Username
                    };

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
    }
}
