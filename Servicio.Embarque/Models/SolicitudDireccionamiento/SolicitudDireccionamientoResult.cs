using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Models.SolicitudDireccionamiento
{
    public class SolicitudDireccionamientoResult : BaseResult
    {
        public int IN_IDSOLICITUD { get; set; }
        public string VH_CODSOLICITUD{ get; set; }

    }
}
