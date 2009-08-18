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
    public class SearchController : Controller
    {
        private HelloRepoDataContext _repo;

        public SearchController() : this(new HelloRepoDataContext(Settings.ConnectionString)) { }

        public SearchController(HelloRepoDataContext repo)
        {
            _repo = repo;
        }

        public ActionResult Index(string search)
        {
            search = search.Trim();

            List<UserType> userTypes = _repo
                .UserTypes
                .OrderBy(ut => ut.Ordering)
                .ToList();

            ViewData["UserTypes"] = userTypes;
            ViewData["SearchTerm"] = search;
            
            if (String.IsNullOrEmpty(search))
            {
                return View();
            }
            else
            {
                List<User> users = _repo.SearchUsers(search, Settings.MaxSearchResults).ToList();
                /*var users = _repo
                    .Users
                    .Where(
                        u => (u.Username.Contains(search)
                           || u.Tags.Any(t => t.Name.Contains(search))
                           || u.UserTypeID.Contains(search))
                           && !u.ShadowAccount)
                    .Take(Settings.MaxSearchResults);*/

                return View(users);
            }
        }
    }
}
