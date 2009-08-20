using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Hello.Repo;
using Hello.Utils;
using System.Text;

namespace Hello.App.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private HelloRepoDataContext _repo;

        public AdminController() : this(new HelloRepoDataContext(Settings.ConnectionString)) { }

        public AdminController(HelloRepoDataContext repo)
        {
            _repo = repo;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Events()
        {
            var events = _repo.Events;
            return View(events);
        }

        public ActionResult Sessions(int id)
        {
            var theEvent = _repo
                .Events
                .SingleOrDefault(e => e.EventID == id);

            if (theEvent == null)
                return RedirectToAction("Events");

            ViewData["Event"] = theEvent;

            var sessions = _repo.Sessions;
            return View(sessions);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Messages()
        {
            var messages = _repo.Messages;
            return View(messages);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Messages(string username)
        {
            var message = _repo
                .Messages
                .SingleOrDefault(m => m.Username == username);

            if (message != null)
            {
                message.Offensive = !message.Offensive;
            }
            _repo.SubmitChanges();

            return RedirectToAction("Messages");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Campaigns()
        {
            var campaigns = _repo.Campaigns;
            return View(campaigns);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Campaigns(Campaign campaign)
        {
            _repo.Campaigns.InsertOnSubmit(campaign);
            _repo.SubmitChanges();
            return RedirectToAction("Campaigns");
        }

        public ActionResult Tokens(int id)
        {
            var campaign = _repo
                .Campaigns
                .SingleOrDefault(c => c.CampaignID == id);

            if (campaign == null)
                return RedirectToAction("Campaigns");

            ViewData["Campaign"] = campaign;

            var tokens = _repo
                .Tokens
                .Where(t => t.CampaignID == id);

            return View(tokens);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int campaignID)
        {
            var campaign = _repo
                .Campaigns
                .SingleOrDefault(c => c.CampaignID == campaignID);

            if (campaign == null)
                return RedirectToAction("Campaigns");

            _repo
                .Campaigns
                .DeleteOnSubmit(campaign);
            _repo.SubmitChanges();

            return RedirectToAction("Campaigns");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Seating(int id)
        {
            var theEvent = _repo
                .Events
                .SingleOrDefault(e => e.EventID == id);

            if (theEvent == null)
                return RedirectToAction("Events");

            ViewData["Event"] = theEvent;

            // Visual representation of the seating plan
            var seatingPlan = String.Empty;

            foreach (var row in theEvent.Seats.GroupBy(s => s.Row))
            {

                foreach (var seat in row)
                {
                    if (seat.Code == null)
                        seatingPlan += ".";
                    else
                        seatingPlan += "x";
                }
                seatingPlan += Environment.NewLine;
            }

            ViewData["SeatingPlan"] = seatingPlan;

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Seating(int id, string seating)
        {
            // Get Event
            var theEvent = _repo
                .Events
                .SingleOrDefault(e => e.EventID == id);

            if (theEvent == null)
                return RedirectToAction("Events");

            var rows = seating.Split(new [] { Environment.NewLine }, StringSplitOptions.None);

            var seats = new List<Seat>();
            var seatCodes = new List<string>();
            var random = new Random();

            for (int i = 0; i < rows.Count(); i++)
            {
                var row = rows[i].ToLower();

                for (int j = 0; j < row.Length; j++)
                {
                    var seatChar = row[j];

                    var seat = new Seat { Row = i, Column = j, EventID = theEvent.EventID };
                    seats.Add(seat);

                    switch (seatChar)
                    {
                        case 'x':
                            seat.Code = GenerateUniqueSeatCode(seatCodes, random);
                            break;
                        case '.':
                            break;
                        default:
                            break;
                    }
                }
            }

            // Delete the old seating plan
            _repo
                .Sats
                .DeleteAllOnSubmit(theEvent
                    .Seats
                    .SelectMany(s => s.Sats));
            _repo
                .Seats
                .DeleteAllOnSubmit(theEvent.Seats);

            // Insert the new seating plan
            _repo
                .Seats
                .InsertAllOnSubmit(seats);

            _repo.SubmitChanges();

            return RedirectToAction("Seating", new { id = theEvent.EventID });
        }

        private string GenerateUniqueSeatCode(List<string> seatCodes, Random r)
        {
            var code = String.Empty;
            do
            {
                code = GenerateSeatCode(r);
            }
            while (seatCodes.Contains(code));

            seatCodes.Add(code);

            return code;
        }

        private static string GenerateSeatCode(Random r)
        {
            var code = new StringBuilder(5);

            for (int i = 0; i < 5; i++)
            {
                var c = r.Next(36) + 48;
                if (c > 57)
                    c += 7;
                code.Append((char)c);
            }

            return code.ToString();
        }
    }
}
