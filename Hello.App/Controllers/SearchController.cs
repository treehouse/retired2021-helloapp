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

            var userTypes = _repo
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
                // TODO: Split search terms and return something sensible...

                //var searchTerms = search.Split(' ');

                //foreach (var term in searchTerms)
                //{
                //    users = _repo
                //        .Users
                //        .Select(u => new
                //        {
                //            User = u,
                //            Weight = ...
                //        });
                //}

                var users = _repo
                    .Users
                    .Where(
                        u => (u.Username.Contains(search)
                           || u.Tags.Any(t => t.Name.Contains(search))
                           || u.UserTypeID.Contains(search))
                           && !u.ShadowAccount)
                    .Take(100);

                return View(users);
            }
        }
    }
}
