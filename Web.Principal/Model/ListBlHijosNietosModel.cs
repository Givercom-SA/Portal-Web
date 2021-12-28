using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Common.Request;

namespace Web.Principal.Model
{
    public class BlHijoNieto
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
       
    }

    public class ListBlHijosNietosModel : DataRequestViewModelResponse
    {
        public List<BlHijoNieto> blHijoNietos { get; set; }
    }
}
