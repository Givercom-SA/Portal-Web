using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.Embarque.CobroPendienteFacturar;

namespace ViewModel.Datos.Embarque.SolicitudFacturacionTercero
{
    public class ListarFacturacionTerceroParameterVM
    {
        public string Entidad { get; set; }
        public string EmbarqueKeyBL { get; set; }
        public string EmbarqueNroBL { get; set; }
        public string Cliente { get; set; }
        public string NroDocumento { get; set; }
        public string Estado { get; set; }
        public int? IdEntidad { get; set; }

        
    }



}
