using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Solicitud.Models
{
    public class AprobarSolicitudParameter
    {
        public string CodigoSolicitud { get; set; }
        public int IdSolicitud { get; set; }
        public int IdUsuarioEvalua { get; set; }
        public string EstadoSolicitud { get; set; }

        public string Motivorechazo { get; set; }

    }
}
