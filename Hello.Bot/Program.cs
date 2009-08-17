using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using Hello.Repo;
using System.Net;
using log4net;
using Hello.Utils;

namespace Hello.Bot
{
    public class Program
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Program));

        public static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            HelloRepoDataContext repo = new HelloRepoDataContext(Settings.ConnectionString);
            TweetQueuer queuer = new TweetQueuer(repo);
            TweetProcessor processor = new TweetProcessor(repo);

            // Collect & store tweets
            try
            {
                queuer.QueueMentions();
            }
            catch (WebException e)
            {
                _log.Error("WebException in Engine.QueueMentions", e);
            }

            // Process tweets
            processor.ProcessTweets();
        }
    }
}
