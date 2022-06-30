using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModel.Datos.Vista
{
    public class ListarVistasResultVM : BaseResultVM
    {
        public ListarVistasResultVM()
        {

        }
        public List<VistaVM> Vistas { get; set; }
        
    }

}
