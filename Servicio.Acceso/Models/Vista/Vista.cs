using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Acceso.Models.Vista
{
    public class Vista
    {
        private string strTipoMenu;

        public int IdVista { get; set; }
        public string VistaNombre { get; set; }
        public string VistaArea { get; set; }
        public string VistaController { get; set; }
        public string VistaAction { get; set; }
        public string VistaVerbo { get; set; }
        public int VistaPrincipal { get; set; }
        public int VistaOpcion { get; set; }
        
        public bool? Activo { get; set; }

        public int? IdPadre { get; set; }
        
  
        public int vistaOrden { get; set; }

        public int? IdUsuarioCrea { get; set; }
        public int? IdUsuarioModifica { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaModifica { get; set; }
        public string UsuarioCreaNombres { get; set; }
        public string UsuarioModificaNombres { get; set; }
        public string NombreControlHtml { get; set; }
        public int IdSesion { get; set; }

    }
}
