using Microsoft.AspNetCore.SignalR;

namespace SingalrAPI
{
    public class ApplicationHub : Hub
    {
        public static Dictionary<string, string> OnLineUsers = new Dictionary<string, string>();
        public override Task OnConnectedAsync()
        {
            Console.WriteLine("New user is connected : " + Context.ConnectionId);
            return base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string userName = OnLineUsers[Context.ConnectionId];

            await Clients.All.SendAsync("receiveMessage", "Hub : " + userName == null ? "-" : userName + " Has left the chat !");
            OnLineUsers.Remove(Context.ConnectionId);
            Console.WriteLine("User is disconnected : " + Context.ConnectionId);
            await Clients.All.SendAsync("userLeft", userName);

            await base.OnDisconnectedAsync(exception);
        }
        public async Task JoinApp(string UserName)
        {
            OnLineUsers.Add(Context.ConnectionId, UserName);

            await Clients.All.SendAsync("receiveMessage", "Hub : " + UserName + " Has joind the chat !");
            await Clients.Client(Context.ConnectionId).SendAsync("ConfirmJoin", Context.ConnectionId);
            await Clients.All.SendAsync("userJoined", OnLineUsers.Select(s=>s.Value).ToList());
        }

        public async Task sendMessage(string destination,string chatMessage)
        {
            string senderUserName = OnLineUsers[Context.ConnectionId];
            if (string.IsNullOrEmpty(destination))
            {
                await Clients.All.SendAsync("receiveMessage", senderUserName + " (To All) : "+ chatMessage);
            }
            else
            {
                var reciver = OnLineUsers.Where(s=>s.Value == destination).FirstOrDefault();
                await Clients.Client(reciver.Key).SendAsync("receiveMessage", senderUserName + " : " + chatMessage);
            }

        }
    }
}
