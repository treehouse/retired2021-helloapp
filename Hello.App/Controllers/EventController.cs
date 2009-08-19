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

            return View(theEvent);
        }

    }
}
