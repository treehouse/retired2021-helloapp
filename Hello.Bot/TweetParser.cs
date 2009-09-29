using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Text.RegularExpressions;
using Hello.Utils;
using Hello.Bot.TweetTypes;
using Hello.Repo;

namespace Hello.Bot
{
    public static class TweetParser
    {
        public static ProcessedTweet Parse(QueuedTweet tweet)
        {
            // ignore our tweets and vias
            if (tweet.Username == Settings.TwitterBotUsername
                || tweet.Message.Contains("via @"))
            {
                return null;
            }

            string[] tokens = tweet
                .Message
                .ToLower()
                .Replace(',', ' ')
                .Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Length == 0)
                return null;

            // skip the first token if it is #OurTag or @OurApp
            if (tokens[0].Equals("@" + Settings.TwitterBotUsername, StringComparison.InvariantCultureIgnoreCase)
                || tokens[0].Equals("#" + Settings.TwitterHashTag, StringComparison.InvariantCultureIgnoreCase))
                tokens = tokens.Skip(1).ToArray();

            if (tokens.Length == 1)
                return null;

            switch (tokens[0])
            {
                case "hello":
                    HelloTweet helloTweet = new HelloTweet();
                    foreach (string token in tokens.Skip(1))
                    {
                        if (token.StartsWith("!"))
                            helloTweet.UserType = token.Substring(1);
                        else if (token.StartsWith("#"))
                        {
                            string cleanTag = TagHelper.Clean(token);
                            if (!helloTweet.Tags.Contains(cleanTag) && !String.IsNullOrEmpty(cleanTag))
                                helloTweet.Tags.Add(cleanTag);
                        }
                    }
                    return helloTweet;
                case "sat":
                    SatTweet sitTweet = new SatTweet();
                    sitTweet.SeatCode = tokens[1];
                    return sitTweet;
                case "claim":
                    ClaimTweet claimTweet = new ClaimTweet();
                    claimTweet.Token = tokens[1];
                    return claimTweet;
                case "met":
                    MetTweet metTweet = new MetTweet();
                    metTweet.AddFriends(tokens.Skip(1));
                    return metTweet;
                case "hi5":
                    var hifivee = tokens.Skip(1).FirstOrDefault().Trim();
                    if (hifivee.StartsWith("@"))
                        hifivee = hifivee.Substring(1);
                    HiFiveTweet hiFiveTweet = new HiFiveTweet(hifivee);
                    return hiFiveTweet;
                case "message":
                    MessageTweet messageTweet = new MessageTweet { Message = tweet.Message };
                    if (messageTweet.Message.StartsWith("@" + Settings.TwitterBotUsername + " ", StringComparison.InvariantCultureIgnoreCase))
                        messageTweet.Message = messageTweet.Message.Substring(("@" + Settings.TwitterBotUsername + " ").Length);
                    if (messageTweet.Message.StartsWith("#" + Settings.TwitterHashTag + " ", StringComparison.InvariantCultureIgnoreCase))
                        messageTweet.Message = messageTweet.Message.Substring(("#" + Settings.TwitterHashTag + " ").Length);
                    messageTweet.Message = messageTweet.Message.Substring("message ".Length).Trim().TrimEnd(new[] { '.' });
                    if (messageTweet.Message.StartsWith("\"") && messageTweet.Message.EndsWith("\""))
                        messageTweet.Message = messageTweet.Message.Substring(1, messageTweet.Message.Length - 2);
                    if (messageTweet.Message.StartsWith("'") && messageTweet.Message.EndsWith("'"))
                        messageTweet.Message = messageTweet.Message.Substring(1, messageTweet.Message.Length - 2);
                    return messageTweet;
                default:
                    return null;
            }
        }
    }
}
