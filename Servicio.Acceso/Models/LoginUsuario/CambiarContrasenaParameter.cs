using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Acceso.Models.LoginUsuario
{
    public class CambiarContrasenaParameter
    {
        public int IdUsuario { get; set; }
        public string ContrasenaActual { get; set; }
        public string ContrasenaNuevo { get; set; }
        public bool? EsUsuarioNuevo { get; set; }
    }
}
