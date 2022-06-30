using Servicio.Acceso.Models.Perfil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Acceso.Models.Menu
{
    public class Menu
    {
        private string strTipoMenu;

        public int IdMenu { get; set; }
        public string Nombre { get; set; }
        public string Area { get; set; }
        public string Controlador { get; set; }
        public string Accion { get; set; }
        public bool Activo { get; set; }
        public int IdSesion { get; set; }
        public string TipoMenuNombre { get; set; }
        
        public int? IdPadre { get; set; }
        public bool Visible { get; set; }
        public string TipoMenu
        {
            get
            {

                if (strTipoMenu == null)
                    return "";
                else
                    return strTipoMenu;

            }
            set { strTipoMenu = value; }
        }
        public int Orden { get; set; }

        public int? IdUsuarioCrea { get; set; }
        public int? IdUsuarioModifica { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaModifica { get; set; }
        public string UsuarioCreaNombres { get; set; }
        public string UsuarioModificaNombres { get; set; }

        public List<VistaMenu> Vistas { get; set; }

    }
}
