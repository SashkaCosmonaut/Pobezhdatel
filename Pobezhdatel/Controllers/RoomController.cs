using System;
using System.Web.Mvc;
using log4net;
using Newtonsoft.Json;
using Pobezhdatel.Models;

namespace Pobezhdatel.Controllers
{
    /// <summary>
    /// Controller for chat room.
    /// </summary>
    [Authorize]
    public class RoomController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RoomController));

        /// <summary>
        /// Model for communication with database.
        /// </summary>
        protected PobezhdatelDbModel DBModel = new PobezhdatelDbModel();

        /// <summary>
        /// Show main single page of the application.
        /// </summary>
        /// <returns>View of the page.</returns>
        public ActionResult Index()
        {
            Log.Debug("Index");

            ViewBag.PlayerName = "Test";
            ViewBag.RoomName = "Test";

            return View();
        }

        /// <summary>
        /// Get all previous messages that already exist in DB.
        /// </summary>
        /// <returns>JSON-string with messages models array.</returns>
        public string GetMessages()
        {
            Log.Debug("GetMessages");

            return JsonConvert.SerializeObject(DBModel.GetMessages(""));
        }
    }
}