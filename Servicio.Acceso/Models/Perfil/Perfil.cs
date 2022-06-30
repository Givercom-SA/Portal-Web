using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccesoDatos.Utils;

namespace Servicio.Acceso.Models.Perfil
{
    public class Perfil
    {
        public int IdPerfil { get; set; }
        public string Nombre { get; set; }
        public int Activo { get; set; }
        public int IdSesion { get; set; }
        public int IdUsuarioCrea { get; set; }
        public int IdUsuarioModifica { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaModifica { get; set; }

        public string TipoPerfil { get; set; }
        public string UsuarioCrea { get; set; }
        public string UsuarioModifica { get; set; }

        public string Tipo { get; set; }
        public string Dashboard { get; set; }
        public bool Checked { get; set; }
        public List<MenuPerfil> Menus { get; set; }
        public List<VistaMenu> VistaMenu { get; set; }
    }
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
        public int IdPadre { get; set; }
        public int Orden { get; set; }
        

    }

}
