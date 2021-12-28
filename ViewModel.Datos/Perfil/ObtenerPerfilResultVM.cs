using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModel.Datos.Perfil
{
    public class ObtenerPerfilResultVM: BaseResultVM
    {
        public ObtenerPerfilResultVM()
        {

        }
        public PerfilVM Perfil { get; set; }
    }
}
