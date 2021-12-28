using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Principal.Areas.GestionarAccesos.Models
{
    public class CambiarContraseniaModel
    {
        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "Debe ingresar su contraseña")]
        [DataType(DataType.Password)]
        public string contraseniaActual { get; set; }



        [Display(Name = "Contraseña Nueva")]
        [Required(ErrorMessage = "Debe ingresar su nueva contraseña")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$", ErrorMessage = "La contraseña debe contener al menos una mayúscula, una minúscula, al menos un número, un caracter especial (ej. !@#$%^&*) y mínimo debe ser de 8 caracteres.")]
        public string contraseniaNueva { get; set; }

        [Display(Name = "Confirmar Contraseña")]
        [Required(ErrorMessage = "Debe Confirmar su nueva contraseña")]
        [Compare("contraseniaNueva", ErrorMessage = "Contraseña nueva y confirmar contraseña no coinciden")]
        [DataType(DataType.Password)]
        public string Contrasenia { get; set; }

        public string EsNuevo { get; set; }

















    }
}
