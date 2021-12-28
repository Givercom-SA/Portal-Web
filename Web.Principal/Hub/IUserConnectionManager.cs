using System.Collections.Generic;

namespace Web.Principal.Interface
{
    public interface IUserConnectionManager
    {
        void KeepUserConnection(string userId, string connectionId);
        void RemoveUserConnection(string connectionId);
        List<string> GetUserConnections(string userId);
        List<string> GetConnections();
    }
}