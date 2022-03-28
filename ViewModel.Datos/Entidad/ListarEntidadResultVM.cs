using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.Entidad
{
  public  class ListarEntidadResultVM : BaseResultVM
    {
        public IEnumerable<EntidadTipoVM> Entidades { get; set; }
    }
    public class EntidadTipoVM
    {
        public long EntidadId { get; set; }

        public string TipoDocumento { get; set; }

        public string NumeroDocumento { get; set; }
        public string RazonSocial { get; set; }
        public int IdPerfil { get; set; }

        public string PerfilEntidad { get; set; }
        public string CodigoTipoEntidad { get; set; }
        public int IdUsuarioEntidadAdmin { get; set; }
    }
}
