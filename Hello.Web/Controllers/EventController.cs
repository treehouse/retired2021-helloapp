using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Hello.Utils;
using Hello.Repo;
using System.Data.Linq;

namespace Hello.Web.Controllers
{
    public class EventController : HelloBaseController
    {
        public override void PostRepoInit()
        {
            var options = new DataLoadOptions();
            options.LoadWith<Sat>(s => s.User);
            options.LoadWith<User>(u => u.Tags);
            _repo.LoadOptions = options;
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

            // Or the first session
            if (session == null)
                session = _repo
                    .Sessions
                    .Where(s => s.EventID == theEvent.EventID)
                    .OrderBy(s => s.Start)
                    .FirstOrDefault();

            if (session == null)
                throw new HelloException("The event '" + theEvent.Name + "' has no sessions.");

            // Who is sitting where?
            var sats = _repo
                .Sats
                .Where(s => s.Session.EventID == theEvent.EventID
                        && s.SessionID == session.SessionID)
                .ToList(); // Stops the query being executed for each seat

            ViewData["Sats"] = sats;

            // Get the tags for this event
            var tags = sats
                .Select(s => s.User)
                .SelectMany(u => u.Tags)
                .GroupBy(t => t.Name)
                .OrderByDescending(t => t.Count())
                .Take(Settings.MaxTags)
                .Select(t => t.Key)
                .ToList();

            var rankedTags = new Dictionary<string, string>();

            var i = 0;

            foreach (var tag in tags)
            {
                var tagSize = String.Empty;
                if (!Settings.TagSizes.TryGetValue(i++, out tagSize))
                {
                    tagSize = Settings.DefaultTagSize;
                }
                rankedTags.Add(tag, tagSize);
            }

            ViewData["Tags"] = rankedTags;
            ViewData["TagsKeys"] = rankedTags.Keys.OrderBy(t => t);

            var userTypes = _repo
                .UserTypes
                .OrderBy(ut => ut.Ordering)
                .ToList();

            ViewData["UserTypes"] = userTypes;
        }

        public ActionResult Search(string eventslug, string searchTerm, string viewBy)
        {
            if (searchTerm != null)
                searchTerm = searchTerm.Trim();
            ViewData["SearchTerm"] = searchTerm;
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
