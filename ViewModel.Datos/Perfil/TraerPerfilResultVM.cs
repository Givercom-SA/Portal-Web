using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.Perfil
{
    public class TraerPerfilResultVM: BaseResultVM
    {
        public TraerPerfilResultVM()
        {

        }
        public PerfilVM perfil { get; set; }

    }
}
