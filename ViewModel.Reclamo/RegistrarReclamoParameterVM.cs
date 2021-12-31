using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Reclamo
{
  public  class RegistrarReclamoParameterVM
    {
        public string CodigoTipoFormulario { get; set; }

        public string Ruc { get; set; }
        public string RazonSocial { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public DateTime FechaIncidencia { get; set; }
        public string CodigoEmpresa { get; set; }
        public string CodigoUnidadNegocio { get; set; }
        public string CodigoTipoDocumento { get; set; }
        public string Observacion { get; set; }

    }
}
