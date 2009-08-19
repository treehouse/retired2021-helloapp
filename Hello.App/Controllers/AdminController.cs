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

        public ActionResult Messages()
        {
            var messages = _repo.Messages;
            return View(messages);
        }

        public ActionResult Events()
        {
            var events = _repo.Events;
            return View(events);
        }
    }
}
