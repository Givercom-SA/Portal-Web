using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Principal.Util
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

        public static class Servicio
        {

            public static readonly string Seguridad = "ConfiguracionServicios:Seguridad";
            public static readonly string Maestro = "ConfiguracionServicios:Maestro";
            public static readonly string Usuario = "ConfiguracionServicios:Usuario";
            public static readonly string Afiliado = "ConfiguracionServicios:Afiliado";
            public static readonly string Afiliacion = "ConfiguracionServicios:Afiliacion";
            public static readonly string Empleador = "ConfiguracionServicios:Empleador";
            public static readonly string Perfil = "ConfiguracionServicios:Perfil";
            public static readonly string Planilla = "ConfiguracionServicios:Planilla";
            public static readonly string RENIEC = "ConfiguracionServicios:RENIEC";
            public static readonly string LiquidacionPrevia = "ConfiguracionServicios:LiquidacionPrevia";
            public static readonly string ObligacionPago = "ConfiguracionServicios:ObligacionPago";
            public static readonly string Regularizaciones = "ConfiguracionServicios:Regularizaciones";
            public static readonly string Pagos = "ConfiguracionServicios:Pagos";
            public static readonly string Notificacion = "ConfiguracionServicios:Notificacion";
        }

        public static class RequestsConfig
        {
            public const string MultipartBodyLengthLimit = "RequestsConfig:MultipartBodyLengthLimit";
        }


    }
}
