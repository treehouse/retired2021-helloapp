using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hello.Repo;
using Hello.Utils;
using Dimebrain.TweetSharp.Fluent;
using Dimebrain.TweetSharp.Extensions;
using Dimebrain.TweetSharp.Model;

namespace Hello.Bot
{
    public class TweetQueuer
    {
        private const long MIN_TWEET_ID = 2; // twitter doesn't like 0, and we do a lastID - 1 below, so... 2, it's the magic number

        private HelloRepoDataContext _repo;

        public TweetQueuer(HelloRepoDataContext repo)
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

        public void QueueMentions()
        {
            long lastID = GetLastTideMark("Mentions");
            
            string request = GetTwitterRequest()
                    .Statuses()
                    .Mentions()
                    .Since(lastID)
                    .Take(int.MaxValue)
                    .Request();

            var statuses = request
                    .AsStatuses();

            if (statuses == null)
            {
                TwitterError error = request.AsError();
                if (error != null)
                    throw new HelloException(error.ErrorMessage);
            }

            var queuedTweets = statuses
                    .Select(s => new QueuedTweet
                    {
                        Username = s.User.ScreenName.ToLower(),
                        Message = s.Text,
                        Created = DateTime.Now,
                        ImageURL = s.User.ProfileImageUrl,
                        Followers = s.User.FollowersCount
                    });

            StoreTweets(queuedTweets);

            long maxID = statuses.Count() > 0 ? statuses.Max(s => s.Id) : lastID;
            MarkTide("Mentions", maxID);
        }

        private void StoreTweets(IEnumerable<QueuedTweet> tweets)
        {
            if (tweets.Count() > 0)
            {
                _repo.QueuedTweets.InsertAllOnSubmit(tweets);
                _repo.SubmitChanges();
            }
        }

        private long GetLastTideMark(string name)
        {
            TideMark lastTideMark = _repo
                .TideMarks
                .Where(m => m.Name == name)
                .OrderByDescending(m => m.TimeStamp)
                .FirstOrDefault();

            return lastTideMark != null ? lastTideMark.LastID : MIN_TWEET_ID;
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
