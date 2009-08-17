using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Text.RegularExpressions;
using Hello.Utils;

namespace Hello.Bot
{
    public static class TweetProcessor
    {
        public static ProcessedTweet Process(string tweetText)
        {
            var tokens = tweetText
                .ToLower()
                .Replace(',', ' ')
                .Replace("@", "")
                .Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Length == 0)
                return null;

            // skip the first token if it is #OurTag or @OurApp
            if (tokens[0] == Settings.TwitterBotUsername.ToLower() || tokens[0] == "#" + Settings.TwitterHashTag.ToLower())
                tokens = tokens.Skip(1).ToArray();

            if (tokens.Length == 1)
                return null;

            switch (tokens[0])
            {
                case "hello":
                    var helloTweet = new HelloTweet();
                    foreach (var token in tokens.Skip(1))
                    {
                        if (token.StartsWith("!"))
                            helloTweet.UserType = token.Substring(1);
                        else if (token.StartsWith("#"))
                        {
                            var cleanTag = TagHelper.Clean(token);
                            if (!helloTweet.Tags.Contains(cleanTag))
                                helloTweet.Tags.Add(cleanTag);
                        }
                    }
                    return helloTweet;
                
                // Ignore other tweets in this initial version

                case "sat":
                    var sitTweet = new SatTweet();
                    sitTweet.SeatCode = tokens[1];
                    return sitTweet;
                case "claim":
                    var claimTweet = new ClaimTweet();
                    claimTweet.Token = tokens[1];
                    return claimTweet;
                case "met":
                    var metTweet = new MetTweet();
                    metTweet.Friends = new List<string>(
                        tokens
                            .Skip(1)
                            .Select(f => f)
                    );
                    return metTweet;
                case "message":
                    var messageTweet = new MessageTweet();
                    if (tweetText.StartsWith("@" + Settings.TwitterBotUsername + " "))
                        tweetText = tweetText.Substring(("@" + Settings.TwitterBotUsername + " ").Length);
                    if (tweetText.StartsWith("#" + Settings.TwitterHashTag + " "))
                        tweetText = tweetText.Substring(("#" + Settings.TwitterBotUsername + " ").Length);
                    messageTweet.Message = tweetText.Substring("message ".Length);
                    return messageTweet;
                default:
                    return null;
            }
        }
    }

    public class ProcessedTweet
    {
    }

    public class HelloTweet : ProcessedTweet
    {
        public string UserType { get; set; }
        public List<string> Tags { get; set; }

        public HelloTweet()
        {
            Tags = new List<string>();
        }
    }

    public class SatTweet : ProcessedTweet
    {
        public string SeatCode { get; set; }
    }

    public class ClaimTweet : ProcessedTweet
    {
        public string Token { get; set; }
    }

    public class MetTweet : ProcessedTweet
    {
        public List<string> Friends { get; set; }

        public MetTweet()
        {
            Friends = new List<string>();
        }
    }

    public class MessageTweet : ProcessedTweet
    {
        public string Message { get; set; }
    }
}
