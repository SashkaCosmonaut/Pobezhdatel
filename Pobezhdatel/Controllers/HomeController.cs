using System.Web.Mvc;

namespace Pobezhdatel.Controllers
{
    /// <summary>
    /// Controller for main single page of the application.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Show main single page of the application.
        /// </summary>
        /// <returns>View of the page.</returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}