using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Datos.LoginInicial;
using ViewModel.Datos.SolictudAcceso;
using ViewModel.Datos.UsuarioRegistro;
using ViewModel.Datos.Perfil;
using Web.Principal.Util;
using ViewModel.Datos.Acceso;
using Service.Common;
using ViewModel.Datos.Menu;
using ViewModel.Datos.Vista;

namespace Web.Principal.ServiceConsumer
{
    public class ServicioAcceso : IServiceConsumer
    {
        private readonly string URL_BASE;
        private const string SERVICIO_ACCESOS = "Accesos/";

        static HttpClient client = new HttpClient();

        public ServicioAcceso(IConfiguration configuration)
        {
            this.URL_BASE = $"{configuration["ConfiguracionServicios:Accesos"]}";
        }

        public async Task<UsuarioRegistroVM> LoginInicial(LoginInicialVW loginInicial)
        {
            var json = JsonConvert.SerializeObject(loginInicial);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "login-usuario";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<UsuarioRegistroVM>();

            return resultado;
        }

        public async Task<UsuarioRegistroVM> ObtenerUsuarioPorId(int IdUsuario)
        {
         

            const string SERVICIO = "usuario-leer";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}/{IdUsuario}";
            var response = await client.GetAsync(uri);

            var resultado = response.ContentAsType<UsuarioRegistroVM>();

            return resultado;
        }
        public async Task<LeerMenusResultVM> LeerMenu(int IdMenu)
        {

            const string SERVICIO = "leer-menu";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}/{IdMenu}";
            var response = await client.GetAsync(uri);

            var resultado = response.ContentAsType<LeerMenusResultVM>();

            return resultado;
        }
        public async Task<LeerMenusResultVM> ListarTodasVistasParaMenu()
        {

            const string SERVICIO = "vistas-para-menu";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.GetAsync(uri);

            var resultado = response.ContentAsType<LeerMenusResultVM>();

            return resultado;
        }

        
        public async Task<LeerVistaResultVM> LeerVista(int IdVista)
        {

            const string SERVICIO = "leer-vista";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}/{IdVista}";
            var response = await client.GetAsync(uri);

            var resultado = response.ContentAsType<LeerVistaResultVM>();


            return resultado;
        }

        public async Task<ListarTodoVistaResultVM> ListarTodoVistas()
        {

            const string SERVICIO = "vistas-todos";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.GetAsync(uri);

            var resultado = response.ContentAsType<ListarTodoVistaResultVM>();


            return resultado;

        }

        public async Task<ListarTodoMenusResultVM> ListarTodoMenus()
        {

            const string SERVICIO = "menus-todos";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.GetAsync(uri);

            var resultado = response.ContentAsType<ListarTodoMenusResultVM>();


            return resultado;

        }
        public async Task<SolicitarAccesoResultVM> SolicitarAcceso(SolicitarAccesoParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "solicitar-acceso";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<SolicitarAccesoResultVM>();

            return resultado;
        }

        public async Task<ListarAreaControllerActionResultVM> ListarSoloArea(ListarAreaControllerActionParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "vista-listar-area";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<ListarAreaControllerActionResultVM>();

            return resultado;
        }
        public async Task<ListarAreaControllerActionResultVM> ListarSoloController(ListarAreaControllerActionParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "vista-listar-controller";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<ListarAreaControllerActionResultVM>();

            return resultado;
        }
        public async Task<ListarAreaControllerActionResultVM> ListarSoloAction(ListarAreaControllerActionParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "vista-listar-action";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<ListarAreaControllerActionResultVM>();

            return resultado;
        }
        public async Task<MantenimientoVistaResultVM> VistaModificar(MantenimientoVistaParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "vista-modificar";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<MantenimientoVistaResultVM>();

            return resultado;
        }
        public async Task<MantenimientoVistaResultVM> VistaRegistrar(MantenimientoVistaParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "vista-registrar";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<MantenimientoVistaResultVM>();

            return resultado;
        }
        public async Task<MantenimientoMenuResultVM> MenuRegistrar(MantenimientoMenuParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "menu-registrar";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<MantenimientoMenuResultVM>();

            return resultado;
        }
        public async Task<MantenimientoMenuResultVM> MenuModificar(MantenimientoMenuParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "menu-modificar";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<MantenimientoMenuResultVM>();

            return resultado;
        }
        public async Task<CodigoGeneradoValidacionResultVM> GenerarCodigoVerificacion(CodigoGeneradoValidacionParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "generar-codigo-validacion";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<CodigoGeneradoValidacionResultVM>();

            return resultado;
        }

        public async Task<ListarVistasResultVM> ListarVistas(ListarVistaParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "obtener-vistas";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<ListarVistasResultVM>();

            return resultado;
        }
        public async Task<ListarMenusResultVM> ListarMenus(ListarMenusParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "obtener-menus";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<ListarMenusResultVM>();

            return resultado;
        }

        public async Task<VerificarCodigoValidacionResultVM> VerificarCodigoValidacion(VerificarCodigoValidacionParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "verificar-codigo-validacion";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<VerificarCodigoValidacionResultVM>();

            return resultado;
        }

        public async Task<CodigoGeneradoValidacionResultVM> GenerarCodigoVerificacionCorreo(CodigoGeneradoValidacionParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "generar-codigo-validacion-correo";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<CodigoGeneradoValidacionResultVM>();

            return resultado;
        }

        public async Task<VerificarCodigoValidacionResultVM> VerificarCodigoValidacionCorreo(VerificarCodigoValidacionParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "verificar-codigo-validacion-correo";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<VerificarCodigoValidacionResultVM>();

            return resultado;
        }

        public async Task<VerificarSolicitudAccesoResultVM> VerificarSolicitudAcceso(VerificarSolicitudAccesoParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "verificar-solicitud-acceso";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<VerificarSolicitudAccesoResultVM>();

            return resultado;
        }

        public async Task<CambiarContrasenaResultVM> ActualizarContrasena(CambiarContrasenaParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "actualizar-contrasenia";

            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<CambiarContrasenaResultVM>();


            return resultado;
        }
  
     
        public async Task<ListarPerfilesResultVM> ObtenerPerfiles(PerfilParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "obtener-perfiles";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<ListarPerfilesResultVM>();

            return resultado;
        }

        public async Task<ListarPerfilesActivosResultVM> ObtenerPerfilesActivos(ListarPerfilActivosParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "obtener-perfiles-activos";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<ListarPerfilesActivosResultVM>();

            return resultado;
        }


        public async Task<TraerPerfilResultVM> ObtenerPerfil(PerfilParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "obtener-perfil";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<TraerPerfilResultVM>();

            return resultado;
        }
        public async Task<TraerPerfilResultVM> ObtenerPerfilUsuario(PerfilParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "obtener-perfil-usuario";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<TraerPerfilResultVM>();

            return resultado;
        }
       
        public async Task<ListarPerfilesResultVM> ObtenerPerfilesPorEntidad(PerfilParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "obtener-perfiles-entidad";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<ListarPerfilesResultVM>();

            return resultado;
        }
        public async Task<ListarMenusPerfilResultVM> ObtenerMenus()
        {
            const string SERVICIO = "obtener-menus";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.GetAsync(uri);

            var resultado = response.ContentAsType<ListarMenusPerfilResultVM>();

            return resultado;
        }

        public async Task<PerfilResultVM> CrearPerfil(PerfilParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "crear-perfil";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<PerfilResultVM>();

            return resultado;
        }

        public async Task<PerfilResultVM> EditarPerfil(PerfilParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "editar-perfil";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<PerfilResultVM>();

            return resultado;
        }
        public async Task<PerfilResultVM> EliminarPerfil(PerfilParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "eliminar-perfil";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<PerfilResultVM>();

            return resultado;
        }

        public async Task<PerfilResultVM> VerificarPerfil(PerfilParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "veridicar-accesos-perfil";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<PerfilResultVM>();

            return resultado;
        }
       

        public async Task<ListarTransGroupEmpresaVM> ObetenerTransGroupEmpresas()
        {
            
            const string SERVICIO = "obtener-transgroup-empresas";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.GetAsync(uri);

            var resultado = response.ContentAsType<ListarTransGroupEmpresaVM>();

            return resultado;
        }

    }
}