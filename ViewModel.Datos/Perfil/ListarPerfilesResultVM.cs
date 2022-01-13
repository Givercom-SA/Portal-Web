using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModel.Datos.Perfil
{
    public class ListarPerfilesResultVM: BaseResultVM
    {
        public ListarPerfilesResultVM()
        {

        }
        public IEnumerable<PerfilVM> Perfiles { get; set; }
    }

}
