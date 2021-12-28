using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Models.SolicitudDireccionamiento
{
    public class ListarEventosResult : BaseResult
    {
        public IEnumerable<EventosResult> ListaEventos { get; set; }
    }

    public class EventosResult
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
