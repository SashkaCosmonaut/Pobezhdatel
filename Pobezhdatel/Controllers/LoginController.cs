using System;
using System.Web.Mvc;
using log4net;
using Pobezhdatel.Models;
using System.Web.Security;
using System.Web;

namespace Pobezhdatel.Controllers
{
    /// <summary>
    /// Controller for login to the application.
    /// </summary>
    public class LoginController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LoginController));

        /// <summary>
        /// Show the login page.
        /// </summary>
        /// <returns>View of the login page.</returns>
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
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(PlayerLoginModel model)
        {
            Log.Debug("LogIn");

            try
            {
                if (!ModelState.IsValid) return View("Index", model);

                // Create and save cookies
                var authTicket = new FormsAuthenticationTicket(
                    1,
                    model.PlayerName,                   // Set user name linked with the cookie
                    DateTime.Now,
                    DateTime.Now.AddMinutes(240),
                    true,
                    model.RoomName);                    // Save room name as additional cookie data

                HttpContext.Response.Cookies.Add(
                    new HttpCookie(
                        FormsAuthentication.FormsCookieName,
                        FormsAuthentication.Encrypt(authTicket)));

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