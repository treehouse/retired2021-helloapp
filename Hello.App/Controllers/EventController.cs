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
            var theEvent = _repo
                .Events
                .SingleOrDefault(e => e.Slug == eventslug);

            // if the event doesn't exist or starts in the future then redirect
            if (theEvent == null || theEvent.Start > DateTime.Now)
                return RedirectToAction("Index", "Home");

            // Get the messages in a random order
            var randomOffset = new Random(DateTime.Now.Millisecond).Next(1000);

            var messages = _repo
                .Messages
                .Where(m => !m.Offensive
                    && m.User.Points.Sum(p => p.Amount) > Settings.Thresholds.Silver)
                .OrderBy(m => (m.User.Created.Millisecond * randomOffset) % 1000)
                .Take(Settings.MaxMessages);

            ViewData["Message"] = messages.FirstOrDefault();
            ViewData["Messages"] = messages;

            // Who is sitting where?
            var sats = _repo
                .Sats
                .Where(s => s.Session.EventID == theEvent.EventID);

            ViewData["Sats"] = sats;

            // Get the tags for this event
            var tags = _repo
                .Tags
                .Where(t => t.User.Sats
                    .Any(s => s.Seat.EventID == theEvent.EventID));

            ViewData["Tags"] = tags;

            return View(theEvent);
        }

        public ActionResult Search(string searchterm)
        {
            return Content(searchterm);
        }
    }
}
