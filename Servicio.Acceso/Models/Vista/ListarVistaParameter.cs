using Servicio.Acceso.Models.Perfil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Acceso.Models.Vista
{
    public class ListarVistaParameter
    {
        public int Estado { get; set; }
        public string Nombre { get; set; }
        public int IdVistaPadre { get; set; }

        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        



    }
}
