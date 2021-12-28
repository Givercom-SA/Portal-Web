using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Acceso.Models.Perfil
{
    public class ListarMenusPerfilResult : BaseResult
    {
        public ListarMenusPerfilResult()
        {

        }
        public List<MenuPerfil> Menus { get; set; }
    }

}
