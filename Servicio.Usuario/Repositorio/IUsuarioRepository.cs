using Servicio.Usuario.Models.Cliente;
using Servicio.Usuario.Models.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Servicio.Usuario.Repositorio
{
    public interface IUsuarioRepository
    {
        public ListarUsuariosResult ObtenerResultados(ListarUsuariosParameter parameter);
        public ListarUsuariosResult ListarClienteUsuarios(ListarUsuariosParameter parameter);

        public ListarClienteResult ListarClientes(ListarClienteParameter parameter);

        public LeerClienteResult LeerCliente(Int64 id);
        
        public LeerUsuariosResult ObtenerUsuario(int IdUsuario);
        public ListarUsuariosResult ObtenerUsuariosSecundarios(ListarUsuariosParameter parameter);
        public UsuarioSecundarioResult CrearUsuarioSecundario(CrearUsuarioSecundarioParameter parameter);
        public UsuarioSecundarioResult CrearUsuario(CrearUsuarioSecundarioParameter parameter);
        public UsuarioSecundarioResult EditarUsuarioSecundario(CrearUsuarioSecundarioParameter parameter);
        public UsuarioSecundarioResult EditarUsuarioInterno(CrearUsuarioSecundarioParameter parameter);
        public UsuarioSecundarioResult CambiarClaveUsuario(CrearUsuarioSecundarioParameter parameter);
        public UsuarioSecundarioResult ObtenerUsuarioSecundario(CrearUsuarioSecundarioParameter parameter);
        public UsuarioSecundarioResult ObtenerUsuario(CrearUsuarioSecundarioParameter parameter);
        public ListarUsuarioMenuResult ObtenerListaUsuarioMenu(CrearUsuarioSecundarioParameter parameter);
        public UsuarioSecundarioResult HabilitarUsuario(CrearUsuarioSecundarioParameter parameter);
        public bool ExisteUsuario(string Correo);
        public UsuarioSecundarioResult ConfirmarCorreoUsuario(int IdUsuario);
        public CambiarPerfilDefectoResult CambiarPerfilDefecto(CambiarPerfilDefectoParameter parameter);

        public DashboardClienteResult DashboardCliente(DashboardClienteParameter parameter);
        public DashboardAdminResult DashboardAdmin(DashboardAdminParameter parameter);

    }
}
