using QueryHandlers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Notificacion.QueryHandler.ObtenerNotificacionesPorUsuario
{
    public class ObtenerNotificacionesPorUsuarioResult : QueryResult
    {
        public List<NotificacionResult> Elementos { get; set; }
    }

    public class NotificacionResult
    {
        public string Proceso { get; set; }

        public string Mensaje { get; set; }

        public DateTime Fecha { get; set; }
    }
}
