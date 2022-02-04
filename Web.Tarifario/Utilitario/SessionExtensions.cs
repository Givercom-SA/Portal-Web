using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.UsuarioRegistro;

namespace Web.Tarifario.Utilitario
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

        public static UsuarioRegistroVM GetUserContent(this ISession session)
        {
            var defaultUser = session.GetString(USER_SESSION);

            if (defaultUser == null)
                throw new System.Exception("La sesión actual ha caducado.");

            return JsonConvert.DeserializeObject<UsuarioRegistroVM>(session.GetString(USER_SESSION));
            //return defaultUser == null ? null : JsonConvert.DeserializeObject<UsuarioSesion>(session.GetString(USER_SESSION));
        }
    }
}
