using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpenLawOffice.WebClient.Controllers
{
    [HandleError]
    public class HomeController : BaseController
    {
        [SecurityFilter]
        public ActionResult Index()
        {
            ViewData["Message"] = "Welcome to ASP.NET MVC!";

            return View();
        }

        [SecurityFilter]
        public ActionResult About()
        {
            return View();
        }
    }
}
