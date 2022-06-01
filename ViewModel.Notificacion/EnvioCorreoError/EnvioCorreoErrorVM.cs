using System;
using System.Collections.Generic;
using System.Text;
using ViewModel.Common.Request;

namespace ViewModel.Notificacion.EnvioCorreoError
{
    public class EnvioCorreoErrorVM : DataRequestViewModel
    {
        public string Correo { get; set; }

        public string Mensaje { get; set; }
        public string Asunto { get; set; }
        public string TipoAsunto { get; set; }
    }
}
