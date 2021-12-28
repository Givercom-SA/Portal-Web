using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Datos.Embarque.CobroPendienteFacturar;
using ViewModel.Datos.Embarque.SolicitudFacturacionTercero;

namespace ViewModel.Datos.Embarque.SolicitudFacturacion
{

    public class ListarProvisionFacturacionTerceroParameterVM
    {
        public string KeyBl { get; set; }

        public List<ProvisionVM> Provision { get; set; }


    }
    public class ProvisionVM
    {
        public string IdProvision { get; set; }
        public string NroBl { get; set; }
        public string keyBl { get; set; }




    }




}
