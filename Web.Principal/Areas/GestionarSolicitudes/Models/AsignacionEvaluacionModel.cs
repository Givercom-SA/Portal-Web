using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.Embarque.AsignarAgente;

namespace Web.Principal.Areas.GestionarSolicitudes.Models
{
    public class AsignacionEvaluacionModel
    {

        public ViewModel.Datos.Solicitud.SolicitudVM SolicitudVM { get; set; }

        [Display(Name = "Motivo Rechazo (*)")]
        [Required(ErrorMessage = "Debe ingresar un motivo de rechazo")]
        public string MotivoRechazo { get; set; }

        [Display(Name = "Código de Solicitud (*)")]
        [Required(ErrorMessage = "Debe ingresar un código de solicitud")]
        public string CodigoSolicitud { get; set; }


    }
}
