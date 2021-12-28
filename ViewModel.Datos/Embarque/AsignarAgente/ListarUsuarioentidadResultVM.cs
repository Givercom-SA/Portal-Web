using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.Embarque.AsignarAgente
{
    public class ListarUsuarioEntidadResultVM : BaseResultVM
    {
        public List<UsuarioEntidadVM> Usuarios { get; set; }
    }

    public class UsuarioEntidadVM
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
