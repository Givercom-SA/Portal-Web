namespace Batch.Correo.Utils
{
    public static class AppSettingKeys
    {
        public static class Colas
        {
            public static readonly string Servidor = "Queue:Server";
        }

        public static class Servicios
        {
            public static readonly string Maestro = "Servicios:Maestro";
        }

        public static class Parametros
        {
            public static readonly string Opcion = "Parametros:Opcion";
            public static readonly string CarpetaF = "Parametros:CarpetaF";
            public static readonly string CarpetaC = "Parametros:CarpetaC";
            
        }

        public static class Cola
        {
            
            public static readonly string Correo = "Cola:Correo";

        }

        public static class Archivos
        {
            public static readonly string Eliminar = "Archivos:Eliminar";
        }
    }
}
