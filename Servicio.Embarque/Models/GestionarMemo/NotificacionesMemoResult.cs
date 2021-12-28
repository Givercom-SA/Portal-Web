using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Models.GestionarMemo
{
    public class NotificacionesMemoResult : BaseResult
    {
        public List<NotificacionesMemo> ListaNotificaciones { get; set; }

        public string NombreArchivo { get; set; }
        public string CODIGOMEMO { get; set; }
    }

    public class NotificacionesMemo
    {
        public string KeyBLD { get; set; }
        public string Estado { get; set; }
        public string FlagVigente { get; set; }
        public string FlagRuteado { get; set; }
        public string NombreArchivo { get; set; }
        public int IdUsuarioCrea { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
