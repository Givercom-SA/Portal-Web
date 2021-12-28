using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.ListarSolicitudes;
using ViewModel.Datos.Embarque.SolicitudDireccionamiento;
using ViewModel.Datos.Embarque.SolicitudFacturacionTercero;
using ViewModel.Datos.Embarque.CobroPendienteFacturar;
using ViewModel.Datos.Embarque.SolicitudFacturacion;

namespace Web.Principal.Areas.GestionarEmbarques.Models
{
    public class ListarSolicitudFacturacionBandejaModel
    {

        public ListarSolicitudFacturacionBandejaModel()
        {

            this.SolicitudFacturacionBandeja = new ListarSolicitudFacturacionBandejaParameterVM();
            this.SolicitudFacturacionBandejaResult = new ListarSolicitudFacturacionBandejaResultVM();
            this.Motivorechazo = new SolicitudFacturacionMotivoRechazoModel();

        }



        public SolicitudFacturacionMotivoRechazoModel Motivorechazo { get; set; }
        public string TipoPerfil { get; set; }

        [Display(Name = "Nro. de Embarcación")]
        public string NroBl { get; set; }


        [Display(Name = "RUC/DNI Consignatario")]
        public string NroDocumentoConsignatario { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; }
        [Display(Name = "Nro. de Solicitud")]
        public string CodigoFacturacion { get; set; }
        [Display(Name = "Nombre Consignatario")]
        public string SolicitanteNombre { get; set; }
        [Display(Name = "Fecha de Registro")]
        public string FechaRegistro { get; set; }


        public ListarSolicitudFacturacionBandejaParameterVM SolicitudFacturacionBandeja { get; set; }

        public ListarSolicitudFacturacionBandejaResultVM SolicitudFacturacionBandejaResult { get; set; }

    }

}
