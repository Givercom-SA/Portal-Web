using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Models.SolicitudFacturacion
{
    public class ListarProvisionFacturacionTerceroParameter
    {
        public string KeyBl { get; set; }

        public List<Provision> Provision { get; set; }


    }
    public class Provision
    {
        public int IdProvision { get; set; }
        public string NroBl { get; set; }
        public string keyBl { get; set; }




    }


}
