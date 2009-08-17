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

            var repo = new HelloRepoDataContext(Settings.ConnectionString);
            var engine = new TweetQueuer(repo);

            // Collect & store tweets
            
            //try
            //{
            //    engine.QueueHashTagged();
            //}
            //catch (WebException e)
            //{
            //    _log.Error("WebException in Engine.QueueHashTagged", e);
            //}

            try
            {
                engine.QueueMentions();
            }
            catch (WebException e)
            {
                _log.Error("WebException in Engine.QueueMentions", e);
            }
            
            //try
            //{
            //    engine.QueueDirectMessages();
            //}
            //catch (WebException e)
            //{
            //    _log.Error("WebException in Engine.QueueDirectMessages", e);
            //}

            // Process tweets
            engine.ProcessTweets();

            //Console.WriteLine("Done");
            //Console.ReadKey();
        }
    }
}
