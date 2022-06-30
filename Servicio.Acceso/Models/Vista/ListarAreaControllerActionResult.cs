using AccesoDatos.Utils;
using Servicio.Acceso.Models.Perfil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Acceso.Models.Vista
{
    public class ListarAreaControllerActionResult : BaseResult
    {
        public List<AreaControllerAction> AreasControllersActions { get; set; }
        

     
    }
}
