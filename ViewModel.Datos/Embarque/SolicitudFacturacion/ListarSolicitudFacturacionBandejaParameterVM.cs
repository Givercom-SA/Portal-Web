using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.Embarque.SolicitudFacturacion
{
   public class ListarSolicitudFacturacionBandejaParameterVM
    {
        [Display(Name = "Nro. de Embarcación")]
        public string NroBl { get; set; }
        [Display(Name = "Estado")]
        public string Estado { get; set; }
        [Display(Name = "Código de facturación")]
        public string CodigoFacturacion { get; set; }
        [Display(Name = "Nombre Consignatario")]
        public string SolicitanteNombre { get; set; }
        [Display(Name = "Fecha de Registro")]
        public string FechaRegistro { get; set; }

        public string NroDocumentoConsigntario { get; set; }

        
    }
}
