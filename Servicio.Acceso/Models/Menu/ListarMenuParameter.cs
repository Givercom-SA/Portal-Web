using Servicio.Acceso.Models.Perfil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Acceso.Models.Menu
{
    public class ListarMenuParameter
    {
        public int Estado { get; set; }
        public string Nombre { get; set; }
        public int IdMenuPadre { get; set; }
        public string TipoMenu { get; set; }



    }
}
