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
        var httpContext = Context.GetHttpContext();
        var username = httpContext.Request.Query["username"];
        _connectionManager.AddConnection(username, Context.ConnectionId);

        return Context.ConnectionId;
    }

    public async override Task OnDisconnectedAsync(Exception exception)
    {
        var connectionId = Context.ConnectionId;
        _connectionManager.RemoveConnection(connectionId);
        var value = await Task.FromResult(0);
    }

    public async Task SendNotification(string username, string message)
    {
        await Clients.User(username).SendAsync("ReceiveNotification", message);
    }
}