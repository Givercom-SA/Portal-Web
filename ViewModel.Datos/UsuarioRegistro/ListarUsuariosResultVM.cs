using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.UsuarioRegistro
{
    public class ListarUsuariosResultVM : BaseResultVM
    {


        public int TotalRegistros { get; set; }
        public List<UsuarioVM> Usuarios { get; set; }

    }





}
