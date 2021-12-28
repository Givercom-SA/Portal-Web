using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Models
{
    public class VerificarAsignacionAgenteAduanasResult : BaseResult
    {
    
     public string RazonSocial { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }

        public int EstadoAsignacion { get; set; }
        

    }
 

}
