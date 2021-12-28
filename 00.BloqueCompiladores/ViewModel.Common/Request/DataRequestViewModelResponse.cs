using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Common.Request
{
    public class DataRequestViewModelResponse
    {
        public string StatusResponse { get; set; }
        public string Message { get; set; }
        public ResultadoServicio Resultado { get; set; }
        public enum ResultadoServicio
        {
            Exito = 1,
            ErrorNoControlado = 3,
            ErrorComunicacion = 4,
            Error = 2
        }

        public DataRequestViewModelResponse()
        {
            StatusResponse = string.Empty;
            Message = string.Empty;
            Resultado = 0;
        }
    }
}
