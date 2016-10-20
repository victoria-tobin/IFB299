using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnTheSpot.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "15 years in the making...";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Let us help you.";

            return View();
        }
    }
}
