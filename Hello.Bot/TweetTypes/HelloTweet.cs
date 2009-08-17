using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hello.Bot.TweetTypes
{
    public class HelloTweet : ProcessedTweet
    {
        public string UserType { get; set; }
        public List<string> Tags { get; set; }

        public HelloTweet()
        {
            Tags = new List<string>();
        }
    }
}
