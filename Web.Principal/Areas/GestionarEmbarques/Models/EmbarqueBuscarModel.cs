using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Common.Request;
using Web.Principal.Model;

namespace Web.Principal.Areas.GestionarEmbarques.Models
{
    public class EmbarqueBuscarModel : DataRequestViewModelResponse
    {
        public string ReturnUrl { get; set; }

        public string CodigoTransGroupEmpresa { get; set; }

        [Display(Name = "Año")]
        
        public string Anio { get; set; }


        [Display(Name = "Tipo Criterio")]

        public string TipoFiltro { get; set; }

        [Display(Name = "Criterio")]
        public string Filtro { get; set; }
        public string TipoEntidad { get; set; }
        public string RucEntidad { get; set; }

        public SelectList ListAnios { get; set; }

        public SelectList TipoFiltros { get; set; }

        public ListaEmbarqueModel listEmbarques { get; set; }

        [Display(Name = "Servicio")]
        public string Servicio { get; set; }

        [Display(Name = "Origen")]
        public string Origen { get; set; }

        public SelectList ListaServicios { get; set; }

        public SelectList ListaOrigen { get; set; }
    }
}
