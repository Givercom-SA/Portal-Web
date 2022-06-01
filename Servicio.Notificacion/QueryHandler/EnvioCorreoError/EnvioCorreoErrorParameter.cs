using QueryHandlers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Notificacion.QueryHandler.EnvioCorreoError
{
    public class EnvioCorreoErrorParameter : QueryParameter
    {


        public string Correo { get; set; }

        public string Mensaje { get; set; }
    }
}
