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

            BuildViewData(theEvent);

            return View(theEvent);
        }

        private void BuildViewData(Event theEvent)
        {
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

            // Get the last session that has started
            var session = _repo
                .Sessions
                .Where(s => s.EventID == theEvent.EventID
                        && s.Start < DateTime.Now)
                .OrderByDescending(s => s.Start)
                .FirstOrDefault();

            // Who is sitting where?
            var sats = _repo
                .Sats
                .Where(s => s.Session.EventID == theEvent.EventID
                        && s.SessionID == session.SessionID);

            ViewData["Sats"] = sats;

            // Get the tags for this event
            var tags = _repo
                .Tags
                .Where(t => t.User.Sats
                    .Any(s => s.SessionID == session.SessionID))
                .GroupBy(t => t.Name)
                .OrderByDescending(t => t.Count())
                .Take(Settings.MaxTags)
                .ToList();

            var rankedTags = new Dictionary<string, string>();

            var i = Settings.MaxTags;

            foreach (var tag in tags)
            {
                var tagSize = String.Empty;
                if (!Settings.TagSizes.TryGetValue(i--, out tagSize))
                {
                    tagSize = Settings.DefaultTagSize;
                }
                rankedTags.Add(tag.Key, tagSize);
            }

            ViewData["Tags"] = rankedTags;
            ViewData["TagsKeys"] = rankedTags.Keys.OrderBy(t => t);
        }

        public ActionResult Search(string eventslug, string searchterm, string viewBy)
        {
            ViewData["SearchTerm"] = searchterm;
            ViewData["ViewBy"] = viewBy;

            var theEvent = _repo
                .Events
                .SingleOrDefault(e => e.Slug == eventslug);

            // if the event doesn't exist or starts in the future then redirect
            if (theEvent == null || theEvent.Start > DateTime.Now)
                return RedirectToAction("Index", "Home");

            BuildViewData(theEvent);

            return View("Index", theEvent);
        }
    }
}
