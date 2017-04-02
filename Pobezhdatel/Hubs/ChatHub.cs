using log4net;
using Microsoft.AspNet.SignalR;
using Pobezhdatel.Models;
using System;
using System.Threading.Tasks;
using System.Web.Security;

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
        /// Get player data from cookies.
        /// </summary>
        /// <param name="playerName">Name of current player.</param>
        /// <param name="roomName">Name of room where player is.</param>
        /// <returns></returns>
        private bool GetPlayerData(out string playerName, out string roomName)
        {
            playerName = "";
            roomName = "";

            var authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie?.Value == null) return false;

            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            playerName = authTicket.Name;
            roomName = authTicket.UserData;

            return true;
        }

        /// <summary>
        /// Handle event of player connected to the game.
        /// </summary>
        /// <returns>Task for async call of this method.</returns>
        public override Task OnConnected()
        {
            var playerName = "";
            var roomName = "";

            if (GetPlayerData(out playerName, out roomName))                    // If player data is successfully taken from cookies
            {
                Groups.Add(Context.ConnectionId, roomName).Wait();              // Add player to group and wait for it

                Clients.Group(roomName).playerJoinRoom(playerName, roomName);   // Notify other players in this room
            }

            return base.OnConnected();
        }

        /// <summary>
        /// Handle event of player leave the game.
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns>Task for async call of this method.</returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            var playerName = "";
            var roomName = "";

            if (GetPlayerData(out playerName, out roomName))                    // If player data is successfully taken from cookies
            {
                Groups.Remove(Context.ConnectionId, roomName).Wait();           // Remove player to group and wait for it

                Clients.Group(roomName).playerLeaveRoom(playerName, roomName);  // Notify other players in this room
            }

            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        /// Handle event of player reconnect to the game.
        /// </summary>
        /// <returns>Task for async call of this method.</returns>
        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }

        /// <summary>
        /// Send a message to groups of current player.
        /// </summary>
        /// <param name="message">Player's message.</param>
        public void Send(string message)
        {
            Log.Debug("Send");

            var dicesRoll = Utilities.DiceRoll(message);

            // If message succesfully sent to DB, show it in the chat
            if (DBModel.AddMessage(new MessageModel(DateTime.Now, Clients.Caller.roomName, Clients.Caller.playerName, message, dicesRoll)))
            {
                // We must pass a string to this method, not dynamic type
                Clients.Group((string)Clients.Caller.roomName).addMessageToChat(message, dicesRoll);
            }
        }
    }
}