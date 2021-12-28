using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Models
{
    public class ListarUsuarioEntidadResult: BaseResult
    {
        public List<UsuarioEntidad> Usuarios { get; set; }
    }

    public class UsuarioEntidad
    {
        public int IdPerfil { get; set; }
        public int IdUsuario { get; set; }
        public int IdEntidad { get; set; }
        public string NombrePerfil { get; set; }
        public string NombreUsuario { get; set; }
        public string Correo { get; set; }
        public string TipoDocumento { get; set; }
        public string NroDocumento { get; set; }
        public string NombreEntidad { get; set; }

    }
}
