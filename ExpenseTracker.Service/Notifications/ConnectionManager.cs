
namespace ExpenseTracker.Service.Notifications;

public class ConnectionManager : IConnectionManager
{
    private static Dictionary<string, string> userMap = new Dictionary<string, string>();
    public IEnumerable<string> OnlineUsers { get { return userMap.Keys; } }
    public void AddConnection(string username, string connectionId)
    {
        lock (userMap)
        {
            if (!userMap.ContainsKey(username))
            {
                userMap[username] = "";
            }
            userMap[username] = connectionId;
        }
    }

    public string GetConnections(string username)
    {
        var connections = "";
        try
        {
            lock (userMap)
            {
                connections = userMap[username];
            }
        }
        catch
        {
            connections = null;
        }

        return connections;
    }

    public void RemoveConnection(string connectionId)
    {
        lock (userMap)
        {
            foreach (var username in userMap.Keys)
            {
                if (userMap.ContainsKey(username))
                {
                    if (userMap[username].Contains(connectionId))
                    {
                        userMap[username] = "";
                        break;
                    }
                }
            }
        }
    }
}