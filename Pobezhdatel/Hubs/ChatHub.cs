using System;
using log4net;
using Microsoft.AspNet.SignalR;
using Pobezhdatel.Models;
using System.Threading.Tasks;

namespace Pobezhdatel.Hubs
{
    /// <summary>
    /// Main game chat.
    /// </summary>
    [Authorize]
    public class ChatHub : Hub
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ChatHub));

        /// <summary>
        /// Model for communication with database.
        /// </summary>
        protected PobezhdatelDbModel DBModel = new PobezhdatelDbModel();

        /// <summary>
        /// Add a player to a room.
        /// </summary>
        /// <param name="roomName">Name of the room that player is joined.</param>
        /// <returns>SignalR waits for this Task to complete.</returns>
        public Task JoinRoom(string roomName)
        {
            return Groups.Add(Context.ConnectionId, roomName);
        }

        /// <summary>
        /// Remove a player from a room.
        /// </summary>
        /// <param name="roomName">Name of the room that player is leaved.</param>
        /// <returns>SignalR waits for this Task to complete.</returns>
        public Task LeaveRoom(string roomName)
        {
            return Groups.Remove(Context.ConnectionId, roomName);
        }

        /// <summary>
        /// Send a message to the main game chat.
        /// </summary>
        /// <param name="roomName">Name of current room.</param>
        /// <param name="playerName">Name of the player.</param>
        /// <param name="message">Player's message.</param>
        public void Send(string roomName, string playerName, string message)
        {
            Log.Debug("Send");

            var dicesRoll = Utilities.DiceRoll(message);

            // If message succesfully sent to DB, show it in the chat
            if (DBModel.AddMessage(new MessageModel(DateTime.Now, roomName, playerName, message, dicesRoll)))
            {
                Clients.Group(roomName).addMessageToChat(playerName, message, dicesRoll);
            }
        }
    }
}