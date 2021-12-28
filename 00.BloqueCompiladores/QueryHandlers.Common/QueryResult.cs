namespace QueryHandlers.Common
{
    public class QueryResult
    {
        public string StatusResponse { get; set; }
        public string Message { get; set; }
        public ResultadoTransaccion Resultado { get; set; }
        public enum ResultadoTransaccion
        {
            Exito = 1,
            ErrorNoControlado = 3,
            ErrorComunicacion = 4,
            Error = 2
        }
    }
}
