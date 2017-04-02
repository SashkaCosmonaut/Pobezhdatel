using System.Web.Mvc;
using Pobezhdatel.Models;

namespace Pobezhdatel.Controllers
{
    /// <summary>
    /// Base controller that is parent for all other controllers.
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// Session object of current player model with game settings.
        /// </summary>
        public PlayerLoginModel CurrentPlayer
        {
            get { return HttpContext.Session?["CurrentPlayer"] as PlayerLoginModel; }
            set
            {
                if (HttpContext.Session != null)
                    HttpContext.Session["CurrentPlayer"] = value;
            }
        }
    }
}