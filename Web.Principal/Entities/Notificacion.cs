using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Principal.Entities
{
    public class Notificacion
    {
        public int CodigoUsuario { get; set; }
        public string Proceso{ get; set; }
        public string Mensaje { get; set; }
        public DateTime CreacionFecha { get; set; }
    }
}
