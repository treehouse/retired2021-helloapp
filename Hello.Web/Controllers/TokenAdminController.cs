using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Hello.Utils;
using Hello.Repo;

namespace Hello.Web.Controllers
{
    public class TokenAdminController : HelloBaseController
    {
        public ActionResult Index(int id)
        {
            var campaign = _repo
               .Campaigns
               .SingleOrDefault(e => e.CampaignID == id);

            if (campaign == null)
                return RedirectToAction("Index", "CampaignAdmin");

            ViewData["Campaign"] = campaign;

            var tokens = _repo
                .Tokens
                .Where(t => t.CampaignID == id);

            return View(tokens);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Create(int id)
        {
            var campaign = _repo
               .Campaigns
               .SingleOrDefault(e => e.CampaignID == id);

            if (campaign == null)
                return RedirectToAction("Index", "CampaignAdmin");

            ViewData["Campaign"] = campaign;

            ViewData["Code"] = CodeHelper.GenerateTokenCode();
            
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(int id, Token token)
        {
            var campaign = _repo
                .Campaigns
                .SingleOrDefault(c => c.CampaignID == id);

            if (campaign == null)
                return RedirectToAction("Index", "CampaignAdmin");

            if (ModelState.IsValid)
            {
                token.CampaignID = id;

                _repo
                    .Tokens
                    .InsertOnSubmit(token);
                _repo.SubmitChanges();

                return RedirectToAction("Index", new { id = campaign.CampaignID });
            }
            else
            {
                ViewData["Campaign"] = campaign;

                return View(token);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id)
        {
            var token = _repo
                .Tokens
                .SingleOrDefault(t => t.TokenID == id);

            if (token == null)
                return RedirectToAction("Index", "CampaignAdmin");

            _repo
                .Tokens
                .DeleteOnSubmit(token);
            _repo.SubmitChanges();

            return RedirectToAction("Index", new { id = token.CampaignID });
        }
    }
}
