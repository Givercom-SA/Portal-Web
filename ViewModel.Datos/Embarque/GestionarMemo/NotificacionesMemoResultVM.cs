using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModel.Datos.Embarque.GestionarMemo
{
    public class NotificacionesMemoResultVM : BaseResult
    {
        public List<NotificacionesMemoVM> ListaNotificaciones { get; set; }

        public string NombreArchivo { get; set; }
        public string CODIGOMEMO
        {
            get; set;
        }
    }
    public class NotificacionesMemoVM
    {
        public string KeyBLD { get; set; }
        public string Estado { get; set; }
        public string FlagVigente { get; set; }
        public string FlagRuteado { get; set; }
        public int IdUsuarioCrea { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
