using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Security;

namespace Hello.Web.Controllers
{
    public class AccountController : Controller
    {
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult LogOn()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LogOn(string name, string password, string returnUrl)
        {
            if (FormsAuthentication.Authenticate(name, password))
            {
                // Assign a default redirection destination if not set
                returnUrl = returnUrl ?? Url.Action("Index", "Admin");
                // Grant cookie and redirect
                FormsAuthentication.SetAuthCookie(name, false);
                return Redirect(returnUrl);
            }
            else
            {
                ViewData["lastLoginFailed"] = true;
                return View();
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("LogOn", "Account");
        }
    }
}
