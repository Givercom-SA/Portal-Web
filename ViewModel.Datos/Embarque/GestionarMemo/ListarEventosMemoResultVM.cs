using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModel.Datos.Embarque.GestionarMemo
{
    public class ListarEventosMemoResultVM : BaseResultVM
    {
        public IEnumerable<EventosMemoResultVM> ListaEventos { get; set; }
    }

    public class EventosMemoResultVM
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Descripcion { get; set; }
        public string CorreoUsuario { get; set; }
        
        public DateTime FechaRegistro { get; set; }
    }
}
