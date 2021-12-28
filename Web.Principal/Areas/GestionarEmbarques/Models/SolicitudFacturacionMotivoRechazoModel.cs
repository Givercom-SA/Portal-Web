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
    public class SolicitudFacturacionMotivoRechazoModel
    {

        public SolicitudFacturacionMotivoRechazoModel() {

          
        }

        public Int32 IdSolicitudFacturacion { get; set; }
        public string Correo { get; set; }

        [StringLength(200, MinimumLength = 4, ErrorMessage ="Se requiere mínimo 4 caracteres")]
        [Required(ErrorMessage = "Se requiere ingresar el motivo")]
        public string Mensaje { get; set; }
        public string Estado { get; set; }



    }

}
