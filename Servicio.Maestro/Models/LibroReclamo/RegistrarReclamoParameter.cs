using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Maestro.Models.LibroReclamo
{
    public class RegistrarReclamoParameter
    {
        public string CodigoTipoFormulario { get; set; }

        public string Ruc { get; set; }
        public string RazonSocial { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string    Celular { get; set; }
        public DateTime FechaIncidencia { get; set; }
        public string CodigoEmpresa { get; set; }
        public string CodigoUnidadNegocio { get; set; }
        public string CodigoTipoDocumento { get; set; }
        public string Observacion { get; set; }

    }




}
