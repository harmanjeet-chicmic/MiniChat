using System.Collections.Concurrent;

namespace MiniChat.Server.Services
{
    public class UserConnectionService
    {
        private readonly ConcurrentDictionary<string, string> _users
            = new();

        public void AddUser(string username, string connectionId)
        {
            _users[username] = connectionId;
        }

        public void RemoveUser(string connectionId)
        {
            var user = _users.FirstOrDefault(
                x => x.Value == connectionId);

            if (!string.IsNullOrEmpty(user.Key))
            {
                _users.TryRemove(user.Key, out _);
            }
        }

        public bool TryGetConnection(
            string username,
            out string connectionId)
        {
            return _users.TryGetValue(username, out connectionId!);
        }

        public bool IsUserOnline(string username)
        {
            return _users.ContainsKey(username);
        }

        public List<string> GetOnlineUsers()
        {
            return _users.Keys.ToList();
        }
    }
}
