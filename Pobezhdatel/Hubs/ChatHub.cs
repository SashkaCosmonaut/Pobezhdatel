using log4net;
using Microsoft.AspNet.SignalR;

namespace Pobezhdatel.Hubs
{
    /// <summary>
    /// Main game chat.
    /// </summary>
    public class ChatHub : Hub
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ChatHub));

        /// <summary>
        /// Send a message to the main game chat.
        /// </summary>
        /// <param name="playerName">Name of the player.</param>
        /// <param name="message">Player's message.</param>
        public void Send(string playerName, string message)
        {
            Log.Debug("Send");

            Clients.All.addMessageToChat(playerName, message);
        }
    }
}