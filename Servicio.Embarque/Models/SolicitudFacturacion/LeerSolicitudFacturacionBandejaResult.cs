using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Models.SolicitudFacturacion
{
    public class LeerSolicitudFacturacionBandejaResult : BaseResult
    {
        public SolicitudFacturacion SolicitudFacturacion { get; set; }
    }


}
