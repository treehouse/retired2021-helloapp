using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Hello.Repo;
using Hello.Utils;
using System.Text.RegularExpressions;

namespace Hello.Web.Controllers
{
    [Authorize]
    public class AdminController : HelloBaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Messages()
        {
            var messages = _repo.Messages;
            return View(messages);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Messages(string username)
        {
            var message = _repo
                .Messages
                .SingleOrDefault(m => m.Username == username);

            if (message != null)
            {
                message.Offensive = !message.Offensive;
                _repo.SubmitChanges();
            }

            return View();
        }

        public ActionResult Status()
        {
            var processedTweets = _repo
                .QueuedTweets
                .Count(t => t.Processed);
            ViewData["ProcessedTweets"] = processedTweets;

            var unprocessedTweets = _repo
                .QueuedTweets
                .Count(t => !t.Processed);
            ViewData["UnprocessedTweets"] = unprocessedTweets;

            var tideMarks = _repo
                .TideMarks
                .OrderByDescending(t => t.TimeStamp)
                .Take(Settings.Admin.MaxTideMarks);

            return View(tideMarks);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Tokens(int id, Token token)
        {
            var campaign = _repo
                .Campaigns
                .SingleOrDefault(c => c.CampaignID == id);

            if (campaign == null)
                return RedirectToAction("Campaigns");

            _repo
                .Tokens
                .InsertOnSubmit(token);
            _repo.SubmitChanges();

            return RedirectToAction("Tokens");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteToken(int id)
        {
            var token = _repo
                .Tokens
                .SingleOrDefault(t => t.TokenID == id);

            if (token == null)
                return RedirectToAction("Campaigns");

            _repo
                .Tokens
                .DeleteOnSubmit(token);
            _repo.SubmitChanges();

            return RedirectToAction("Tokens", new { id = token.CampaignID });
        }
    }
}
