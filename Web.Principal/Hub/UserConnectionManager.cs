using System.Collections.Generic;

namespace Web.Principal.Interface
{
    public class UserConnectionManager : IUserConnectionManager
    {
        private static Dictionary<string, List<string>> userConnectionMap = new Dictionary<string, List<string>>();
        private static string userConnectionMapLocker = string.Empty;

        public void KeepUserConnection(string userId, string connectionId)
        {
            lock (userConnectionMapLocker)
            {
                if (!userConnectionMap.ContainsKey(userId))
                {
                    userConnectionMap[userId] = new List<string>();
                }
                userConnectionMap[userId].Add(connectionId);
            }
        }

        public void RemoveUserConnection(string connectionId)
        {
            //Remove the connectionId of user 
            lock (userConnectionMapLocker)
            {
                foreach (var userId in userConnectionMap.Keys)
                {
                    if (userConnectionMap.ContainsKey(userId))
                    {
                        if (userConnectionMap[userId].Contains(connectionId))
                        {
                            userConnectionMap[userId].Remove(connectionId);
                            break;
                        }
                    }
                }
            }
        }
        public List<string> GetUserConnections(string userId)
        {
            var conn = new List<string>();
            lock (userConnectionMapLocker)
            {
                conn = userConnectionMap[userId];
            }
            return conn;
        }
        public List<string> GetConnections()
        {
            var conn = new List<string>();

          
            lock (userConnectionMapLocker)
            {
                foreach (var item in userConnectionMap)
                {
                    if (item.Value.Count > 0) {
                        if (string.IsNullOrEmpty(item.Value[0])) {
                            conn.Add(item.Value[0]);
                        }           
                    }
                    
                }
            }

            return conn;
        }

    }
}