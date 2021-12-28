using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Acceso.Models.Perfil
{
    public class ListarTransGroupEmpresaResult : BaseResult
    {
        public ListarTransGroupEmpresaResult()
        {

        }


        public List<TransGroupEmpresa> Empresa { get; set; }
    }
    public class TransGroupEmpresa {

        public string GTEM_CODIGO { get; set; }
        public string GTEM_NOMBRES { get; set; }
        public string GTEM_RUC { get; set; }
        public string GTEM_IMAGEN { get; set; }
        

    }


}
