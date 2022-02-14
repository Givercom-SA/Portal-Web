using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.UsuarioRegistro
{
   public class ListarClienteParameterVM
    {

    

        public string RazonSocialRepresentanteLegal { get; set; }
        public string NumeroDocumento { get; set; }

        public int? IdPerfil { get; set; }
        public string TipoDocumento { get; set; }

        public bool? isActivo { get; set; }

    }
}
