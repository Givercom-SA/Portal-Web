using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.ListarEventos
{
    public class ListarEventosVW : BaseResultVM
    {
        public IEnumerable<EventoVW> ListaSolicitudes { get; set; }
    }

    public class EventoVW
    {
        public int EventId { get; set; }
        public string NombreUsuario { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaEvento { get; set; }
    }
}
