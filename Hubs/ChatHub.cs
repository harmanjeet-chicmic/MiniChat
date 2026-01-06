using Microsoft.AspNetCore.SignalR;
using MiniChat.Server.Data;
using MiniChat.Server.Models;
using MiniChat.Server.Services;

namespace MiniChat.Server.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserConnectionService _userConnections;
        private readonly ChatDbContext _dbContext;

        public ChatHub(
            UserConnectionService userConnections,
            ChatDbContext dbContext)
        {
            _userConnections = userConnections;
            _dbContext = dbContext;
        }

        public async Task JoinChat(string username)
        {
            _userConnections.AddUser(username, Context.ConnectionId);

            await Clients.All.SendAsync(
                "OnlineUsersUpdated",
                _userConnections.GetOnlineUsers());
        }

        public async Task SendMessage(
            string sender,
            string receiver,
            string message)
        {
            var chatMessage = new Message
            {
                Sender = sender,
                Receiver = receiver,
                Content = message,
                CreatedAt = DateTime.UtcNow,
                IsDelivered = _userConnections.IsUserOnline(receiver)
            };

            _dbContext.Messages.Add(chatMessage);
            await _dbContext.SaveChangesAsync();

            if (_userConnections.TryGetConnection(receiver, out var connectionId))
            {
                await Clients.Client(connectionId)
                    .SendAsync("ReceiveMessage", sender, message);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _userConnections.RemoveUser(Context.ConnectionId);

            await Clients.All.SendAsync(
                "OnlineUsersUpdated",
                _userConnections.GetOnlineUsers());

            await base.OnDisconnectedAsync(exception);
        }
    }
}
