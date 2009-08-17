using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hello.Bot.TweetTypes
{
    public class MetTweet : ProcessedTweet
    {
        public List<string> Friends { get; set; }

        public MetTweet()
        {
            Friends = new List<string>();
        }

        public void AddFriends(IEnumerable<string> friends)
        {
            foreach (string friend in friends)
            {
                Friends.Add(friend);
            }
        }
    }
}
