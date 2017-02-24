using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pobezhdatel.Controllers
{
    /// <summary>
    /// Controller for main home page of the application.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Show home page.
        /// </summary>
        /// <returns>View of home page.</returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}