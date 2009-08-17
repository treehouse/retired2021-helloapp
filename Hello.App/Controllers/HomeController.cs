using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hello.Repo;
using Hello.Utils;
using System.Text;

namespace Hello.App.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private HelloRepoDataContext _repo;

        public HomeController() : this(new HelloRepoDataContext(Settings.ConnectionString)) { }

        public HomeController(HelloRepoDataContext repo)
        {
            _repo = repo;
        }

        public ActionResult Index()
        {
            var users = _repo
                .Users
                .Where(u => !u.ShadowAccount)
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
            var twitterUrl = new StringBuilder("http://twitter.com/?status=");

            twitterUrl.Append(
                Url.Encode("@" + Settings.TwitterBotUsername + " hello !" + userType));

            if (!String.IsNullOrEmpty(tag1))
                twitterUrl.Append(
                    Url.Encode(" #" + TagHelper.Clean(tag1)));

            if (!String.IsNullOrEmpty(tag2))
                twitterUrl.Append(
                    Url.Encode(" #" + TagHelper.Clean(tag2)));

            if (!String.IsNullOrEmpty(tag3))
                twitterUrl.Append(
                    Url.Encode(" #" + TagHelper.Clean(tag3)));

            return Redirect(twitterUrl.ToString());
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
