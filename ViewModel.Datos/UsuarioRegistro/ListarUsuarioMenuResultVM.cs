using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.UsuarioRegistro
{
    public class ListarUsuarioMenuResultVM : BaseResultVM
    {
        public ListarUsuarioMenuResultVM()
        {

        }
        public List<UsuarioMenuVM> Menus { get; set; }

    }

}
