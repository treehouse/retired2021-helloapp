using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Hello.App.Controllers;

namespace Hello.App.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static string BodyClass(this HtmlHelper html)
        {
            if (html.ViewContext.Controller is HomeController)
            {
                var action = html.ViewContext.RouteData.Values["action"].ToString().ToLower();
                return action == "index" ? "home" : action;
            }
            else
                return String.Empty;
        }
    }
}
