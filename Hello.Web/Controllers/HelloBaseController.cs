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
    public class HelloBaseController : Controller
    {
        public HelloRepoDataContext _repo;

        public HelloBaseController() : this(new HelloRepoDataContext(Settings.ConnectionString)) { }

        public HelloBaseController(HelloRepoDataContext repo)
        {
            _repo = repo;
            PostRepoInit();
        }

        public virtual void PostRepoInit() { }
    }
}
