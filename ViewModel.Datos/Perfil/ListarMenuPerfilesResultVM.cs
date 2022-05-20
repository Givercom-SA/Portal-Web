using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModel.Datos.Perfil
{
    public class ListarMenusPerfilResultVM : BaseResultVM
    {
        public ListarMenusPerfilResultVM()
        {

        }
        public List<MenuPerfilVM> Menus { get; set; }
        public List<VistaMenuVM> VistaMenu { get; set; }
    }

}
