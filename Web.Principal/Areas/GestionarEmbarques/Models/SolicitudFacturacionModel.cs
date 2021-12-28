using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.ListarSolicitudes;
using ViewModel.Datos.Embarque.SolicitudDireccionamiento;
using ViewModel.Datos.Embarque.SolicitudFacturacionTercero;
using ViewModel.Datos.Embarque.CobroPendienteFacturar;

namespace Web.Principal.Areas.GestionarEmbarques.Models
{
    public class SolicitudFacturacionModel
    {

        public SolicitudFacturacionModel() {

            this.SolicitarFacturacionTercero = new SolicitarFacturacionTerceroParameterVM();
            this.CobrosPendientesEmbarque = new List<CobrosPendienteEmbarqueVM>();

        }

        public string KEYBL { get; set; }
        public string BL { get; set; }

        [Display(Name = "BLs")]
        public string BlNietos { get; set; }

        public SolicitarFacturacionTerceroParameterVM SolicitarFacturacionTercero { get; set; }

        public List<CobrosPendienteEmbarqueVM> CobrosPendientesEmbarque { get; set; }

    }

}
