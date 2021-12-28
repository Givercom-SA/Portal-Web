using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Models.SolicitudFacturacion
{
    public class SolicitarFacturacionEstadoParameter
    {
        public string Estado { get; set; }
        public string ObservacionRechazo { get; set; }
        public int IdUsuarioEvalua { get; set; }
        public int IdSolicitudFacturacion  { get; set; }
        public string IdSolicitudTAFF { get; set; }

        

    }

 


}
