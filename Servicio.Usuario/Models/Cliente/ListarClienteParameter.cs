using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Usuario.Models.Cliente
{
    public class ListarClienteParameter
    {
        public string RazonSocialRepresentanteLegal { get; set; }
        public string NumeroDocumento { get; set; }
      
        public int? IdPerfil { get; set; }
        public string TipoDocumento { get; set; }
      
        public bool? isActivo { get; set; } 
    }
}
