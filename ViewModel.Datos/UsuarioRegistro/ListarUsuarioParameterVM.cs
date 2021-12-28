using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.UsuarioRegistro
{
   public class ListarUsuarioParameterVM
    {

        public ListarUsuarioParameterVM() {

        }


        public int RegistroInicio { get; set; }
        public int RegistroFin { get; set; }

        public string Correo { get; set; }
        public string ApellidoPaterno { get; set; }
        public string Nombres { get; set; }
        public string ApellidoMaterno { get; set; }

        public int IdPerdil { get; set; }
        public int IdEntidad { get; set; }

        public int isActivo { get; set; } = -1;

    }
}
