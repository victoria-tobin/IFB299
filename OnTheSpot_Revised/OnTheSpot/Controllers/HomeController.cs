using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnTheSpot.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Basic template of a controller for reference.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        /// <summary>
        /// About page controller, returns the About page cshtml.
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            ViewBag.Message = "15 years in the making...";

            return View();
        }

        /// <summary>
        /// Contact page controller, returns the contact page cshtml.
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact()
        {
            ViewBag.Message = "Let us help you.";

            return View();
        }

        /// <summary>
        /// Howto page controller, returns the Howto page cshtml.
        /// </summary>
        /// <returns></returns>
        public ActionResult Howto()
        {
            ViewBag.Message = "Ordering is simple with us! Just follow these steps!";

            return View();
        }
    }
}
