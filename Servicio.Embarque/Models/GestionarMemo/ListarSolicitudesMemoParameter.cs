using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Models.GestionarMemo
{
    public class ListarSolicitudesMemoParameter : BaseResult
    {
        public string nroSolicitud { get; set; }
        public string codEstado { get; set; }
        public string strRuc { get; set; }
        public string CodigoEmpresaServicio { get; set; }
    }

  
}
