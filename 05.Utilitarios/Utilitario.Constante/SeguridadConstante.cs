using System;

namespace Utilitario.Constante
{
    public static class SeguridadConstante
    {
        public static class TipPerfil
        {
            public static string EXTERNO = "TP02";
            public static string INTERNO = "TP01";   
        }

        public static class CodigoSeguridad {

            public static string POSIBLES_CODIGOS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        }

        public static class ModoVisualizacion
        {
            public static string ADMINISTRADOR = "ADMIN";
            public static string ADMIN_INSPECTOR = "ADMIN_INSPECTOR";
            public static string USUARIO = "USUARIO";
        }


    }
}