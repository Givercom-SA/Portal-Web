using QueryHandlers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Notificacion.QueryHandler.EnvioCorreoError
{
    public class EnvioCorreoErrorResult : QueryResult
    {
        public bool ResultadoCorreo { get; set; }
    }
}
