using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Common.Request;
using Web.Principal.Model;
using ViewModel.Datos.Embarque.CobrosPagar;

namespace Web.Principal.Areas.GestionarEmbarques.Models
{
    public class EmbarqueDetalleModel : DataRequestViewModelResponse
    {

        public string EstaAsginadoAgenteAduanas { get; set; }
        public bool SoyAgenteAduanas { get; set; }


        public int MostrarOpcionRegistroFacturacionTercero { get; set; }
        public bool MostrarMensajeOpcionRegistroFacturacionTercero { get; set; }
        public string MensajeOpcionRegistroFacturacionTercero { get; set; }

        public string TipoEntidad { get;set; }

        public bool ExisteCobrosPagarRegistrado { get; set; }

        public string TieneDesgloses { get; set; }


        public EmbarqueModel EmbarqueDetalle { get; set; }

        public ListaCobrosModel listaCobros { get; set; }

        public ListaTrackingModel listaTracking { get; set; }

        public ListarCobrosPagarResultVM listaCobrosPagar { get; set; }

        public string Servicio { get; set; }
        public string Origen { get; set; }
    }
}
