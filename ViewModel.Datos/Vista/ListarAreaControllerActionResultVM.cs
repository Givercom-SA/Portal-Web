using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModel.Datos.Vista
{
    public class ListarAreaControllerActionResultVM : BaseResultVM
    {
        public ListarAreaControllerActionResultVM()
        {

        }
        public List<AreaControllerActionVM> AreasControllersActions { get; set; }
        
    }

}
