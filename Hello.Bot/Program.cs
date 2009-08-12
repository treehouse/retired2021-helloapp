using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using Hello.Repo;

namespace Hello.Bot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var engine = new Engine(new HelloRepoDataContext());

            // Collect & store tweets
            engine.QueueDirectMessages();
            engine.QueueMentions();
            engine.QueueHashTagged();

            // Process tweets
            engine.ProcessTweets();
        }
    }
}
