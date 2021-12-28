using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Usuario.Models.Usuario
{
    public class CrearUsuarioSecundarioParameter
    {
        public int IdUsuario { get; set; }
        public int IdPerfil { get; set; }
        public int IdEntidad { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Correo { get; set; }
        public string Contrasenia { get; set; }
        public string ContraseniaNocifrado { get; set; }
        public bool Activo { get; set; }
        public bool EsAdmin { get; set; }
        public int IdUsuarioCrea { get; set; }
        public int IdUsuarioModifica { get; set; }
        public string UrlConfirmacion { get; set; }
        public bool RequiereConfirmacion { get; set; }

        public string ImagenGrupoTrans { get; set; }
        public List<int> Menus { get; set; }
    }
}

