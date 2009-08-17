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

            var queue = args.Length == 0 || args[0].Equals("queue", StringComparison.InvariantCultureIgnoreCase);
            var process = args.Length == 0 || args[0].Equals("process", StringComparison.InvariantCultureIgnoreCase);

            HelloRepoDataContext repo = new HelloRepoDataContext(Settings.ConnectionString);
            TweetQueuer queuer = new TweetQueuer(repo);
            TweetProcessor processor = new TweetProcessor(repo);

            // Collect & store tweets
            if (queue)
            {
                _log.Info("About to queue Mentions");
                try
                {
                    queuer.QueueMentions();
                }
                catch (WebException e)
                {
                    _log.Error("WebException in Engine.QueueMentions", e);
                }
            }

            if (process)
            {
                _log.Info("About to process Tweets");
                // Process tweets
                processor.ProcessTweets();
            }
        }
    }
}
