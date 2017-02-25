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
    }
}