using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hello.Repo;
using Hello.Utils;
using Dimebrain.TweetSharp.Fluent;
using Dimebrain.TweetSharp.Extensions;

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

                if (statuses == null)
                {
                    var error = request.AsError();
                    if (error != null)
                        throw new Exception (error.ErrorMessage);
                }

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
