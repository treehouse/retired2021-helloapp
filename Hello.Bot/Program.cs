using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using System.Text;
using Hello.Repo;
using Dimebrain.TweetSharp.Fluent;
using Dimebrain.TweetSharp.Extensions;

namespace Hello.Bot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ProcessDirectMessages();

            ProcessMentions();

            ProcessHashTag();
        }

        private static IFluentTwitter GetTwitterRequest()
        {
            return FluentTwitter.CreateRequest()
                                .AuthenticateAs(
                                    ConfigurationManager.AppSettings["TwitterBotUsername"],
                                    ConfigurationManager.AppSettings["TwitterBotPassword"]);
        }

        private static void ProcessHashTag()
        {
            // TODO: Check max id

            var statuses = GetTwitterRequest()
                .Search()
                .Query()
                .ContainingHashTag(ConfigurationManager.AppSettings["TwitterHashTag"])
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

        private static void ProcessMentions()
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

        private static void ProcessDirectMessages()
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

        private static void StoreTweets(IEnumerable<QueuedTweet> tweets)
        {
            var repo = new HelloRepoDataContext();
            repo.QueuedTweets.InsertAllOnSubmit(
                tweets.Select(t => new QueuedTweet {
                    Username = t.Username,
                    Message = t.Message,
                    Created = DateTime.Now
                })
            );
        }

        private static void SendDirectMessage(string username, string message)
        {
            var response = GetTwitterRequest()
                .Statuses()
                .Update(String.Format("d {0} {1}", username, message))
                .Request();
        }
    }
}
