using Microsoft.AspNetCore.SignalR;


namespace ExpenseTracker.Service.Notifications;

public class NotificationHub : Hub
{
    private readonly IConnectionManager _connectionManager;
    public NotificationHub(IConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
    }

    public string GetConnectionId()
    {
        var username = Context.User?.Identity?.Name;
#pragma warning disable CS8604 // Possible null reference argument.
        _connectionManager.AddConnection(username, Context.ConnectionId);
#pragma warning restore CS8604 // Possible null reference argument.

        return Context.ConnectionId;
    }

#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
    public async override Task OnDisconnectedAsync(Exception exception)
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
    {
        var connectionId = Context.ConnectionId;
        _connectionManager.RemoveConnection(connectionId);
        var value = await Task.FromResult(0);
    }

    public override Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;
        var username = Context.User?.Identity?.Name;
        if (!string.IsNullOrEmpty(username))
        {
            _connectionManager.AddConnection(username, Context.ConnectionId);
        }
        return base.OnConnectedAsync();
    }
    public async Task SendNotification(string username, string message)
    {
        await Clients.All.SendAsync("ReceiveNotification", message);
    }
}