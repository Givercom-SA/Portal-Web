using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.Datos.Autorizacion;

namespace ViewModel.Datos.UsuarioRegistro
{
   public class CrearUsuarioSecundarioParameterVM
    {
        public CrearUsuarioSecundarioParameterVM()
        {

        }

        public string ImagenGrupoTrans { get; set; }
        public int IdUsuario { get; set; }
        public int IdPerfil { get; set; }
        public int IdEntidad { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Correo { get; set; }
        public string Contrasenia { get; set; }
        public string ContraseniaNoCifrado { get; set; }
        public bool Activo { get; set; }
        public bool EsAdmin { get; set; }
        public int IdUsuarioCrea { get; set; }
        public int IdUsuarioModifica { get; set; }
        public string UrlConfirmacion { get; set; }
        public bool RequiereConfirmacion { get; set; }
        public List<int> Menus { get; set; }

        public List<MenuVM> MenusPerfil { get; set; }

        
    }
}
