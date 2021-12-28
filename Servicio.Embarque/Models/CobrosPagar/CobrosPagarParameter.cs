using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Models.CobrosPagar
{
    public class CobrosPagarParameter
    {
        public int Id { get; set; }
        public string KeyBLD { get; set; }
        public string BL { get; set; }
        public string BLNieto { get; set; }
        public string ConceptoCodigo { get; set; }
        public int IdUsuario { get; set; }
        public string Estado { get; set; }
        public List<CobrosPagarDetalle> ListaDetalle { get; set; }
    }

    public class CobrosPagarDetalle
    {
        public string RubroCodigo { get; set; }
        public string ConceptoCodigo { get; set; }
        public string Descripcion { get; set; }
        public string Concepto { get; set; }
        public string Moneda { get; set; }
        public string Importe { get; set; }
        public string IGV { get; set; }
        public string Total { get; set; }
        public string FlagAsignacion { get; set; }

        public string BlPagar { get; set; }
        public string KeyBl { get; set; }
        public string IdBlPagar { get; set; }
        public string NroBL { get; set; }
        public string IdProvision { get; set; }

        
    }
}
