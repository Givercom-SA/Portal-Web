using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Reclamo
{
    public class ListaEmpresasResultVM : BaseResultVM
    {
        public List<EmpresasReclamoVM> Empresas { get; set; }
    }
    public class EmpresasReclamoVM
    {

        public string CodigoEmpresa { get; set; }
        public string NombreEmpresa { get; set; }

    }
}
