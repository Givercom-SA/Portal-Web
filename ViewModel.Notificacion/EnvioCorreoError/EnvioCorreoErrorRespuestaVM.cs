using System;
using System.Collections.Generic;
using System.Text;
using ViewModel.Common.Request;

namespace ViewModel.Notificacion.EnvioCorreoError
{
    public class EnvioCorreoErrorRespuestaVM : DataRequestViewModelResponse
    {

        public bool ResultadoCorreo { get; set; }
    }
}
