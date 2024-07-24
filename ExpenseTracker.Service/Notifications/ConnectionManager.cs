
namespace ExpenseTracker.Service.Notifications;

public class ConnectionManager : IConnectionManager
{
    private static Dictionary<string, HashSet<string>> userMap = new Dictionary<string, HashSet<string>>();
    public IEnumerable<string> OnlineUsers { get { return userMap.Keys; } }
    public void AddConnection(string username, string connectionId)
    {
        lock (userMap)
        {
            if (!userMap.ContainsKey(username))
            {
                userMap[username] = new HashSet<string>();
            }
            userMap[username].Add(connectionId);
        }
    }

    public HashSet<string> GetConnections(string username)
    {
        var connections = new HashSet<string>();
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
                        userMap[username].Remove(connectionId);
                        break;
                    }
                }
            }
        }
    }
}