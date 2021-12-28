using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.Entidad
{
    public class ListaTipoEntidadVM : BaseResultVM
    {
        public List<TipoEntidadVM> ListarTipoEntidad { get; set; }
    }

    public class TipoEntidadVM
    {
        public int IdParametro { get; set; }
        public string CodTipoEntidad { get; set; }

        public string NombreTipoEntidad { get; set; }

        public bool Check { get; set; }
    }
}
