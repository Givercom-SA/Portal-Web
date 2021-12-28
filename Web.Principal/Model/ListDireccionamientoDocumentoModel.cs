using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Common.Request;

namespace Web.Principal.Model
{
    public class ListDireccionamientoDocumentoModel
    {
        public string codSolicitud { get; set; }
        public string codDocumento { get; set; }
        public string CodEstado { get; set; }
        public string CodEstadoRechazo { get; set; }

    }
  
}
