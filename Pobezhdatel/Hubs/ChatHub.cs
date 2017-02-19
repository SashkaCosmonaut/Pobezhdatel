using System;
using log4net;
using Microsoft.AspNet.SignalR;
using Pobezhdatel.Models;

namespace Pobezhdatel.Hubs
{
    /// <summary>
    /// Main game chat.
    /// </summary>
    public class ChatHub : Hub
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ChatHub));

        /// <summary>
        /// Model for communication with database.
        /// </summary>
        protected PobezhdatelDbModel DBModel = new PobezhdatelDbModel();

        /// <summary>
        /// Send a message to the main game chat.
        /// </summary>
        /// <param name="playerName">Name of the player.</param>
        /// <param name="message">Player's message.</param>
        public void Send(string playerName, string message)
        {
            Log.Debug("Send");

            Clients.All.addMessageToChat(playerName, message);

            DBModel.AddMessage(new MessageModel(DateTime.Now, playerName, message));
        }
    }
}