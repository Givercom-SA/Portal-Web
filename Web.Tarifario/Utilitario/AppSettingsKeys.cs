using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Tarifario.Utilitario
{
    public static class AppSettingsKeys
    {
        public static class ConnectionStrings
        {
            public const string AFPnet = "ConnectionStrings:AFPnet";
        }

        public static class AppSettingKeys
        {

            public static class Email
            {
                public static readonly string SMTPServer = "Email:SMTPServer";

                public static readonly string Port = "Email:Port";

                public static readonly string SSL = "Email:SSL";

                public static readonly string User = "Email:User";

                public static readonly string Password = "Email:Password";

                public static readonly string SenderMail = "Email:SenderUser";

            }
        }

        public static class Seguridad
        {
            public static readonly string LlaveEncriptamiento = "Security:EncryptKey";

            public static readonly string CodigoDisponibilidadSistema = "Security:SystemAvailabilityCode";
            public static readonly string SessionTimeExpired = "Session:TimeExpired";
        }

      
        public static class RequestsConfig
        {
            public const string MultipartBodyLengthLimit = "RequestsConfig:MultipartBodyLengthLimit";
        }


    }
}
