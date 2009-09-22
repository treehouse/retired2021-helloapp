using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Hello.Repo;

namespace Hello.Web.Controllers
{
    [Authorize]
    public class CampaignAdminController : HelloBaseController
    {
        public ActionResult Index()
        {
            var campaigns = _repo.Campaigns;
            return View(campaigns);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Create()
        {
            return View();
        } 

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Campaign campaign)
        {
            if (ModelState.IsValid)
            {
                _repo
                    .Campaigns
                    .InsertOnSubmit(campaign);
                _repo.SubmitChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View(campaign);
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(int id)
        {
            var campaign = _repo
               .Campaigns
               .SingleOrDefault(e => e.CampaignID == id);

            if (campaign == null)
                return RedirectToAction("Index");

            return View(campaign);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Campaign campaign)
        {
            if (ModelState.IsValid)
            {
                var originalCampaign = _repo
                    .Campaigns
                    .Single(e => e.CampaignID == campaign.CampaignID);

                // Copy across changes
                originalCampaign.Name = campaign.Name;
                originalCampaign.Value = campaign.Value;

                _repo.SubmitChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View(campaign);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id)
        {
            var campaign = _repo
                .Campaigns
                .SingleOrDefault(c => c.CampaignID == id);

            if (campaign == null)
                return RedirectToAction("Index");

            _repo
                .Tokens
                .DeleteAllOnSubmit(campaign.Tokens);
            _repo
                .Campaigns
                .DeleteOnSubmit(campaign);
            _repo.SubmitChanges();

            return RedirectToAction("Index");
        }
    }
}
