using System;

namespace ViewModel.Tarifario
{
    public class ListarTarifarioParameterVM
    {
        public string Rubro { get; set; }
        public string Servicio { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}
