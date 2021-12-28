using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Models.CobrosPagar
{
    public class ListarCobrosPagarResult : BaseResult
    {
        public List<CobrosPagar> ListaCobrosPagar { get; set; }
    }

    public class CobrosPagar
    {
        public int Id { get; set; }
        public string KeyBLD { get; set; }
        public string BL { get; set; }
        public string BLNieto { get; set; }
        public string RubroCodigo { get; set; }
        public string ConceptoCodigo { get; set; }
        public string Descripcion { get; set; }
        public string Concepto { get; set; }
        public string Moneda { get; set; }
        public string Importe { get; set; }
        public string IGV { get; set; }
        public string Total { get; set; }
        public string FlagAsignacion { get; set; }
        public string Estado { get; set; }
        public string EstadoNombre { get; set; }
        public int IdUsuarioCrea { get; set; }
        public int IdUsuarioModifica { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModifica { get; set; }

        public string EmbarqueKeyBl { get; set; }
        public string EmbarqueNroBl { get; set; }
        public string EmbarqueSelectBLPagar { get; set; }
        public string EmbarqueSelectIdBlPagar { get; set; }
        public string IdProvision { get; set; }



    }
}
