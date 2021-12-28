using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Datos.Embarque.CobroPendienteFacturar;
using ViewModel.Datos.Embarque.SolicitudFacturacionTercero;

namespace ViewModel.Datos.Embarque.SolicitudFacturacion
{
    public class SolicitarFacturacionEstadoParameterVM
    {
        public string Estado { get; set; }
        public string ObservacionRechazo { get; set; }
        public int IdUsuarioEvalua { get; set; }
        public int IdSolicitudFacturacion { get; set; }
        public string IdSolicitudTAFF { get; set; }
    }

  

}
