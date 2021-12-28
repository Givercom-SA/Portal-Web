using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Solicitud.Models
{
    public class ListarSolicitudesParameter
    {
    
        public string CampoRuc { get; set; }

       
        public string CampoRazonSocial { get; set; }

        public string CampoCodSolicitud { get; set; }


        public DateTime? FechaIngreso { get; set; }


    
        public string CodEstado { get; set; }

 
        public string NombreContacto { get; set; }

        public string ReturnUrl { get; set; }

    }
}
