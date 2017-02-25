using System;
using System.Web.Mvc;
using log4net;
using Pobezhdatel.Models;

namespace Pobezhdatel.Controllers
{
    /// <summary>
    /// Controller for main home page of the application.
    /// </summary>
    public class HomeController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(HomeController));

        /// <summary>
        /// Show home page.
        /// </summary>
        /// <returns>View of home page.</returns>
        public ActionResult Index()
        {
            Log.Debug("Index");

            return View();
        }

        /// <summary>
        /// Get results of login form, check it and redirect player.
        /// </summary>
        /// <param name="model">Data from login form.</param>
        /// <returns>Room view or current view with errors.</returns>
        [HttpPost]
        public ActionResult LogIn(LoginModel model)
        {
            Log.Debug("LogIn");

            try
            {
                if (!ModelState.IsValid) return View(model);

                return RedirectToAction("Index", "Room");
            }
            catch (Exception ex)
            {
                Log.Error("LogIn", ex);
                return View(model);
            }
        }
    }
}