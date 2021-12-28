using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Models
{
    public class ListaExpressReleaseAceptadasResult : BaseResult
    {
        public List<ExpressReleaseAceptadaResult> listaExpressRelease { get; set; }
    }

    public class ExpressReleaseAceptadaResult
    {
        public int ID_ACEPTA_EXPRESS_RELEASE { get; set; }
        public string AER_KEYBD { get; set; }
        public string AER_NROBL { get; set; }
        public int AER_IDUSUARIO_CREA { get; set; }
        public DateTime AER_FECHA_CREA { get; set; }
    }
}
