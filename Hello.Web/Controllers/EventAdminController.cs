using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Hello.Repo;
using Hello.Utils;

namespace Hello.Web.Controllers
{
    [Authorize]
    public class EventAdminController : Controller
    {
        private HelloRepoDataContext _repo;

        public EventAdminController() : this(new HelloRepoDataContext(Settings.ConnectionString)) { }

        public EventAdminController(HelloRepoDataContext repo)
        {
            _repo = repo;
        }

        public ActionResult Index()
        {
            var events = _repo.Events;
            return View(events);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Create()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Event theEvent)
        {
            if (ModelState.IsValid)
            {
                _repo
                    .Events
                    .InsertOnSubmit(theEvent);
                _repo.SubmitChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View(theEvent);
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(int id)
        {
            var theEvent = _repo
                .Events
                .SingleOrDefault(e => e.EventID == id);
            if (theEvent == null)
                return RedirectToAction("Index");
            else
                return View(theEvent);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, Event theEvent)
        {
            if (ModelState.IsValid)
            {
                var originalEvent = _repo
                    .Events
                    .Single(e => e.EventID == theEvent.EventID);
                
                // Copy across changes
                originalEvent.Name = theEvent.Name;
                originalEvent.Slug = theEvent.Slug;
                originalEvent.Start = theEvent.Start;
                originalEvent.End = theEvent.End;
                originalEvent.HiFiveLimit = theEvent.HiFiveLimit;

                _repo.SubmitChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View(theEvent);
            }
        }
    }
}
