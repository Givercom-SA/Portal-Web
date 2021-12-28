using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Models
{
    public class RegistrarNotificacionArriboParameter
    {
        public string keyBld { get; set; }
        public string NumeracionEmbarque { get; set; }
        public string CodigoGtrmEmpresa { get; set; }
        public int idUsuario { get; set; }
        public string tipoDocumento { get; set; }
    }
}
