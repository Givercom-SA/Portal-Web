using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.ListarSolicitudes;
using ViewModel.Datos.Embarque.SolicitudDireccionamiento;

namespace Web.Principal.Areas.GestionarEmbarques.Models
{
    public class ListarSolicitudesModel
    {
        [Display(Name ="RUC/DNI Consignatario")]
        public string CampoRuc { get; set; }

        [Display(Name = "Razón Social")]
        public string CampoRazonSocial { get; set; }

        [Display(Name = "Nro. Solicitud")]
        public string CampoCodSolicitud { get; set; }

        [Display(Name = "Estado")]
        public string CodEstado { get; set; }

        public string ReturnUrl { get; set; }

        public IEnumerable<SolicitudResultVM> listaResultado { get; set; }
    }

}
