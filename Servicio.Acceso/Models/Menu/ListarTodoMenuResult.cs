using AccesoDatos.Utils;
using Servicio.Acceso.Models.Perfil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Acceso.Models.Menu
{
    public class ListarTodoMenuResult : BaseResult
    {
        public List<Menu> Menus { get; set; }
        
    }
}
