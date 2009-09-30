using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hello.Repo;

namespace Hello.Tests
{
    public static class TestHelperMethods
    {
        public static void SetupTestEvent(HelloRepoDataContext repo)
        {
            repo.Events.InsertOnSubmit(new Event
            {
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1),
                HiFiveLimit = 5,
                Slug = "testEvent",
                Name = "testEvent"
            });
            repo.SubmitChanges();
        }

        public static void SetupTestEvent(HelloRepoDataContext repo, out int eventId)
        {
            repo.Events.InsertOnSubmit(new Event
            {
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1),
                HiFiveLimit = 5,
                Slug = "testEvent",
                Name = "testEvent"
            });
            repo.SubmitChanges();

            eventId = repo.Events.Where(e => e.Name == "testEvent").Select(e => e.EventID).FirstOrDefault();
        }

        public static User CreateTestUser(HelloRepoDataContext repo, string username)
        {
            var user = new User
            {
                Username = username,
                Created = DateTime.Now,
                Updated = DateTime.Now,
                ImageURL = "URL"
            };
            repo.Users.InsertOnSubmit(user);
            repo.SubmitChanges();
            return user;
        }

        public static void SetupTestSession(HelloRepoDataContext repo, int eventId)
        {
            repo.Sessions.InsertOnSubmit(new Session
                                             {
                                                 EventID = eventId,
                                                 Name = "testSession",
                                                 Start = DateTime.Now,
                                                 Finish = DateTime.Now.AddHours(1)
                                             });
            repo.SubmitChanges();
        }

        public static void SetupTestSeat(HelloRepoDataContext repo, int eventId, int row, int column, string code)
        {
            repo.Seats.InsertOnSubmit(new Seat
                                          {
                                              Row = row,
                                              Column = column,
                                              EventID = eventId,
                                              Code = code
                                          });
            repo.SubmitChanges();
        }
    }
}
