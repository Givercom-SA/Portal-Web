using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Models.GestionarMemo
{
    public class ListarEventosMemoResult : BaseResult
    {
        public IEnumerable<EventosMemoResult> ListaEventos { get; set; }
    }

    public class EventosMemoResult
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
