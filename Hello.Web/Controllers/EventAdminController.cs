using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Hello.Repo;
using Hello.Utils;
using System.Text;

namespace Hello.Web.Controllers
{
    [Authorize]
    public class EventAdminController : HelloBaseController
    {
        public ActionResult Index()
        {
            var events = _repo.Events;
            return View(events);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Create()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Event theEvent)
        {
            if (ModelState.IsValid)
            {
                _repo
                    .Events
                    .InsertOnSubmit(theEvent);
                _repo.SubmitChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View(theEvent);
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(int id)
        {
            var theEvent = _repo
                .Events
                .SingleOrDefault(e => e.EventID == id);
            if (theEvent == null)
                return RedirectToAction("Index");
            else
                return View(theEvent);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Event theEvent)
        {
            if (ModelState.IsValid)
            {
                var originalEvent = _repo
                    .Events
                    .Single(e => e.EventID == theEvent.EventID);
                
                // Copy across changes
                originalEvent.Name = theEvent.Name;
                originalEvent.Slug = theEvent.Slug;
                originalEvent.Start = theEvent.Start;
                originalEvent.End = theEvent.End;
                originalEvent.HiFiveLimit = theEvent.HiFiveLimit;

                _repo.SubmitChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View(theEvent);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id)
        {
            var theEvent = _repo
                .Events
                .SingleOrDefault(e => e.EventID == id);

            if (theEvent == null)
                return RedirectToAction("Index");

            _repo
                .Events
                .DeleteOnSubmit(theEvent);
            _repo
                .Seats
                .DeleteAllOnSubmit(theEvent.Seats);
            _repo
                .Sessions
                .DeleteAllOnSubmit(theEvent.Sessions);
            _repo
                .HiFives
                .DeleteAllOnSubmit(theEvent.HiFives);
            _repo
                .Sats
                .DeleteAllOnSubmit(theEvent.Seats.SelectMany(s => s.Sats));
            _repo.SubmitChanges();

            return RedirectToAction("Index");
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

            var rows = seating.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            var seats = new List<Seat>();
            var seatCodes = new List<string>();

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
                            seat.Code = CodeHelper.GenerateUniqueSeatCode(seatCodes);
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
    }
}
