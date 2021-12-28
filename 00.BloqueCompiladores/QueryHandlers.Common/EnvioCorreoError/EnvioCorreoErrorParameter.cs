
using System;


namespace QueryHandlers.Common.EnvioCorreoError
{
    public class EnvioCorreoErrorParameter : QueryParameter
    {

        public string Asunto { get; set; }
        public string Correo { get; set; }

        public string Mensaje { get; set; }
        public string TipoAsunto { get; set; }
    }
}
