using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.ListarTipoEntidadSolicitud
{
    public class ListarEntidadesVW : BaseResultVM
    {
        public IEnumerable<TipoEntidadVW> ListaEntidades { get; set; }
    }

    public class TipoEntidadVW
    {
        public string CodigoEntidad { get; set; }
        public string NombreEntidad { get; set; }
    }
}
