using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Acceso.Models.SolicitarAcceso
{
    public class SolicitarAccesoResult : BaseResult
    {
        public string VH_CODSOLICITUD_ACCESO { get; set; }
        public string Contrasenia
        { get; set; }
    }
}
