using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Hello.Utils;

namespace Hello.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            routes.MapRoute(null,
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = "" },
                new { controller = "(Home)|(Search)|(Profile)|(Admin)|(Account)|(EventAdmin)|(SessionAdmin)|(CampaignAdmin)|(TokenAdmin)" }
            );

            routes.MapRoute(null,
                "{eventslug}/{action}",
                new { controller = "Event", action = "Index" },
                new { eventslug = Settings.EventSlugRegex }
            );
        }

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
        }
    }
}