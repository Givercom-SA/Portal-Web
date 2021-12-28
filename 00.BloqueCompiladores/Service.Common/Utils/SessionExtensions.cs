using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;


namespace Service.Common.Utils
{
    public static class SessionExtensions
    {
        private static string USER_SESSION = "UserDefault";

        public static void SetSession(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static void SetUserContent(this ISession session, object value)
        {
            session.SetString(USER_SESSION, JsonConvert.SerializeObject(value));
        }

        public static T GetSession<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);            
        }
        
        public static T GetUserContent<T>(this ISession session)
        {
            var defaultUser = session.GetString(USER_SESSION);

            if (defaultUser == null)
                throw new Exception("La sesión actual ha caducado.");

            return JsonConvert.DeserializeObject<T>(session.GetString(USER_SESSION));
            //return defaultUser == null ? null : JsonConvert.DeserializeObject<UsuarioSesion>(session.GetString(USER_SESSION));
        }
    }
}
