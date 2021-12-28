using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilitario.Constante
{


    public static class ConfiguracionConstante
    {
    
        public static class Correo
        {
            public const string Email = "Email:User";
            public const string SMTPServer = "Email:SMTPServer";
            public const string Port = "Email:Port";


            public const string SSL = "Email:SSL";
            public const string User = "Email:User";

            public const string Password = "Email:Password";
            public const string SenderUser = "Email:SenderUser";
            public const string CopiaOculta = "Email:BCC";

        }

        public static class Imagen
        {
            public const string ImagenGrupo = "Imagen:ImagenGrupo";
            public const string ImagenGrupoUrl = "Imagen:ImagenGrupoUrl";


        }

        public static class Cola
        {
            public const string Correo = "Cola:Correo";
           


        }
    }

}
