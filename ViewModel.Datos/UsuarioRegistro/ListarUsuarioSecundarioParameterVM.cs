using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.UsuarioRegistro
{
   public class ListarUsuarioSecundarioParameterVM
    {

        public ListarUsuarioSecundarioParameterVM() {

        }


        public int RegistroInicio { get; set; }
        public int RegistroFin { get; set; }

        public string Correo { get; set; }

        public int IdEntidad { get; set; }

    }
}
