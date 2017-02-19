using System.Web.Mvc;
using log4net;

namespace Pobezhdatel.Controllers
{
    /// <summary>
    /// Controller for main single page of the application.
    /// </summary>
    public class HomeController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(HomeController));

        /// <summary>
        /// Show main single page of the application.
        /// </summary>
        /// <returns>View of the page.</returns>
        public ActionResult Index()
        {
            Log.Debug("Index");

            return View();
        }
    }
}