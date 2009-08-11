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
            var statusesWithOurHashTag = GetTwitterRequest()
                .Search()
                .Query()
                .ContainingHashTag(ConfigurationManager.AppSettings["TwitterHashTag"])
                //.Since(123456789)
                //.Skip(2)
                .Take(int.MaxValue)
                .Request()
                .AsSearchResult();
        }

        private static void ProcessMentions()
        {
            var statusesMentioningMe = GetTwitterRequest()
                .Statuses()
                .Mentions()
                //.Since(123456789)
                .Request()
                .AsStatuses();
        }

        private static void ProcessDirectMessages()
        {
            var receivedDMs = GetTwitterRequest()
                .DirectMessages()
                .Received()
                //.Since()
                .Request()
                .AsDirectMessages();
        }

        private static void SendDirectMessage(string screenName, string message)
        {
            var response = GetTwitterRequest()
                .Statuses()
                .Update(String.Format("d {0} {1}", screenName, message))
                .Request();
        }
    }
}
