using System;

namespace ViewModel.Notificacion
{
    public class NotificacionVM
    {
        public string Proceso { get; set; }

        public string Mensaje { get; set; }
        public string Titulo { get; set; }
        public string FechaFormato { get; set; }

        public bool Leido { get; set; }
        public string Link { get; set; }
        public bool ContadorVisible { get; set; }
        public DateTime Fecha { get; set; }
    }
}
