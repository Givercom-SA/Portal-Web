using QueryHandlers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Notificacion.QueryHandler.ObtenerNotificacionesPorUsuario
{
    public class ObtenerNotificacionesPorUsuarioParameter : QueryParameter
    {
        public int CodigoUsuario { get; set; }
    }
}
