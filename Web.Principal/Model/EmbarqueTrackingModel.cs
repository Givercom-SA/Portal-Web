using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Common.Request;

namespace Web.Principal.Model
{
    public class EmbarqueTrackingModel
    {
        public string ETAPOD { get; set; }
        public string ETDPOL { get; set; }
        public string APROBACION_FLETE { get; set; }
        public string DIRECCIONAMIENTOS { get; set; }
        public string DESGLOSES { get; set; }
        public string ACEPTACION_DIRECCIONAMIENTO { get; set; }
        public DateTime? FECENDOSE { get; set; }
        public DateTime? FECVENSOBRESTADIA { get; set; }
        public DateTime? FECRECMEMO { get; set; }
        public DateTime? FECINGRESO { get; set; }
        public int DIASLIBRESALMACENAJE { get; set; }
        public DateTime? FECSALIDA { get; set; }
        public string NROINVENTARIO { get; set; }

    }

    public class ListaTrackingModel: DataRequestViewModelResponse
    {
        public string NroEmbarque { get; set; }
        
        public EmbarqueTrackingModel Tracking { get; set; }
    }
}
