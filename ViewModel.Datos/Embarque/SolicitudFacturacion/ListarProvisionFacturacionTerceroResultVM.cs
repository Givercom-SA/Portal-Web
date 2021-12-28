using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModel.Datos.Embarque.SolicitudFacturacion
{
    public class ListarProvisionFacturacionTerceroResultVM : BaseResultVM
    {
        public List<ProvisionFacturacionTerceroVM> PrivisionFacturacionTercero { get; set; }
    }

    public class ProvisionFacturacionTerceroVM
    {
        public int IdTerceroDetalle { get; set; }
        public int IdFacturacionTercero { get; set; }
        
        public int IdProvision { get; set; }
        public string AgenteNroDocumento { get; set; }
        public string AgenteRazonSocial { get; set; }
        public string AgenteTipoDocumento { get; set; }
        public string TipoEntidad { get; set; }
        public string ClienteNombre { get; set; }
        public string CodigoClienteAgente { get; set; }
        public string ClienteNroDocumento { get; set; }





    }


}
