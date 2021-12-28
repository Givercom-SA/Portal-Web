using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.Parametros
{
    public class ListaParametrosVM : BaseResultVM
    {
        public IEnumerable<ParametrosVM> ListaParametros { get; set; }
    }

    public class ParametrosVM
    {
        public int IdParametro { get; set; }

        public string NombreDescripcion { get; set; }

        public string ValorCodigo { get; set; }
    }
}
