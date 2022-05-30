using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Models.GestionarMemo
{
    public class CrearMemoEnviadoParameter
    {

        public string KeyBLD { get; set; }
        public string NroBL { get; set; }
        public string Origen { get; set; }
        public string Servicio { get; set; }
        public int IdUsuarioCrea { get; set; }
        public string IdEmpresaGtrm { get; set; }
        public int IdSesion { get; set; }
    }


}
