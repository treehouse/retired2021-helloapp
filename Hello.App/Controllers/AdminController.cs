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

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Events()
        {
            var events = _repo.Events;
            return View(events);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Events(Event theEvent)
        {
            _repo
                .Events
                .InsertOnSubmit(theEvent);
            _repo.SubmitChanges();

            return RedirectToAction("Events");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteEvent(int id)
        {
            var theEvent = _repo
                .Events
                .SingleOrDefault(c => c.EventID == id);

            if (theEvent == null)
                return RedirectToAction("Events");

            _repo
                .Events
                .DeleteOnSubmit(theEvent);
            _repo.SubmitChanges();

            return RedirectToAction("Events");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Sessions(int id)
        {
            var theEvent = _repo
                .Events
                .SingleOrDefault(e => e.EventID == id);

            if (theEvent == null)
                return RedirectToAction("Events");

            ViewData["Event"] = theEvent;

            var sessions = _repo
                .Sessions
                .Where(s => s.EventID == theEvent.EventID);
            return View(sessions);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Sessions(int id, Session session)
        {
            var theEvent = _repo
                .Events
                .SingleOrDefault(e => e.EventID == id);

            if (theEvent == null)
                return RedirectToAction("Events");

            _repo
                .Sessions
                .InsertOnSubmit(session);

            _repo.SubmitChanges();

            return RedirectToAction("Sessions", new { id = theEvent.EventID });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteSession(int id)
        {
            var session = _repo
                .Sessions
                .SingleOrDefault(s => s.SessionID == id);

            if (session == null)
                return RedirectToAction("Events");

            _repo
                .Sessions
                .DeleteOnSubmit(session);
            _repo.SubmitChanges();

            return RedirectToAction("Sessions", new { id = session.EventID });
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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteCampaign(int id)
        {
            var campaign = _repo
                .Campaigns
                .SingleOrDefault(c => c.CampaignID == id);

            if (campaign == null)
                return RedirectToAction("Campaigns");

            _repo
                .Campaigns
                .DeleteOnSubmit(campaign);
            _repo.SubmitChanges();

            return RedirectToAction("Campaigns");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Tokens(int id)
        {
            var campaign = _repo
                .Campaigns
                .SingleOrDefault(c => c.CampaignID == id);

            if (campaign == null)
                return RedirectToAction("Campaigns");

            ViewData["Campaign"] = campaign;

            // Token Code
            ViewData["Code"] = GenerateTokenCode(new Random());

            var tokens = _repo
                .Tokens
                .Where(t => t.CampaignID == id);

            return View(tokens);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Tokens(int id, Token token)
        {
            var campaign = _repo
                .Campaigns
                .SingleOrDefault(c => c.CampaignID == id);

            if (campaign == null)
                return RedirectToAction("Campaigns");

            _repo
                .Tokens
                .InsertOnSubmit(token);
            _repo.SubmitChanges();

            return RedirectToAction("Tokens");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteToken(int id)
        {
            var token = _repo
                .Tokens
                .SingleOrDefault(t => t.TokenID == id);

            if (token == null)
                return RedirectToAction("Campaigns");

            _repo
                .Tokens
                .DeleteOnSubmit(token);
            _repo.SubmitChanges();

            return RedirectToAction("Tokens", new { id = token.CampaignID });
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

        public ActionResult SeatCodes(int id)
        {
            var theEvent = _repo
                .Events
                .SingleOrDefault(e => e.EventID == id);

            if (theEvent == null)
                return RedirectToAction("Events");
            
            return View(theEvent);
        }

        public ActionResult Status()
        {
            var processedTweets = _repo
                .QueuedTweets
                .Count(t => t.Processed);
            ViewData["ProcessedTweets"] = processedTweets;

            var unprocessedTweets = _repo
                .QueuedTweets
                .Count(t => !t.Processed);
            ViewData["UnprocessedTweets"] = unprocessedTweets;

            var tideMarks = _repo
                .TideMarks
                .OrderByDescending(t => t.TimeStamp)
                .Take(Settings.Admin.MaxTideMarks);

            return View(tideMarks);
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
            return GenerateCode(r, 5);
        }

        private static string GenerateTokenCode(Random r)
        {
            return GenerateCode(r, 10);
        }

        private static string GenerateCode(Random r, int length)
        {
            var code = new StringBuilder(length);

            for (int i = 0; i < length; i++)
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
