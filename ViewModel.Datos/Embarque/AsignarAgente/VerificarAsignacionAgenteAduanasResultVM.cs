using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.Embarque.AsignarAgente
{
    public class VerificarAsignacionAgenteAduanasResultVM : BaseResultVM
    {
        public string RazonSocial { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }

        public int EstadoAsignacion { get; set; }
    }

}
