using Servicio.Acceso.Models.Perfil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Acceso.Models.Menu
{
    public class ListarPerfileMenuVistaResult
    {
        public List<MenuLogin> Menus { get; set; }
        public List<VistaMenu> VistaMenus { get; set; }

     
    }
}
