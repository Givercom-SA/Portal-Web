using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Datos.Perfil;

namespace ViewModel.Datos.Menu
{
    public class MenuVM 
    {
  

        private string strTipoMenu;

        public int IdSesion { get; set; }
        public int IdMenu { get; set; }
        [Display(Name = "Nombre")]
        [StringLength(50, ErrorMessage = "El nombre tiene una longitud inválida")]
        [Required(ErrorMessage = "Debe ingresar un nombre")]
        public string Nombre { get; set; }
        public string Area { get; set; }
        public string Controlador { get; set; }
        public string Accion { get; set; }
        public bool Activo { get; set; }
        public string TipoMenuNombre { get; set; }

        public int? IdPadre { get; set; }

        [Display(Name = "Visible")]
        [Required(ErrorMessage = "Debe seleccionar la visibilidad")]
        public bool Visible { get; set; }

        [Display(Name = "Opción")]
        [StringLength(2, ErrorMessage = "El tipo de menu tiene una longitud inválida")]
        [Required(ErrorMessage = "Debe ingresar un tipo de menu")]
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
        [Display(Name = "Orden")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(ErrorMessage = "Debe ingresar un orden")]
        public int Orden { get; set; }

        public int? IdUsuarioCrea { get; set; }
        public int? IdUsuarioModifica { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaModifica { get; set; }
        public string UsuarioCreaNombres { get; set; }
        public string UsuarioModificaNombres { get; set; }
        public List<VistaMenuVM> Vistas { get; set; }

    }




}
