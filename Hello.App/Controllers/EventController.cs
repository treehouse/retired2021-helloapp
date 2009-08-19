using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Hello.Utils;
using Hello.Repo;

namespace Hello.App.Controllers
{
    public class EventController : Controller
    {
        private HelloRepoDataContext _repo;

        public EventController() : this(new HelloRepoDataContext(Settings.ConnectionString)) { }

        public EventController(HelloRepoDataContext repo)
        {
            _repo = repo;
        }

        public ActionResult Index(string eventslug)
        {
            // Get the messages in a random order
            var randomOffset = new Random(DateTime.Now.Millisecond).Next(1000);

            var messages = _repo
                .Messages
                .Where(m => !m.Offensive
                    && m.User.Points.Sum(p => p.Amount) > Settings.Thresholds.Silver)
                .OrderBy(m => (m.User.Created.Millisecond * randomOffset) % 1000);

            ViewData["Message"] = messages.FirstOrDefault();
            ViewData["Messages"] = messages;

            var theEvent = _repo
                .Events
                .SingleOrDefault(e => e.Slug == eventslug);

            // if the event doesn't exist or starts in the future then redirect
            if (theEvent == null || theEvent.Start > DateTime.Now)
                return RedirectToAction("Index", "Home");

            return View(theEvent);
        }
    }
}
