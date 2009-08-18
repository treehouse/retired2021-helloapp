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

                if (action == "index")
                    return "home";
                else if (action == "faq")
                    return "instructions";
                else
                    return action;
            }
            else
                return String.Empty;
        }

        public static string Title(this HtmlHelper html)
        {
            string title = "Hello";

            switch (html.ViewContext.RouteData.Values["controller"].ToString().ToLower())
            {
                case "home":
                    switch (html.ViewContext.RouteData.Values["action"].ToString().ToLower())
                    {
                        case "index":
                            title += " » Home";
                            break;
                        case "about":
                            title += " » About";
                            break;
                        case "faq":
                            title += " » Instructions";
                            break;
                        case "join":
                            title += " » Join";
                            break;
                        default:
                            break;
                    }
                    break;
                case "search":
                    title += " » Search";
                    break;
                default:
                    break;
            }
            return html.Encode(title);
        }
    }
}
