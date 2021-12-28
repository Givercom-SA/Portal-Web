using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Principal.Model
{
    public class RegistroSolicitudFacturacionDetalleRequestModel
    {

        public string pIdSolicitud { get; set; }


        public string pRubroCCodigo { get; set; }
        public string pConcepCCodigo { get; set; }

        public string pMoneda { get; set; }
        public double pImporte { get; set; }


    }
}
