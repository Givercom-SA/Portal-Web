using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Common.Request;
using ViewModel.Datos.Entidad;

namespace Web.Principal.Areas.GestionarEmbarques.Models.Entidad
{
    public class EntidadTipoModel : DataRequestViewModelResponse
    {


        public ListarEntidadResultVM EntidadesTipo { get; set; }


    }
}
