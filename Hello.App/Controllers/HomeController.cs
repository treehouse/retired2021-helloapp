using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hello.Repo;

namespace Hello.App.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private HelloRepoDataContext _repo;

        public HomeController() : this(new HelloRepoDataContext()) { }

        public HomeController(HelloRepoDataContext repo)
        {
            _repo = repo;
        }

        public ActionResult Index()
        {
            var users = _repo
                .Users
                .Take(25);

            var userTypes = _repo
                .UserTypes
                .OrderBy(ut => ut.Ordering)
                .ToList();

            ViewData["UserTypes"] = userTypes;

            return View(users);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Join()
        {
            var userTypes = _repo
                .UserTypes
                .OrderBy(ut => ut.Ordering)
                .ToList();

            ViewData["UserTypes"] = userTypes;

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Join(string userType, string tag1, string tag2, string tag3)
        {
            return Redirect(
                "http://twitter.com/?status=" + 
                Url.Encode(String.Format("@{0} !{1} #{2} #{3} #{4}",
                    Settings.TwitterBotUsername,
                    userType,
                    tag1.Replace("#", String.Empty),
                    tag2.Replace("#", String.Empty),
                    tag3.Replace("#", String.Empty))));
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Faq()
        {
            return View();
        }
    }
}
