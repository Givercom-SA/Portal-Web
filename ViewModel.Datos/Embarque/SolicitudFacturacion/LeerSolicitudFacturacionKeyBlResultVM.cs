using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.Embarque.SolicitudFacturacion
{
    public class LeerSolicitudFacturacionKeyBlResultVM : BaseResultVM
    {
        public IEnumerable< SolicitudFacturacionVM> SolicitudFacturaciones { get; set; }
        
    }

  
}
