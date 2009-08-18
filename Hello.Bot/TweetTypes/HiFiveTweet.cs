using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hello.Bot.TweetTypes
{
    public class HiFiveTweet : ProcessedTweet
    {
        public string Friend { get; set; }

        public HiFiveTweet(string friend)
        {
            Friend = friend;
        }
    }
}
