using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Hello.Repo;

namespace Hello.Web.Controllers
{
    [Authorize]
    public class SessionAdminController : HelloBaseController
    {
        public ActionResult Index(int id)
        {
            var theEvent = _repo
                .Events
                .SingleOrDefault(e => e.EventID == id);

            if (theEvent == null)
                return RedirectToAction("Index", "EventAdmin");

            ViewData["Event"] = theEvent;

            var sessions = _repo
                .Sessions
                .Where(s => s.EventID == theEvent.EventID);
            return View(sessions);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Create(int id)
        {
            var theEvent = _repo
                .Events
                .SingleOrDefault(e => e.EventID == id);

            if (theEvent == null)
                return RedirectToAction("Index", "EventAdmin");

            ViewData["Event"] = theEvent;

            return View();
        } 

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(int id, Session session)
        {
            var theEvent = _repo
               .Events
               .SingleOrDefault(e => e.EventID == id);

            if (theEvent == null)
                return RedirectToAction("Index", "EventAdmin");

            if (ModelState.IsValid)
            {
                session.EventID = id;
                _repo
                    .Sessions
                    .InsertOnSubmit(session);
                _repo.SubmitChanges();

                return RedirectToAction("Index", new { id = id });
            }
            else
            {
                ViewData["Event"] = theEvent;

                return View(session);
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(int id)
        {
            var session = _repo
                .Sessions
                .SingleOrDefault(s => s.SessionID == id);
            if (session == null)
                return RedirectToAction("Index");
            else
                return View(session);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, Session session)
        {
            var originalSession = _repo
                .Sessions
                .Single(s => s.SessionID == id);

            // Copy across changes
            originalSession.Name = session.Name;
            originalSession.Start = session.Start;
            originalSession.Finish = session.Finish;

            if (ModelState.IsValid)
            {
                _repo.SubmitChanges();

                return RedirectToAction("Index", new { id = originalSession.EventID });
            }
            else
                return View(originalSession);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id)
        {
            var session = _repo
                .Sessions
                .SingleOrDefault(s => s.SessionID == id);

            if (session == null)
                return RedirectToAction("Index", "EventAdmin");

            _repo
                .Sessions
                .DeleteOnSubmit(session);
            _repo.SubmitChanges();

            return RedirectToAction("Index", new { id = session.EventID });
        }
    }
}
