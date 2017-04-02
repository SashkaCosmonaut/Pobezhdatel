using log4net;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Pobezhdatel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Dictionary where Key is a room name and Value is a dictionary 
        /// where Key is a connection Id and Value is a player name.
        /// </summary>
        protected readonly static Dictionary<string, Dictionary<string, string>> RoomsPlayers =
            new Dictionary<string, Dictionary<string, string>>();

        /// <summary>
        /// Add record about a player in a room.
        /// </summary>
        /// <param name="roomName">Name of a room.</param>
        /// <param name="connectionId">Id of player connection.</param>
        /// <param name="playerName">Player name.</param>
        protected static void AddPlayerToRoom(string roomName, string connectionId, string playerName)
        {
            Log.Debug("AddPlayerToRoom");

            try
            {
                if (!RoomsPlayers.ContainsKey(roomName))
                {
                    RoomsPlayers.Add(roomName, new Dictionary<string, string>
                    {
                        { connectionId, playerName }
                    });
                }
                else if (!RoomsPlayers[roomName].ContainsKey(connectionId))
                {
                    RoomsPlayers[roomName].Add(connectionId, playerName);
                }
            }
            catch (Exception ex)
            {
                Log.Error("AddPlayerToRoom", ex);
            }
        }

        /// <summary>
        /// Delete record about a player in a room.
        /// </summary>
        /// <param name="roomName">Name of a room.</param>
        /// <param name="connectionId">Id of player connection.</param>
        /// <param name="playerName">Player name.</param>
        protected static void RemovePlayerFromRoom(string roomName, string connectionId, string playerName)
        {
            Log.Debug("RemovePlayerFromRoom");

            try
            {
                if (!RoomsPlayers.ContainsKey(roomName)) return;

                RoomsPlayers[roomName].Remove(connectionId);

                if (!RoomsPlayers[roomName].Any()) RoomsPlayers.Remove(roomName);
            }
            catch (Exception ex)
            {
                Log.Error("RemovePlayerFromRoom", ex);
            }
        }

        /// <summary>
        /// Get player data from cookies.
        /// </summary>
        /// <param name="playerName">Name of current player.</param>
        /// <param name="roomName">Name of room where player is.</param>
        /// <returns></returns>
        private bool GetPlayerData(out string playerName, out string roomName)
        {
            Log.Debug("GetPlayerData");

            playerName = "";
            roomName = "";

            try
            {
                var authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];

                if (authCookie?.Value == null) return false;

                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                playerName = authTicket.Name;
                roomName = authTicket.UserData;

                return true;
            }
            catch (Exception ex)
            {
                Log.Error("RemovePlayerFromRoom", ex);
                return false;
            }
        }

        /// <summary>
        /// Handle event of player connected to the game.
        /// </summary>
        /// <returns>Task for async call of this method.</returns>
        public override Task OnConnected()
        {
            Log.Debug("OnConnected");

            try
            {
                var playerName = "";
                var roomName = "";

                if (GetPlayerData(out playerName, out roomName))                // If player data is successfully taken from cookies
                {
                    Groups.Add(Context.ConnectionId, roomName).Wait();          // Add player to group and wait for it

                    AddPlayerToRoom(roomName, Context.ConnectionId, playerName);

                    // Take players of this room, convert them to array of objects for client and send to it
                    Clients.Group(roomName).playerJoinRoom(
                        JsonConvert.SerializeObject(
                            RoomsPlayers[roomName].Select(q => new { id = q.Key, name = q.Value })), playerName, roomName);
                }
            }
            catch (Exception ex)
            {
                Log.Error("OnConnected", ex);
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
            Log.Debug("OnDisconnected");

            try
            {
                var playerName = "";
                var roomName = "";

                if (GetPlayerData(out playerName, out roomName))                    // If player data is successfully taken from cookies
                {
                    RemovePlayerFromRoom(roomName, Context.ConnectionId, playerName);

                    Clients.Group(roomName).playerLeaveRoom(Context.ConnectionId);  // Notify other players in this room
                }
            }
            catch (Exception ex)
            {
                Log.Error("OnDisconnected", ex);
            }

            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        /// Handle event of player reconnect to the game.
        /// </summary>
        /// <returns>Task for async call of this method.</returns>
        public override Task OnReconnected()
        {
            Log.Debug("OnReconnected");

            return base.OnReconnected();
        }

        /// <summary>
        /// Send a message to groups of current player.
        /// </summary>
        /// <param name="message">Player's message.</param>
        public void Send(string message)
        {
            Log.Debug("Send");

            try
            {
                var dicesRoll = Utilities.DiceRoll(message);

                // If message succesfully sent to DB, show it in the chat
                if (DBModel.AddMessage(new MessageModel(DateTime.Now, Clients.Caller.roomName, Clients.Caller.playerName, message, dicesRoll)))
                {
                    // We must pass a string to this method, not dynamic type
                    Clients.Group((string)Clients.Caller.roomName).addMessageToChat(message, dicesRoll);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Send", ex);
            }
        }
    }
}