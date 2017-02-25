using System.Web.Mvc;
using log4net;
using Pobezhdatel.Models;

namespace Pobezhdatel.Controllers
{
    /// <summary>
    /// Base controller that is parent for all other controllers.
    /// </summary>
    public class BaseController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BaseController));

        /// <summary>
        /// Session object of current game model with game settings.
        /// </summary>
        public GameModel CurrentGameModel
        {
            get { return HttpContext.Session?["CurrentGameModel"] as GameModel; }
            set
            {
                if (HttpContext.Session != null)
                    HttpContext.Session["CurrentGameModel"] = value;
            }
        }

        /// <summary>
        /// Check the state of current game model, if it isn't set 
        /// (If player hasn't logged in) then redirect him to login page.
        /// </summary>
        public void CheckCurrentGameModel()
        {
            Log.Debug("CheckCurrentGameModel");

            if (CurrentGameModel == null)
                RedirectToAction("Index", "Home");
        }
    }
}