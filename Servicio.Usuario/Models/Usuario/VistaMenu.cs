using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Usuario.Models.Usuario
{
    public class VistaMenu
    {
        public int IdVistaMenu { get; set; }
        public int IdMenu { get; set; }
        public int IdPerfil { get; set; }

        public int IdVista { get; set; }
        public string VistaArea { get; set; }
        public string VistaController { get; set; }
        public string VistaAction { get; set; }
        public string VistaVerbo { get; set; }
        public string VistaNombre { get; set; }
        public int VistaPrincipal { get; set; }
        public int VistaOpcion { get; set; }
        public bool Checked { get; set; }
        public string IdVistaChecked
        { get; set; }
        public int Orden { get; set; }
    }
}
