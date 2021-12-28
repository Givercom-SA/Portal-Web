using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Models.SolicitudFacturacionTerceros
{
    public class ListarFacturacionTerceroParameter
    {
        public string EMFT_ENTIDAD { get; set; }
        public string EMFT_EMBARQUE_KEYBL { get; set; }
        public string EMFT_EMBARQUE_NROBL { get; set; }
        public string EMFT_CLIENTE { get; set; }
        public string EMFT_CLIENTE_NRODOC { get; set; }
        public string EMFT_ESTADO { get; set; }
        public int? IdEntidad { get; set; }
        


    }



}
