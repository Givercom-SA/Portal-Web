using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.Perfil;


namespace Web.Principal.Areas.GestionarAutorizacion.Models
{
    public class PerfilModel
    {
        public int IdPerfil { get; set; }
        [Display(Name = "Nombre")]
        [StringLength(50, ErrorMessage = "El nombre tiene una longitud inválida")]
        [Required(ErrorMessage = "Debe ingresar un nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Dashboard")]
        [StringLength(50, ErrorMessage = "El dashboard tiene una longitud inválida")]
        [Required(ErrorMessage = "Debe ingresar un dashboard")]
        public string Dashboard { get; set; }

        public bool Activo { get; set; }

        [Display(Name = "Tipo ")]
        [RegularExpression(@"^.{4,}$", ErrorMessage = "Debe seleccionar un tipo de perfil")]
        public string Tipo { get; set; }

        public string UsuarioCrea { get; set; }
        public string FechaModifica { get; set; }
        public string UsuarioModifica { get; set; }
        public string FechaRegistro { get; set; }

      
        public int[] Menus { get; set; }
        public int[] VistaMenu { get; set; }

        public List<PerfilVM> Perfiles{ get; set; }

    }
}
