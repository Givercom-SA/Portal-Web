using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Models.CobrosPagar
{
    public class CobrosPagarPadreKeyBLResult : BaseResult
    {
        public EmbarqueCobroPadre EmbarquePadreKeyBl { get; set; }
    }

    public class EmbarqueCobroPadre
    {
        public string EmbarqueKeyBL { get; set; }
        public string EmbarqueBL { get; set; }
     


    }
}
