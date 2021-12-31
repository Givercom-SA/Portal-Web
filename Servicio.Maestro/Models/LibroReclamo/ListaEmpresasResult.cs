using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Maestro.Models.LibroReclamo
{
    public class ListaEmpresasResult : BaseResult
    {
        public IEnumerable<EmpresaReclamo> Empresas { get; set; }
    }

    public class EmpresaReclamo
    {
        public string CodigoEmpresa { get; set; }
        public string NombreEmpresa { get; set; }
    }
}
