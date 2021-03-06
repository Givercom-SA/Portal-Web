using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.UsuarioRegistro;


namespace Web.Principal.Areas.GestionarUsuarios.Models
{
    public class UsuarioSecundarioModel
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

        [Display(Name = "Confirmar Contraseña")]
        [Required(ErrorMessage = "Debe Confirmar su contraseña")]
        [Compare("Contrasenia", ErrorMessage = "Contraseña y Confirmar Contraseña no coinciden.")]
        [DataType(DataType.Password)]
        public string ConfirmarContrasenia { get; set; }

        [Display(Name = "Perfil")]
        [Required(ErrorMessage = "Seleccione Perfil")]
        public int Perfil { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Seleccione Estado")]
        public bool Activo { get; set; }

        public string PerfilNombre { get; set; }

        public List<UsuarioMenuVM> Menus { get; set; }


    }
}
