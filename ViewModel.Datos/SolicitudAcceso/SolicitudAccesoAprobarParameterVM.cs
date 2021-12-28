using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.SolictudAcceso
{
    [Serializable]
    public class SolicitudAccesoAprobarParameterVM
    {
        public string CodigoSolicitud { get; set; }
        public int IdSolicitud { get; set; }
        public int IdUsuarioEvalua { get; set; }
        public string EstadoSolicitud { get; set; }
        public string MotivoRechazo { get; set; }
        public string ImagenGrupTransmares { get; set; }

    }

 
}
 
