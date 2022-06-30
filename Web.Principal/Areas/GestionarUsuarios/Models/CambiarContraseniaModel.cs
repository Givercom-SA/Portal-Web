using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.UsuarioRegistro;

namespace Web.Principal.Areas.GestionarUsuarios.Models
{
    public class CambiarContraseniaModel
    {
        [Display(Name = "Correo")]
        [Required(ErrorMessage = "Debe ingresar el correo")]
        [EmailAddress(ErrorMessage = "El correo ingresado no es válido")]
        [StringLength(50, ErrorMessage = "El correo ingresado tiene longitud invalida")]
        [Remote("ExisteCorreo", "UsuarioSecundario", ErrorMessage = "El Correo ya existe.")]
        public string Correo { get; set; }

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "Debe ingresar su contraseña")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$", ErrorMessage = "La contraseña debe contener al menos una mayúscula, una minúscula, al menos un número, un caracter especial (ej. !@#$%^&*) y mínimo debe ser de 8 caracteres.")]
        [DataType(DataType.Password)]
        public string Contrasenia { get; set; }

        [Required(ErrorMessage = "Se requiere id usuario")]
        public Int32 IdUsuario { get; set; }
        public string contraseniaActual { get; set; }
        public string contraseniaNueva { get; set; }        
        public string EsNuevo { get; set; }
    }
}
