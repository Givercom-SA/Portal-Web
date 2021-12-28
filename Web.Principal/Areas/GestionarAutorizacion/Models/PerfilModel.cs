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
        [Display(Name = "Nombre del perfil")]
        [StringLength(50, ErrorMessage = "El nombre tiene una longitud inválida")]
        [Required(ErrorMessage = "Debe ingresar un nombre")]
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public int[] Menus { get; set; }
    }
}
