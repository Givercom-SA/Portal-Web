using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.ListarSolicitudes;
using ViewModel.Datos.Embarque.GestionarMemo;

namespace Web.Principal.Areas.GestionarEmbarques.Models
{
    public class ListarSolicitudesMemoModel
    {

        [Display(Name = "Nro. Solicitud")]
        public string CodSolicitud { get; set; }

        [Display(Name = "Estado")]
        public string CodEstado { get; set; }

        public string ReturnUrl { get; set; }
        [Display(Name = "RUC/DNI Consignatario")]
        public string Ruc { get; set; }
        
        public IEnumerable<SolicitudMemoResultVM> listaResultado { get; set; }
    }

}
