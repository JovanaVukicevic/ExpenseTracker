using Microsoft.Identity.Client;

namespace ExpenseTracker.Service.Notifications;

public interface IConnectionManager
{
    void AddConnection(string username, string connectionId);
    void RemoveConnection(string connectionId);
    string GetConnections(string username);
    IEnumerable<string> OnlineUsers { get; }
}