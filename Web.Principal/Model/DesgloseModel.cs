using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Common.Request;
using ViewModel.Datos.Embarque.CobroPendienteFacturar;

namespace Web.Principal.Model
{
   
    public class ListaDesgloseModel : DataRequestViewModelResponse
    {
        public IList<DesgloseModel> listaDesglose { get; set; }
    }
}
