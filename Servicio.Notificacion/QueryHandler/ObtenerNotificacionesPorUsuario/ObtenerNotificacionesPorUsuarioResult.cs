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
        public Int64 Id { get; set; }
        public string Proceso { get; set; }

        public string Mensaje { get; set; }
        public string Titulo { get; set; }
        public string FechaFormato { get { return this.Fecha.ToString("dddd dd MMMM HH:mm:ss"); } }

        public bool Leido { get; set; }
        public string Link { get; set; }
        public bool ContadorVisible { get; set; }
        public DateTime Fecha { get; set; }
    }
}
