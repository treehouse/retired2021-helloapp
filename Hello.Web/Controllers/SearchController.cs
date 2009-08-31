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
    public class SearchController : HelloBaseController
    {
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
                return View(users);
            }
        }
    }
}
