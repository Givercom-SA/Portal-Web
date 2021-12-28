using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.Acceso
{
    public class ListarTransGroupEmpresaVM : BaseResultVM
    {
        public List<TransGroupEmpresaVM> Empresa { get; set; }
    }

    public class TransGroupEmpresaVM
    {

        public string Codigo { get; set; }
        public string Nombres { get; set; }
        public string Ruc { get; set; }
        public string Imagen { get; set; }


        public string getNombres() {

            return $"{this.Ruc} - {this.Nombres}";
        
        }


    }
}
