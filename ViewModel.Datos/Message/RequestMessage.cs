using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.Message
{
  public  class RequestMessage
    {
        public string Contenido { get; set; }
        public string Asunto { get; set; }
        public string Correo { get; set; }
        public string[] Archivos { get; set; }
        public string[] Cuentas { get; set; }
        public string[] CopiaCuentas { get; set; }
        public int TipoCorreo { get; set; }



    }
}
