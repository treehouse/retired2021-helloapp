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
    public class ProfileController : HelloBaseController
    {
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index(string username)
        {
            Repo.User user = null;
            
            if (!String.IsNullOrEmpty(username))
                user = _repo.Users.SingleOrDefault(u => u.Username == username);

            if (user != null)
            {
                int currentPointsTotal = user.CurrentPointsTotal;
                ViewData["pointsTotal"] = currentPointsTotal;

                var redeemedTokens = user.Redemptions.OrderBy(r => r.Created).Select(r => r.Token).ToList();
                ViewData["redeemedTokens"] = redeemedTokens;

                ViewData["currentEvent"] = CurrentEvent.Slug;
                ViewData["tags"] = user.Tags.OrderByDescending(t => t.Created)
                    .Select(t => t.Name).Take(3).ToList();

                var friends = user.Friends
                    .Select(f => f).ToList();

                var friendsNames = friends.Select(f => f.Befriender).ToList();

                var following = _repo.Friendships.Where(f => f.Befriender == user.Username)
                    .Select(f => f.Befriendee).ToList().Except(friendsNames).ToList();
                var followers = _repo.Friendships.Where(f => f.Befriendee == user.Username)
                    .Select(f => f.Befriender).ToList().Except(friendsNames).ToList();

                ViewData["friends"] = friends;
                ViewData["following"] = following;
                ViewData["followers"] = followers;

                var hiFivers = _repo.HiFives.Where(h => h.HiFivee == user.Username).Select(h => h.HiFiver).ToList();
                var hiFivees = _repo.HiFives.Where(h => h.HiFiver == user.Username).Select(h => h.HiFivee).ToList();
                ViewData["hiFivers"] = hiFivers;
                ViewData["hiFivees"] = hiFivees;
                
                var userTypes = _repo
                .UserTypes
                .OrderBy(ut => ut.Ordering)
                .ToList();
                ViewData["UserTypes"] = userTypes;

                return View(user);
            }
            return View("NotFound", null);
        }
    }
}