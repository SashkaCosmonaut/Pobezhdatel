using Microsoft.AspNet.SignalR;

namespace Pobezhdatel.Hubs
{
    /// <summary>
    /// Main game chat.
    /// </summary>
    public class ChatHub : Hub
    {
        /// <summary>
        /// Send a message to the main game chat.
        /// </summary>
        /// <param name="playerName">Name of the player.</param>
        /// <param name="message">Player's message.</param>
        public void Send(string playerName, string message)
        {
            Clients.All.addMessageToPage(playerName, message);
        }
    }
}