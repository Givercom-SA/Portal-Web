using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Common.Request;

namespace Web.Principal.Model
{
    public class ParameterDireccionamientoDocumentoModel
    {
        public int CantidadNoSeleccionado { get; set; }
        public int CantidadSeleccionado { get; set; }
        public int CantidadDocumentos { get; set; }
        public string CodigoSolicitudDireccionamiento { get; set; }

        public string EstadoCodigo { get; set; }
        public string CodigoMotivoRechazo { get; set; }
        public ListDireccionamientoDocumentoModel[] Documentos { get; set; }
    

    }
  
}
