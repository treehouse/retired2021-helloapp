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
        public static string BodyID(this HtmlHelper html)
        {
            var controller = GetControllerName(html);
            var action = GetActionName(html);

            switch (controller)
            {
                case "home":
                    switch (action)
                    {
                        case "index":
                            return "home";
                        case "faq":
                            return "instructions";
                        default:
                            return action;
                    }
                case "event":
                    switch (action)
                    {
                        case "index":
                            return "home";
                        case "search" :
                            return "home";
                        default:
                            return action;
                    }
                default:
                    return String.Empty;
            }
        }

        public static string BodyClass(this HtmlHelper html)
        {
            var controller = GetControllerName(html);
            var action = GetActionName(html);

            switch (controller)
            {
                case "event":
                    return "confDay";
                default:
                    return String.Empty;
            }
        }

        public static string Title(this HtmlHelper html)
        {
            var title = "Hello";
            var controller = GetControllerName(html);
            var action = GetActionName(html);

            switch (controller)
            {
                case "home":
                    switch (action)
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

        private static string GetActionName(HtmlHelper html)
        {
            var action = html.ViewContext.RouteData.Values["action"].ToString().ToLower();
            return action;
        }

        private static string GetControllerName(HtmlHelper html)
        {
            var controller = html.ViewContext.RouteData.Values["controller"].ToString().ToLower();
            return controller;
        }
    }
}
