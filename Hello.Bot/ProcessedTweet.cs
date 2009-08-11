using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hello.Repo;

namespace Hello.Bot
{
    public class ProcessedTweet
    {
        public string UserType { get; set; }
        public List<string> Tags { get; set; }
        public string SeatCode { get; set; }
        public List<string> Friends { get; set; }
        public string Token { get; set; }

        public ProcessedTweet()
        {
            Tags = new List<string>();
            Friends = new List<string>();
        }

        public ProcessedTweet(string tweetText) : this()
        {
            var tokens = tweetText
                .ToLower()
                .Replace(',', ' ')
                .Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Length == 0)
                return;

            if (tokens[0].Equals("met"))
            {
                Friends = new List<string>(
                    tokens
                        .Skip(1)
                        .Select(f => f.Replace("@", ""))
                );
            }

            foreach (var token in tokens)
            {
                if (token.StartsWith("!"))
                    UserType = token.Substring(1);
                else if (token.StartsWith("#"))
                    Tags.Add(token.Substring(1));
                else if (token.Length <= 5)
                    SeatCode = token;
                else if (token.Length <= 10)
                    Token = token;
            }
        }
    }
}
