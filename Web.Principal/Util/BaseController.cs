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
using ViewModel.Datos.Perfil;
using ViewModel.Datos.UsuarioRegistro;

namespace Web.Principal.Util
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
                Log4netExtensions.Action(requestData.HttpArea, requestData.HttpController, requestData.HttpAction, requestData.HttpMethod);
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
                    _logger.LogInformation(string.Format(":::SESION HA EXPIRADO O NO EXISTE:::Area={0},Controller={1},Action={2},Method={3} ", requestData.HttpArea, requestData.HttpController, requestData.HttpAction, requestData.HttpMethod));

                    if (!ValidateLogin(requestData))
                    {

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
                }
                else
                {

                    usuario = context.HttpContext.Session.GetUserContent();

                    var validateAccess = ValidateAccess(context, requestData);

                    long.TryParse(context.HttpContext.Session.GetString("parguid"), out long vParguidMenuId);



                    var requestArea = context.RouteData.Values["area"];
                    var requestController = context.RouteData.Values["controller"];
                    var requestAction = context.RouteData.Values["action"];
                    var requestMethod = context.HttpContext.Request.Method;
                    var trazaSeguridad = string.Format("SEGURIDAD:::USUARIO:::{0}:::PERFIL:::{1}:::PERMISO:::{2}:::Area={3},Controller={4},Action={5},Method={6},Opcion={7}", usuario.idUsuario + "-" + usuario.NombresUsuario, usuario.TipoEntidad, "Si", requestArea + "|" + requestData.HttpArea, requestController + "|" + requestData.HttpController, requestAction + "|" + requestData.HttpAction, requestMethod + "|" + requestData.HttpMethod, vParguidMenuId);
                    _logger.LogInformation(trazaSeguridad);



                    if (!validateAccess)
                    {
                        
                        try
                        {
                            // Enviar correo por message

                        }
                        catch (Exception err ) {
                            _logger.LogError(err,"Envio de Notificacion");
                        };

                        if (IsAjaxRequest(context.HttpContext))
                        {
                            context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                            if (requestData.HttpMethod.ToLower().Equals("post"))
                                context.Result = Json(SistemaConstante.Error.YouDoNotHaveAccess);
                            else
                                context.Result = Json(SistemaConstante.Error.YouDoNotHaveAccess);
                        }
                        else {
                            context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                            if (requestData.HttpMethod.ToLower().Equals("post"))
                                context.Result = RedirectToResult(SistemaConstante.RedirectTo.PageNotFound());
                            else
                                context.Result = RedirectToResult(SistemaConstante.RedirectTo.PageNotFound());
                        }
                    }
                    else
                    {

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
                        ViewData["AdminSistema"] = usuario.AdminSistema;
                        ViewData["ModoAdminSistema"] = usuario.ModoAdminSistema;
                        if (usuario.NumeroDocumento != null)
                            ViewData["RucRazonSocial"] = string.Format("{0} ({1} {2})", usuario.RazonSocial, usuario.TipoDocumento, usuario.NumeroDocumento);
                        else
                            ViewData["RucRazonSocial"] = "";

                        ViewData["PerfilesSesion"] = usuario.Perfiles;
                        ViewData["EmpresasSesion"] = usuario.Empresas;
                        ViewData["EmpresaTransmaresSelct"] = usuario.Sesion.NombreTransGroupEmpresaSeleccionado;
                        ViewData["ImagenEmpresaTransmaresSelct"] = usuario.Sesion.ImagenTransGroupEmpresaSeleccionado;
                    }

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


        private bool ValidateAccess(ActionExecutingContext context, MenuElementoVM acceso)//MARC 10091(INTERNO) Refactorizacion de codigo 
        {
            
           //ViewBag.UsuarioConectado = JsonConvert.SerializeObject(usuario);
            
            var tieneAcceso = GetMenuItemActive(context, acceso);
            return tieneAcceso;
        }
        private bool ValidateLogin(MenuElementoVM requestData)
        {
            var compareRouteLoginAfiliado = CompareRoutes(requestData, SistemaConstante.RedirectTo.Login());

            return compareRouteLoginAfiliado ;
        }

        private bool GetMenuItemActive(ActionExecutingContext context, MenuElementoVM acceso)
        {
            var pageHome = SistemaConstante.RedirectTo.Home();
            if (acceso.HttpArea == pageHome.HttpArea &&
                acceso.HttpController == pageHome.HttpController &&
                acceso.HttpAction == pageHome.HttpAction)
                return true;

            var pageLogout = SistemaConstante.RedirectTo.Logout();
            if (acceso.HttpArea == pageLogout.HttpArea &&
                acceso.HttpController == pageLogout.HttpController &&
                acceso.HttpAction == pageLogout.HttpAction)
                return true;

            var updatePassword = SistemaConstante.RedirectTo.UpdatePassword();
            if (acceso.HttpArea == updatePassword.HttpArea &&
                acceso.HttpController == updatePassword.HttpController &&
                acceso.HttpAction == updatePassword.HttpAction)
                return true;

            var cambiarEmpresa = SistemaConstante.RedirectTo.CambiarEmpresa();
            if (acceso.HttpArea == cambiarEmpresa.HttpArea &&
                acceso.HttpController == cambiarEmpresa.HttpController &&
                acceso.HttpAction == cambiarEmpresa.HttpAction)
                return true;

            var cambiarPerfil = SistemaConstante.RedirectTo.CambiarPerfil();
            if (acceso.HttpArea == cambiarPerfil.HttpArea &&
                acceso.HttpController == cambiarPerfil.HttpController &&
                acceso.HttpAction == cambiarPerfil.HttpAction)
                return true;

            var micuenta = SistemaConstante.RedirectTo.MiCuenta();
            if (acceso.HttpArea == micuenta.HttpArea &&
                acceso.HttpController == micuenta.HttpController &&
                acceso.HttpAction == micuenta.HttpAction)
                return true;

            var cerrarInspectoModoAdmin = SistemaConstante.RedirectTo.CerrarInspeccionModoAdmin();
            if (acceso.HttpArea == cerrarInspectoModoAdmin.HttpArea &&
                acceso.HttpController == cerrarInspectoModoAdmin.HttpController &&
                acceso.HttpAction == cerrarInspectoModoAdmin.HttpAction)
                return true;
            
            var usuario = context.HttpContext.Session.GetUserContent();

            if (usuario.ModoAdminSistema == Utilitario.Constante.SeguridadConstante.ModoVisualizacion.ADMINISTRADOR.ToString())
            {
                var updateAdministrador = SistemaConstante.RedirectTo.DashboardAdministrador();
                if (acceso.HttpArea == updateAdministrador.HttpArea &&
                    acceso.HttpController == updateAdministrador.HttpController &&
                    acceso.HttpAction == updateAdministrador.HttpAction)
                    return true;
            }
            else {

                if (usuario.IdEntidad > 0)
                {
                    var updateEntidadEnterno = SistemaConstante.RedirectTo.DashboardEntidadExterna();
                    if (acceso.HttpArea == updateEntidadEnterno.HttpArea &&
                        acceso.HttpController == updateEntidadEnterno.HttpController &&
                        acceso.HttpAction == updateEntidadEnterno.HttpAction)
                        return true;

                }
                else {
                    var updateInterno = SistemaConstante.RedirectTo.DashboardInterno();
                    if (acceso.HttpArea == updateInterno.HttpArea &&
                        acceso.HttpController == updateInterno.HttpController &&
                        acceso.HttpAction == updateInterno.HttpAction)
                        return true;

                }
            }



            if (usuario.Accesos != null && usuario.Accesos.Any())
            {
                if (!long.TryParse(context.HttpContext.Session.GetString("parguid"), out long vParguidMenuId))
                    vParguidMenuId = default(long);

                var accesosEncontrados = usuario.Accesos.Where(x =>
                                            x.HttpController.Equals(acceso.HttpController) &&
                                            x.HttpAction.Equals(acceso.HttpAction) &&
                                            x.HttpMethod.Equals(acceso.HttpMethod) &&
                                            x.IdPerfil ==usuario.IdPerfil &&
                                            x.Checked==true
                                            ).ToList();

                if (accesosEncontrados != null &&
                    accesosEncontrados.Count > 0 &&
                    !string.IsNullOrWhiteSpace(acceso.HttpArea) &&
                    !string.IsNullOrWhiteSpace(acceso.HttpController) &&
                    !string.IsNullOrWhiteSpace(acceso.HttpAction) &&
                    !string.IsNullOrWhiteSpace(acceso.HttpMethod))
                {
                    var accesosFiltrados = usuario.Accesos.Where(x =>
                                            x.HttpArea.Equals(acceso.HttpArea)
                                            && x.HttpController.Equals(acceso.HttpController)
                                            && x.HttpAction.Equals(acceso.HttpAction)
                                            && x.HttpMethod.Equals(acceso.HttpMethod)
                                            &&
                                            x.IdPerfil == usuario.IdPerfil
                                            ).ToList();

                    

                    var menuActivo = new MenuElementoVM();

                    if (accesosFiltrados == null || accesosFiltrados.Any() == false)
                        return false;
                    else if (accesosFiltrados.Any() && accesosFiltrados.Count == 1)
                        menuActivo = usuario.Accesos.Where(y => y.MenuId == accesosFiltrados[0].MenuId && y.IsMainForm == 1 &&
                                            y.IdPerfil == usuario.IdPerfil).SingleOrDefault();
                    else
                        menuActivo = usuario.Accesos.Where(y => y.MenuId == vParguidMenuId && y.IsMainForm == 1 &&
                                            y.IdPerfil == usuario.IdPerfil).SingleOrDefault();

                    if (menuActivo != null)
                        ViewBag.MenuItemActivo = menuActivo.HttpArea + "/" + menuActivo.HttpController + "/" + menuActivo.HttpAction;
                    else
                    {
                        _logger.LogInformation("No se ha encontrado un Menú Activo para el acceso buscado.", acceso);
                        _logger.LogInformation("Accesos filtrados para el acceso buscado.", accesosFiltrados);
                    }

                    return true;
                }
                else if (accesosEncontrados != null && accesosEncontrados.Count > 0 &&
                            !string.IsNullOrWhiteSpace(acceso.HttpController) &&
                            !string.IsNullOrWhiteSpace(acceso.HttpAction) &&
                            !string.IsNullOrWhiteSpace(acceso.HttpMethod))
                    return true;
                else
                    return false;
            }

            return false;
        }

        private bool IsAjaxRequest(HttpContext context)
        {
            if (context.Request == null)
                throw new ArgumentNullException("request");

            if (context.Request.Headers != null)
                return context.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }



        private MenuElementoVM GetRequestData(ActionExecutingContext context)
        {
            var requestArea = context.RouteData.Values["area"];
            var requestController = context.RouteData.Values["controller"];
            var requestAction = context.RouteData.Values["action"];
            var requestMethod = context.HttpContext.Request.Method;
            var requestParemeterParguid = context.HttpContext.Request.Query["parguid"].Count > 0 ? Convert.ToString(context.HttpContext.Request.Query["parguid"]) : string.Empty;


            if (string.IsNullOrEmpty(requestParemeterParguid))
            {
                var session_menuid = ObtenerMenuId(context.HttpContext, context.RouteData);
                if (session_menuid != null) requestParemeterParguid = Encriptador.Instance.EncriptarTexto(session_menuid.ToString());
            }


            if (!string.IsNullOrEmpty(requestParemeterParguid))
            {
                if (context.HttpContext.Session.GetString("parguid") == null || context.HttpContext.Session.GetString("parguid") != requestParemeterParguid)
                {
                    var menuid = Security.Common.Encriptador.Instance.DesencriptarTexto(requestParemeterParguid);
                    context.HttpContext.Session.SetString("parguid", menuid);
                }
            }

            var requestData = new MenuElementoVM
            {
                HttpArea = requestArea != null ? requestArea.ToString().Trim().ToLower() : string.Empty,
                HttpController = requestController != null ? requestController.ToString().Trim().ToLower() : string.Empty,
                HttpAction = requestAction != null ? requestAction.ToString().Trim().ToLower() : string.Empty,
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

        public static bool ControlPermitido(HttpContext contexto, string nombreControlHtml)
        {
            var usuario = contexto.Session.GetUserContent();

            try
            {
                if (usuario.Accesos == null || !usuario.Accesos.Any()) return false;
                return usuario.Accesos.Where(x =>
                                            x.VistaOpcion == 1 &&
                                            x.NameControlHtml.Equals(nombreControlHtml, StringComparison.CurrentCultureIgnoreCase) &&
                                            x.IdPerfil==usuario.IdPerfil).Any();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return false;
            }

      

        }

        public static bool MenuPermitido(HttpContext contexto, int idPerfil, int menuid)
        {
            var usuario = contexto.Session.GetUserContent();
            return usuario.Menus.Any(x => x.IdMenu == menuid && x.IdPerfil==idPerfil);
        }

        public static bool UrlPermitido(HttpContext contexto, RouteData routeData)
        {
            try
            {
                var usuario = contexto.Session.GetUserContent();
                if (usuario.Accesos == null || !usuario.Accesos.Any()) return false;

                var vRouteLista = usuario.Accesos.LastOrDefault(x =>
                                    x.HttpArea.Equals(routeData.Values["Area"].ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                                    x.HttpController.Equals(routeData.Values["Controller"].ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                                    x.HttpAction.Equals(routeData.Values["Action"].ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                                    x.IdPerfil==usuario.IdPerfil
                                    );

                if (vRouteLista == null) return false;

                return vRouteLista != null;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return false;
            }

           
        }


        public bool CompareRoutes(MenuElementoVM acceso, SistemaConstante.Route route)
        {
            bool result = false;

            if (acceso.HttpArea.ToLower() == route.HttpArea.ToLower() &&
                acceso.HttpController.ToLower() == route.HttpController.ToLower() &&
                acceso.HttpAction.ToLower() == route.HttpAction.ToLower())
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
                   { "area",""},
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
