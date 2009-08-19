using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Hello.Repo;
using Hello.Utils;

namespace Hello.App.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private HelloRepoDataContext _repo;

        public AdminController() : this(new HelloRepoDataContext(Settings.ConnectionString)) { }

        public AdminController(HelloRepoDataContext repo)
        {
            _repo = repo;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Events()
        {
            var events = _repo.Events;
            return View(events);
        }

        public ActionResult Sessions()
        {
            var sessions = _repo.Sessions;
            return View(sessions);
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
            }
            _repo.SubmitChanges();

            return RedirectToAction("Messages");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Campaigns()
        {
            var campaigns = _repo.Campaigns;
            return View(campaigns);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Campaigns(Campaign campaign)
        {
            _repo.Campaigns.InsertOnSubmit(campaign);
            _repo.SubmitChanges();
            return RedirectToAction("Campaigns");
        }

        public ActionResult Tokens(int id)
        {
            var campaign = _repo
                .Campaigns
                .SingleOrDefault(c => c.CampaignID == id);

            ViewData["Campaign"] = campaign;

            var tokens = _repo
                .Tokens
                .Where(t => t.CampaignID == id);

            return View(tokens);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int campaignID)
        {
            var campaign = _repo
                .Campaigns
                .SingleOrDefault(c => c.CampaignID == campaignID);
            _repo
                .Campaigns
                .DeleteOnSubmit(campaign);
            _repo.SubmitChanges();

            return RedirectToAction("Campaigns");
        }
    }
}
