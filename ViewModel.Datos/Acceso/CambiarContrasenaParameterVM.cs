using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.Acceso
{
  public  class CambiarContrasenaParameterVM
    {
        public int IdUsuario { get; set; }
        public string ContrasenaActual { get; set; }
        public string ContrasenaNuevo { get; set; }

        public bool? EsUsuarioNuevo { get; set; }
    }
}
