using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.Solicitud
{
  public  class ListarSolicitudesParameterVM
    {
        [Display(Name = "RUC o DNI")]
        public string CampoRuc { get; set; }

        [Display(Name = "Razón Social")]
        public string CampoRazonSocial { get; set; }

        [Display(Name = "Nro. Solicitud")]
        public string CampoCodSolicitud { get; set; }

        [Display(Name = "Fecha Ingreso")]
        public DateTime? FechaIngreso { get; set; }


        [Display(Name = "Estado")]
        public string CodEstado { get; set; }

        [Display(Name = "Nombre Contacto")]
        public string NombreContacto { get; set; }



        public string ReturnUrl { get; set; }



        public IEnumerable<SolicitudVM> listaResultado { get; set; }
    }
}
