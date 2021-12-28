using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Models
{
    public class AsignarAgenteDetalle
    {
        public int ID { get; set; }
        public string KEYBLD { get; set; }
        public string NumeroEmbarque { get; set; }
        public int IdUsuarioAsigna { get; set; }
        public string NombreUsuarioAsigna { get; set; }
        public string CorreoUsuarioAsigna { get; set; }
        public int IdUsuarioAsignado { get; set; }
        public string NombreUsuarioAsignado { get; set; }
        public string CorreoUsuarioAsignado { get; set; }
        public string Observacion { get; set; }
        public string Estado { get; set; }
        public string EstadoNombre { get; set; }
    }
}
