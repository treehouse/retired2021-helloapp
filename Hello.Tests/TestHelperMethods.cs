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
    }
}
