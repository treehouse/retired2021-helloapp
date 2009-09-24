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

            try
            {
                var queue = args.Length == 0 || args[0].Equals("queue", StringComparison.InvariantCultureIgnoreCase);
                var process = args.Length == 0 || args[0].Equals("process", StringComparison.InvariantCultureIgnoreCase);

                HelloRepoDataContext repo = new HelloRepoDataContext(Settings.ConnectionString);
                TweetQueuer queuer = new TweetQueuer(repo);
                TweetProcessor processor = new TweetProcessor(repo);

                // Collect & store tweets
                if (queue)
                {
                    _log.Info("About to queue tweets");
                    try
                    {
                        queuer.QueueMentions();
                    }
                    catch (WebException e)
                    {
                        _log.Error("WebException in Engine.QueueMentions", e);
                    }
                    catch (Exception e)
                    {
                        _log.Fatal("Exception while Queueing", e);
                    }
                    _log.Info("Done queueing tweets");
                }

                // Process tweets
                if (process)
                {
                    _log.Info("About to process tweets");
                    try
                    {
                        processor.ProcessTweets();
                    }
                    catch (Exception e)
                    {
                        _log.Fatal("Exception while Processing", e);
                    }
                    _log.Info("Done processing tweets");
                }
            }
            catch (Exception e)
            {
                _log.Fatal("Unhandled Exception", e);
            }
        }
    }
}
