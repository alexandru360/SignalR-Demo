using Microsoft.AspNetCore.SignalR;

namespace SignalRDemo;

public class MessageHub : Hub
{
    private readonly IDictionary<string, UserConnection> _connections;

    public MessageHub()
    {
        _connections = new Dictionary<string, UserConnection>();
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public async Task JoinRoom(UserConnection userConnection)
    {
        _connections.Add(Context.ConnectionId, userConnection);
        await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.UserId);

        await Clients.Caller.SendAsync("ReceiveMessage", new MessageDto
        {
            Id = Guid.NewGuid().ToString(),
            MessageText = $"You have joined room {userConnection.UserId}",
            MessageType = "SentFromServer",
            MessageReadStatus = false,
            createdAt = DateTime.Now
        });
    }

    public async Task SendMessage(string message)
    {
        if (_connections.TryGetValue(Context.ConnectionId, out var userConnection))
            await Clients.Group(userConnection.UserId).SendAsync(
                "ReceiveMessage",
                new MessageDto
                {
                    Id = null,
                    MessageText = message,
                    MessageType = "SentFromServer",
                    createdAt = DateTime.Now
                });
    }
}

public class UserConnection
{
    public string User { get; set; }
    public string UserId { get; set; }
}

public class MessageDto
{
    public string? Id { get; set; }
    public string MessageType { get; set; }
    public string MessageText { get; set; }
    public Boolean MessageReadStatus { get; set; }
    public DateTime createdAt { get; set; }
}