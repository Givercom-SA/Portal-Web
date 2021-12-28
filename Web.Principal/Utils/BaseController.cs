using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Security.Common;
using Service.Common.Logging.Application;
using Service.Common.Logging.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Utilitario.Constante;
using ViewModel.Datos.UsuarioRegistro;

namespace Web.Principal.Utils
{
    public class BaseController : Controller
    {
        private readonly IConfiguration configuration;
        
        private readonly IHttpContextAccessor httpContextAccessor;
        
        private static ILogger _logger = ApplicationLogging.CreateLogger("BaseController");

        private string Uri { get; set; }


        public BaseController(IConfiguration configuration) : base()
        {
            this.configuration = configuration;
        }


 
 

        public BaseController() : base()
        {
        }



        public string GetUriHost() {

            return this.Uri;
           
        
        }

        internal UsuarioRegistroVM usuario { get; set; }
        public ActionResponse ActionResponse { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {

                System.Uri _uri = context.HttpContext.Request.GetUri();

                this.Uri = $"{_uri.Scheme}://{_uri.Authority}";


                var requestData = GetRequestData(context);
                /********************************************************************************/
                /*MARC 10091(INTERNO) : Id unico transaccion */
                Log4netExtensions.SetIdUnicoTransaccion();
                Log4netExtensions.Action(requestData.Area, requestData.Controlador, requestData.Accion, requestData.HttpMethod);
                var sesionUsuario = context.HttpContext.Session.GetString("UserDefault");
                if (sesionUsuario != null)
                {
                    var u = context.HttpContext.Session.GetUserContent();
                    if (u != context.HttpContext.Session.GetUserContent())
                        Log4netExtensions.Usuario(u.idUsuario.ToString(), u.NombresUsuario == null ? u.NumeroDocumento : u.NombresUsuario);
                }

                //MARC 10091(INTERNO) Refactorizacion de codigo 
                if (sesionUsuario == null)
                {
                    _logger.LogInformation(string.Format(":::SESION HA EXPIRADO O NO EXISTE:::Area={0},Controller={1},Action={2},Method={3} ", requestData.Area, requestData.Controlador, requestData.Accion, requestData.HttpMethod));



                    if (IsAjaxRequest(context.HttpContext))
                    {
                        context.Result = RedirectToRequestTimeout();
                        _logger.LogInformation(string.Format(":::SESION HA EXPIRADO O NO EXISTE:::LA SOLICITUD PROVIENE DE UN AJAX , SE REDIRECCIONA A LOGIN"));
                        GuardarTempData("TimeSessionExpired", true);
                    }
                    else
                    {
                        context.Result = RedirectLoginPage();
                    }


                }
                else
                {

                    usuario = context.HttpContext.Session.GetUserContent();

                    long.TryParse(context.HttpContext.Session.GetString("parguid"), out long vParguidMenuId);
                    var requestArea = context.RouteData.Values["area"];
                    var requestController = context.RouteData.Values["controller"];
                    var requestAction = context.RouteData.Values["action"];
                    var requestMethod = context.HttpContext.Request.Method;
                    var trazaSeguridad = string.Format("SEGURIDAD:::USUARIO:::{0}:::PERFIL:::{1}:::PERMISO:::{2}:::Area={3},Controller={4},Action={5},Method={6},Opcion={7}", usuario.idUsuario + "-" + usuario.NombresUsuario, usuario.TipoEntidad, "Si", requestArea + "|" + requestData.Area, requestController + "|" + requestData.Controlador, requestAction + "|" + requestData.Accion, requestMethod + "|" + requestData.HttpMethod, vParguidMenuId);
                    _logger.LogInformation(trazaSeguridad);


                    int IdPerfil = usuario.IdPerfil;
                    string PerfilNombre = usuario.PerfilNombre;
                    ViewData["IdUsuario"] = usuario.idUsuario;
                    ViewData["NombreUsuario"] = string.Format("{0} {1} {2}", usuario.NombresUsuario, usuario.ApellidoPaternousuario, usuario.ApellidoMaternoUsuario);
                    ViewData["CorreoUsuario"] = usuario.CorreoUsuario;
                    if (!string.IsNullOrEmpty(HttpContext.Session.GetSession<string>("IdPerfilSesion")))
                    {
                        IdPerfil = HttpContext.Session.GetSession<int>("IdPerfilSesion");
                        PerfilNombre = usuario.Perfiles.Where(y => y.IdPerfil == IdPerfil).Select(x => x.Nombre).FirstOrDefault();
                    }
                    ViewData["IdPerfil"] = IdPerfil;
                    ViewData["PerfilNombre"] = PerfilNombre;
                    ViewData["MenusSesion"] = usuario.Menus.Where(x => x.IdPerfil == IdPerfil).ToList();
                    ViewData["IdEntidad"] = usuario.IdEntidad;
                    ViewData["CambioClave"] = usuario.isCambioClave;
                    if (usuario.NumeroDocumento != null)
                        ViewData["RucRazonSocial"] = string.Format("{0} ({1} {2})", usuario.RazonSocial, usuario.TipoDocumento, usuario.NumeroDocumento);
                    else
                        ViewData["RucRazonSocial"] = "";

                    ViewData["PerfilesSesion"] = usuario.Perfiles;
                    ViewData["EmpresaTransmaresSelct"] = usuario.Sesion.NombreTransGroupEmpresaSeleccionado;
                    ViewData["ImagenEmpresaTransmaresSelct"] = usuario.Sesion.ImagenTransGroupEmpresaSeleccionado;

                }

            }
            catch (Exception e)
            {
                _logger.LogError(e, "INC-BaseOnActionExecuting : " + e.Message);
                context.Result = RedirectToResult(SistemaConstante.RedirectTo.Error());
            }

            ViewBag.FechaServidor = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.HoraServidor = DateTime.Now.ToString("hh:mm tt", CultureInfo.InvariantCulture);

            base.OnActionExecuting(context);

        }




        private bool IsAjaxRequest(HttpContext context)
        {
            if (context.Request == null)
                throw new ArgumentNullException("request");

            if (context.Request.Headers != null)
                return context.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }



        private MenuLoginVM GetRequestData(ActionExecutingContext context)
        {
            var requestArea = context.RouteData.Values["area"];
            var requestController = context.RouteData.Values["controller"];
            var requestAction = context.RouteData.Values["action"];
            var requestMethod = context.HttpContext.Request.Method;
            var requestParemeterParguid = context.HttpContext.Request.Query["parguid"].Count > 0 ? Convert.ToString(context.HttpContext.Request.Query["parguid"]) : string.Empty;


            if (string.IsNullOrEmpty(requestParemeterParguid))
            {
                var session_menuid = ObtenerMenuId(context.HttpContext, context.RouteData);
                if (session_menuid != null) requestParemeterParguid = Encriptador.EncriptarTexto(session_menuid.ToString());
            }


            if (!string.IsNullOrEmpty(requestParemeterParguid))
            {
                if (context.HttpContext.Session.GetString("parguid") == null || context.HttpContext.Session.GetString("parguid") != requestParemeterParguid)
                {
                    var menuid = Security.Common.Encriptador.DesencriptarTexto(requestParemeterParguid);
                    context.HttpContext.Session.SetString("parguid", menuid);
                }
            }

            var requestData = new MenuLoginVM
            {
                Area = requestArea != null ? requestArea.ToString().Trim().ToLower() : string.Empty,
                Controlador = requestController != null ? requestController.ToString().Trim().ToLower() : string.Empty,
                Accion = requestAction != null ? requestAction.ToString().Trim().ToLower() : string.Empty,
                HttpMethod = requestMethod != null ? requestMethod.ToString().Trim().ToLower() : string.Empty
            };

            return requestData;
        }

        public static long? ObtenerMenuId(HttpContext contexto, RouteData routeData)
        {
            try
            {
                var usuario = contexto.Session.GetUserContent();
                if (usuario.Menus == null || !usuario.Menus.Any()) return null;

                var vRouteLista = usuario.Menus.LastOrDefault(x =>
                                    x.Area.Equals(routeData.Values["Area"].ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                                    x.Controlador.Equals(routeData.Values["Controller"].ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                                    x.Accion.Equals(routeData.Values["Action"].ToString(), StringComparison.CurrentCultureIgnoreCase));

                if (vRouteLista == null) return null;

                return vRouteLista.IdMenu;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return null;
            }

          
        }
        public bool CompareRoutes(MenuLoginVM acceso, SistemaConstante.Route route)
        {
            bool result = false;

            if (acceso.Area.ToLower() == route.HttpArea.ToLower() &&
                acceso.Controlador.ToLower() == route.HttpController.ToLower() &&
                acceso.Accion.ToLower() == route.HttpAction.ToLower())
                result = true;
            else
                result = false;

            return result;
        }
        public RedirectToRouteResult RedirectToResult(SistemaConstante.Route route)
        {
            return new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "area", string.IsNullOrWhiteSpace(route.HttpArea) ? null : route.HttpArea.Trim()},
                    { "controller", string.IsNullOrWhiteSpace(route.HttpController) ? null : route.HttpController.Trim() },
                    { "action", string.IsNullOrWhiteSpace(route.HttpAction) ? null : route.HttpAction.Trim() }
                });
        }



        private RedirectToRouteResult RedirectErrorPage()
        {
            return new RedirectToRouteResult(
               new RouteValueDictionary
               {
                   { "area","exists"},
                    { "controller", "Error"},
                    { "action", "PaginaError" }
               });


        }

        private RedirectToRouteResult RedirectLoginPage()
        {
            return new RedirectToRouteResult(
               new RouteValueDictionary
               {
                   { "area","exists"},
                    { "controller", "Seguridad"},
                    { "action", "Login" }
               });


        }

        private RedirectToRouteResult RedirectToRequestTimeout()
        {
            return RedirectToResult(SistemaConstante.RedirectTo.RequestTimeout());
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var data = ViewData["variable"] == null ? "" : ViewData["variable"].ToString();
            ViewData["variable"] = data + "/" + "1";
            if (ObtenerTempData<bool>("TimeSessionExpired"))
            {
                context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.RequestTimeout;
            }
        }

        public void GuardarTempData(string key, object data)
        {
            TempData[key] = JsonConvert.SerializeObject(data);
        }
        public T ObtenerTempData<T>(string key, bool keep = false)
        {
            var data = TempData[key];

            if (keep)
                TempData.Keep(key);

            if (data == null)
                return default(T);

            return JsonConvert.DeserializeObject<T>(data.ToString());
        }
        public void EliminarTempData(string key)
        {
            TempData.Remove(key);
        }

        public string FormatearErrores(string error)
        {
            return "<li>" + error.Trim() + "</li>";
        }
    }

}
