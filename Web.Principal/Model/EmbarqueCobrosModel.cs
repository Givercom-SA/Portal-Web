using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Common.Request;

namespace Web.Principal.Model
{
    public class EmbarqueCobrosModel
    {
        public string RUBRO_C_CODIGO { get; set; }
        public string CONCEP_C_CODIGO { get; set; }
        public string DESCRIPCION { get; set; }
        public string CONCEP_C_DESCRIPCION_COBRO { get; set; }
        public string MONEDA { get; set; }
        public decimal IMPORTE { get; set; }
        public decimal IGV { get; set; }
        public decimal TOTAL { get; set; }
    }

    public class ListaCobrosModel : DataRequestViewModelResponse
    {
        public string NroEmbarque { get; set; }
        public IList<EmbarqueCobrosModel> listaCobros { get; set; }
    }
}
