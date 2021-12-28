using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModel.Datos.Embarque.SolicitudDireccionamiento
{
    public class ListarEventosResultVM : BaseResultVM
    {
        public IEnumerable<EventosResultVM> ListaEventos { get; set; }
    }

    public class EventosResultVM
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
