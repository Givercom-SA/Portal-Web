using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModel.Datos.Embarque.CobrosPagar
{
    public class ListarCobrosPagarPadreBeyBlResultVM : BaseResultVM
    {
        public EmbarqueCobroPadreVM EmbarquePadreKeyBl { get; set; }
    }


    public class EmbarqueCobroPadreVM
    {
        public string EmbarqueKeyBL { get; set; }
        public string EmbarqueBL { get; set; }



    }
}
