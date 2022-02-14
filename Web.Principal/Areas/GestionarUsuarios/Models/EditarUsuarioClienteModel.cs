using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.UsuarioRegistro;

namespace Web.Principal.Areas.GestionarUsuarios.Models
{
    public class EditarUsuarioClienteModel
    {
        public int IdUsuario { get; set; }
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
        [Remote("ExisteCorreo", "UsuarioSecundario", ErrorMessage = "El Correo ya existe.")]
        public string Correo { get; set; }

        [Display(Name = "Perfil")]
        public int Perfil { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Seleccione Estado")]
        public bool Activo { get; set; }

        public bool EsAdmin { get; set; }

        public int? IdEntidad { get; set; }

        public string PerfilNombre { get; set; }
        public int[] Menus { get; set; }


        [Display(Name = "Usuario de Creación")]
        public string UsuarioCrea { get; set; }
        [Display(Name = "Ultima Usuario que Modifico")]
        public string UsuarioModifica { get; set; }
        [Display(Name = "Fecha de Creación")]
        public DateTime? FechaCrea { get; set; }
        [Display(Name = "Ultima fecha de Modificación")]
        public DateTime? FechaModifica { get; set; }

        [Display(Name = "Confirmo Cuenta")]
        public bool ConfirmarCuenta { get; set; }
        [Display(Name = "Cambio Contraseña")]
        public bool CambioContrasenia { get; set; }

        public List<UsuarioMenuVM> Items { get; set; }

        public UsuarioRegistroVM Usuario { get; set; }
        public List<GruposAutorizacion> Grupos { get; set; }

        public string getNombre()
        {
            String nombreCompleto = this.Nombres + " " + this.ApellidoPaterno + " " + this.ApellidoMaterno;

            return nombreCompleto;
        }

    }

    public class EditarClienteModel
    {
        public int IdUsuario { get; set; }
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

        public string Correo { get; set; }

        [Display(Name = "Perfil")]
        public int Perfil { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Seleccione Estado")]
        public bool Activo { get; set; }

        public bool EsAdmin { get; set; }

        public int? IdEntidad { get; set; }

        public string PerfilNombre { get; set; }
        public int[] Menus { get; set; }


        [Display(Name = "Usuario de Creación")]
        public string UsuarioCrea { get; set; }
        [Display(Name = "Ultima Usuario que Modifico")]
        public string UsuarioModifica { get; set; }
        [Display(Name = "Fecha de Creación")]
        public DateTime? FechaCrea { get; set; }
        [Display(Name = "Ultima fecha de Modificación")]
        public DateTime? FechaModifica { get; set; }

        [Display(Name = "Confirmo Cuenta")]
        public bool ConfirmarCuenta { get; set; }
        [Display(Name = "Cambio Contraseña")]
        public bool CambioContrasenia { get; set; }

        public List<UsuarioMenuVM> Items { get; set; }

        public UsuarioRegistroVM Usuario { get; set; }
        public List<GruposAutorizacion> Grupos { get; set; }

        public string getNombre()
        {
            String nombreCompleto = this.Nombres + " " + this.ApellidoPaterno + " " + this.ApellidoMaterno;

            return nombreCompleto;
        }

    }

    public class MenuAutoricacion { 
    

    
        public int IdMenu{ get; set; }
        public string Nombre { get; set; }

        public List<PerfilAutorizacion> Perfiles { get; set; }

    }

    public class PerfilAutorizacion {

        public bool Checked { get; set; }
        public int IdPerfil { get; set; }
        public string Nombre { get; set; }
      
    }

    public class GruposAutorizacion
    {
        public string Nombre { get; set; }
        public List<MenuAutoricacion> Menus { get; set; }
    }

}
