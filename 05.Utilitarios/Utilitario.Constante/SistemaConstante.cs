using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilitario.Constante
{


    public static class SistemaConstante
    {
        public class Route
        {
            public string HttpArea { get; set; }
            public string HttpController { get; set; }
            public string HttpAction { get; set; }
        }

       

        public static RouteData GetRouteData(string area, string controller, string action)
        {
            var routedata = new RouteData();
            routedata.Values.Add("controller", controller);
            routedata.Values.Add("action", action);
            routedata.Values.Add("area", area);
            return routedata;
        }

        public static Route GetRoute(string area, string controller, string action)
        {
            return new Route
            {
                HttpArea = area.ToLower(),
                HttpController = controller.ToLower(),
                HttpAction = action.ToLower()
            };
        }

        public static class RedirectTo
        {
            public static Route Login()
            {
                return GetRoute("", "", "Login");
            }


   
            public static Route Logout()
            {
                return GetRoute("exists", "Seguridad", "CerrarSesionAsync");
            }

            public static Route UpdatePassword()
            {
                return GetRoute("GestionarUsuarios", "Usuario", "CambiarContrasenia");
            }

            public static Route CambiarEmpresa()
            {
                return GetRoute("GestionarDashboards", "inicio", "CambiarEmpresa");
            }

            public static Route MiCuenta()
            {
                return GetRoute("GestionarUsuarios", "Usuario", "CuentaUsuario");
            }

            public static Route CerrarInspeccionModoAdmin()
            {
                return GetRoute("GestionarEmbarques", "Entidad", "CerrarSesionInspector");

            }
            public static Route CambiarPerfil()
            {
                return GetRoute("GestionarDashboards", "inicio", "CambiarPerfil");
            }
            public static Route DashboardAdministrador()
            {
                return GetRoute("GestionarDashboards", "Inicio", "Administracion");
            }


            public static Route DashboardInterno()
            {
                return GetRoute("GestionarDashboards", "Inicio", "Operaciones");
            }
            public static Route DashboardEntidadExterna()
            {
                return GetRoute("GestionarDashboards", "Inicio", "Home");
            }

            public static Route PageNotFound()
            {
                return GetRoute("exists", "Error", "PaginaNoEncontrada");
            }

            public static Route SystemNotAvailable()
            {
                return GetRoute("exists", "Error", "SistemaNoDisponible");
            }

    

            public static Route Home()
            {
                return GetRoute("GestionarDashboards", "Inicio", "Home");
            }


            public static Route RequestTimeout()
            {
                return GetRoute("exists", "Seguridad", "RequestTimeout");
            }

            public static Route Error()
            {
                return GetRoute("exists", "Error", "PaginaError");
            }

     
        }

        public static class Error
        {
            public const string InvalidInputFile = "Archivo de entrada inválido";
            public const string NoRecordsFound = "No se han encontrado registros";
            public const string IncorrectInputData = "Datos de entrada incorrectos";
            public const string DataCannotBeObtained = "No se ha podido obtener los datos";
            public const string ErrorProcessingTheRequest = "Error al procesar la solicitud";
            public const string YouDoNotHaveAccess = "Estimado usuario, no tiene acceso a este recurso.";
            public const string YouDoNotHavePrivileges = "El usuario no cuenta con privilegios";
            
        }

        public static class Cache
        {
            public const string Notificaciones = "Notificaciones";
            public const string Disponibilidad = "Disponibilidad";
            public const string Comunicados = "Comunicados";
        }



    }

}
