using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Datos.ListarSolicitudes;
using ViewModel.Datos.Solicitud;
using ViewModel.Datos.UsuarioRegistro;
using Web.Principal.Util;

namespace Web.Principal.ServiceConsumer
{
    public class ServicioUsuario: IServiceConsumer
    {
        private readonly string URL_BASE;
        private const string SERVICIO_ACCESOS = "Usuario/";
        private const string SERVICIO_CLIENTE = "Cliente/";

        static HttpClient client = new HttpClient();

        public ServicioUsuario(IConfiguration configuration)
        {
            this.URL_BASE = $"{configuration["ConfiguracionServicios:Usuario"]}";
        }

        public async Task<ListarUsuariosResultVM> ObtenerListadoUsuarios(ListarUsuarioParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            const string SERVICIO = "obtener-usuarios";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);
            var resultado = response.ContentAsType<ListarUsuariosResultVM>();

            return resultado;
        }

        public async Task<ListarUsuariosResultVM> ListarClienteUsuarios(ListarUsuarioParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            const string SERVICIO = "listar-cliente-usuarios";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);
            var resultado = response.ContentAsType<ListarUsuariosResultVM>();

            return resultado;
        }

        
        public async Task<ListarClientesResultVM> ListarClientes(ListarClienteParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            const string SERVICIO = "listar-clientes";
            var uri = $"{URL_BASE}{SERVICIO_CLIENTE}{SERVICIO}";
            var response = await client.PostAsync(uri, data);
            var resultado = response.ContentAsType<ListarClientesResultVM>();

            return resultado;
        }

        public async Task<LeerClienteResultVM> LeerCliente(Int64 id)
        {
            const string SERVICIO = "leer-cliente";
            var uri = $"{URL_BASE}{SERVICIO_CLIENTE}{SERVICIO}?Id={id}";
            var response = await client.GetAsync(uri);
            var resultado = response.ContentAsType<LeerClienteResultVM>();
            return resultado;
        }

        public async Task<LeerUsuarioResultVM> ObtenerUsuario(Int32 id)
        {
         
            const string SERVICIO = "obtener-usuario";

            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}/{id}";
            var response = await client.GetAsync(uri);

            var resultado = response.ContentAsType<LeerUsuarioResultVM>();

            return resultado;
        }

        public async Task<ListarUsuariosResultVM> ObtenerListadoUsuariosSecundarios(ListarUsuarioParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            const string SERVICIO = "obtener-usuarios-secundarios";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);
            var resultado = response.ContentAsType<ListarUsuariosResultVM>();

            return resultado;
        }

        public async Task<UsuarioSecundarioResultVM> CrearUsuarioSecundario(CrearUsuarioSecundarioParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            const string SERVICIO = "crear-usuario-secundario";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);
            var resultado = response.ContentAsType<UsuarioSecundarioResultVM>();

            return resultado;
        }

        public async Task<UsuarioSecundarioResultVM> CrearUsuario(CrearUsuarioSecundarioParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            const string SERVICIO = "crear-usuario";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);
            var resultado = response.ContentAsType<UsuarioSecundarioResultVM>();

            return resultado;
        }

        
        public async Task<UsuarioSecundarioResultVM> EditarUsuarioSecundario(CrearUsuarioSecundarioParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            const string SERVICIO = "editar-usuario-secundario";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);
            var resultado = response.ContentAsType<UsuarioSecundarioResultVM>();

            return resultado;
        }

        public async Task<UsuarioSecundarioResultVM> EditarUsuarioInterno(CrearUsuarioSecundarioParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            const string SERVICIO = "editar-usuario-interno";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);
            var resultado = response.ContentAsType<UsuarioSecundarioResultVM>();

            return resultado;
        }

        public async Task<UsuarioSecundarioResultVM> CambiarClaveUsuario(CrearUsuarioSecundarioParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            const string SERVICIO = "cambiar-clave-usuario";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);
            var resultado = response.ContentAsType<UsuarioSecundarioResultVM>();

            return resultado;
        }

        public async Task<UsuarioSecundarioResultVM> HabilitarUsuario(CrearUsuarioSecundarioParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            const string SERVICIO = "habilitar-usuario";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);
            var resultado = response.ContentAsType<UsuarioSecundarioResultVM>();

            return resultado;
        }

        public async Task<UsuarioSecundarioResultVM> ObtenerUsuarioSecundario(CrearUsuarioSecundarioParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            const string SERVICIO = "obtener-usuario-secundario";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);
            var resultado = response.ContentAsType<UsuarioSecundarioResultVM>();

            return resultado;
        }

        public async Task<UsuarioSecundarioResultVM> ObtenerUsuario(CrearUsuarioSecundarioParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            const string SERVICIO = "obtener-usuario";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);
            var resultado = response.ContentAsType<UsuarioSecundarioResultVM>();

            return resultado;
        }
       

        public async Task<ListarUsuarioMenuResultVM> ObtenerListaUsuarioMenu(CrearUsuarioSecundarioParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            const string SERVICIO = "obtener-lista-usuario-menu";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);
            var resultado = response.ContentAsType<ListarUsuarioMenuResultVM>();

            return resultado;
        }

        public async Task<bool> ExisteUsuario(string Correo)
        {
            var json = JsonConvert.SerializeObject(new { Correo });
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            const string SERVICIO = "existe-usuario";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);
            var resultado = response.ContentAsType<bool>();

            return resultado;
        }


        public async Task<CambiarPerfilDefectoResultVM> CambiarPerfilDefecto(CambiarPerfilDefectoParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            const string SERVICIO = "cambiar-perfil-defecto";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);
            var resultado = response.ContentAsType<CambiarPerfilDefectoResultVM>();



            return resultado;
        }

        public async Task<UsuarioSecundarioResultVM> ConfirmarCorreoUsuario(int IdUsuario)
        {
            const string SERVICIO = "confirmar-correo-usuario";

            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}?IdUsuario={IdUsuario}";
            var response = await client.GetAsync(uri);


            var resultado = response.ContentAsType<UsuarioSecundarioResultVM>();

            return resultado;

        }


        public async Task<DashboardClienteResultVM> DashboardCliente(DashboardClienteParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            const string SERVICIO = "dasboard-cliente";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);
            var resultado = response.ContentAsType<DashboardClienteResultVM>();



            return resultado;
        }

        public async Task<DashboardAdminResultVM> DashboardAdmin(DashboardAdminParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            const string SERVICIO = "dasboard-admin";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);
            var resultado = response.ContentAsType<DashboardAdminResultVM>();

            return resultado;
        }

    }
}
