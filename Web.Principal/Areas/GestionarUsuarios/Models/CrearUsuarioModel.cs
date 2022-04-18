using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.UsuarioRegistro;


namespace Web.Principal.Areas.GestionarUsuarios.Models
{
    public class CrearUsuariosModel
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

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Debe ingresar su nombre")]
        public string Nombres { get; set; }
        [Display(Name = "Apellido Paterno")]
        [Required(ErrorMessage = "Debe ingresar su apellido paterno")]
        public string ApellidoPaterno { get; set; }
        [Required(ErrorMessage = "Debe ingresar su apellido materno")]
        [Display(Name = "Apellido Materno")]
        public string ApellidoMaterno { get; set; }

        [Display(Name = "Perfil")]
 
        public string Perfil { get; set; }

        public string EstadoFormulario { get; set; }

        public Int32 IdUsuario { get; set; }

        public bool Activo { get; set; }
        public int[] Menus { get; set; }
        public String ReturnUrl { get; set; }

        public MenuPerfilModel MenuPerfil { get; set; }

        public UsuarioRegistroVM Usuario { get; set; }
        public List<GruposAutorizacion> Grupos { get; set; }



    }

    public class CrearUsuarioIntenoModel
    {
        [Display(Name = "Nombres")]
        [Required(ErrorMessage = "Debe ingresar Nombres")]
        public string Nombres { get; set; }
        [Display(Name = "Apellido Paterno")]
        [Required(ErrorMessage = "Debe ingresar Apellido Paterno")]
        public string ApellidoPaterno { get; set; }
        [Required(ErrorMessage = "Debe ingresar Apellido Materno")]
        [Display(Name = "Apellido Materno")]
        public string ApellidoMaterno { get; set; }
        [Display(Name = "Correo")]
        [Required(ErrorMessage = "Debe ingresar el correo")]
        [EmailAddress(ErrorMessage = "El correo ingresado no es válido")]
        [StringLength(50, ErrorMessage = "El correo ingresado tiene longitud invalida")]
        [Remote("ExisteCorreo", "UsuarioSecundario", ErrorMessage = "El correo ya existe.")]
        public string Correo { get; set; }
        public bool EsAdmin { get; set; }
        public bool Activo { get; set; }

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "Debe ingresar su contraseña")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$", ErrorMessage = "La contraseña debe contener al menos una mayúscula, una minúscula, al menos un número, un caracter especial (ej. !@#$%^&*) y mínimo debe ser de 8 caracteres.")]
        [DataType(DataType.Password)]
        public string Contrasenia { get; set; }

        [Display(Name = "Confirmar Contraseña")]
        [Required(ErrorMessage = "Debe confirmar su contraseña")]
        [Compare("Contrasenia", ErrorMessage = "Contraseña y Confirmar Contraseña no coinciden.")]
        [DataType(DataType.Password)]
        public string ConfirmarContrasenia { get; set; }

        [Display(Name = "Perfil")]
        [Required(ErrorMessage = "Seleccione Perfil")]
        public int Perfil { get; set; }
        public int[] Menus { get; set; }
        public String ReturnUrl { get; set; }

    }
  
}
