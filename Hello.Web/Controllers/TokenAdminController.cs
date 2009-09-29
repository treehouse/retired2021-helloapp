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

            if (ModelState.IsValid && (Request.Files.Count != 0 || Request.Files[0].ContentLength != 0))
            {
                token.CampaignID = id;

                HttpPostedFileBase tokenImage = Request.Files[0];

                String tokenImagesDir = HttpContext.Server.MapPath("~/Content/images/tokens/");
                
                String fileName = String.Format("{0}-{1}", token.CampaignID, System.IO.Path.GetFileName(tokenImage.FileName));
                String fullFileName = String.Format("{0}{1}", tokenImagesDir, fileName);
                
                tokenImage.SaveAs(fullFileName);

                token.FileName = fileName;

                _repo.Tokens.InsertOnSubmit(token);
                
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
