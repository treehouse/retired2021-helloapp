using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HelloApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(null,
                "user/random{format}",
                new { controller = "User", action = "Random" }
                //, new { username = "[fdas]+" }
            );

            //routes.MapRoute(null,
            //    "user/{username}",
            //    new { controller = "User", action = "Index", format = "html" }
            //    //, new { username = "[fdas]+" }
            //);

            routes.MapRoute(null,
                "{eventslug}",
                new { controller = "Event", action = "Index" },
                new { eventslug = @"[0-9a-fA-F\-]+" }
            );

            routes.MapRoute(null,
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = "" }
            );
        }

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
        }
    }
}