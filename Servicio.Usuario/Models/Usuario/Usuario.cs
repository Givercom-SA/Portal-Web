using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Usuario.Models.Usuario
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public int IdPerfil { get; set; }
        public int IdEntidad { get; set; }
        public string Correo { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public bool EsAdmin { get; set; }
        public bool Activo { get; set; }
        public string FechaRegistro { get; set; }
        public string FechaModificacion { get; set; }
        public string PerfilNombre { get; set; }

        public string EntidadNroDocumneto { get; set; }
        public string EntidadTipoDocumento { get; set; }

        public string EntidadRazonSocial { get; set; }
        public string EntidadRepresentanteNombre { get; set; }
  
        public List<UsuarioMenu> Menus { get; set; }
    }
}
