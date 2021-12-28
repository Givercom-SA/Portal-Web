using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Models.GestionarMemo
{
    public class NotificacionMemoParameter
    {
        public string KeyBLD { get; set; }
        public int IdUsuario { get; set; }
        public string FlagVigente { get; set; }
        public string FlagRuteado { get; set; }
        public string NombreArchivo { get; set; }
    }
}
