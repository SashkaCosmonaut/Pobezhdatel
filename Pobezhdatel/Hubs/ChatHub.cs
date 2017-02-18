using Microsoft.AspNet.SignalR;

namespace Pobezhdatel.Hubs
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            Clients.All.addMessageToPage(name, message);
        }
    }
}