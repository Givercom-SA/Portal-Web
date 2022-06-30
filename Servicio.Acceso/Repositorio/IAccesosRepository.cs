using Servicio.Acceso.Models;
using Servicio.Acceso.Models.LoginUsuario;
using Servicio.Acceso.Models.Menu;
using Servicio.Acceso.Models.Perfil;
using Servicio.Acceso.Models.Vista;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Acceso.Repositorio
{
    public interface IAccesosRepository
    {
        public ListarAreaControllerActionResult ListarSoloAreas(ListarAreaControllerActionParameter parameter);
        public ListarAreaControllerActionResult ListarSoloAction(ListarAreaControllerActionParameter parameter);
            public ListarAreaControllerActionResult ListarSoloController(ListarAreaControllerActionParameter parameter);
        public MantenimientoVistaResult ModificarVista(MantenimientoVistaParameter parameter);
        public MantenimientoVistaResult RegistrarVista(MantenimientoVistaParameter parameter);
        public MantenimientoMenuResult RegistrarMenu(MantenimientoMenuParameter parameter);
        public MantenimientoMenuResult ModificarMenu(MantenimientoMenuParameter parameter);
        public LeerVistaResult LeerVista(int id);
        public ListarTodoVistaResult ListarTodoVistas( );
        public LeerMenuResult ListarTodasVistasParaMenu();
        public ListarTodoMenuResult ListarTodoMenus();
        public LeerMenuResult LeerMenus(Int32 id);
        public ListarVistaResult ListarVistas(ListarVistaParameter parameter);
        public ListarMenuResult ListarMenus(ListarMenuParameter parameter);
        public UsuarioResult ObtenerLogin(string correo, string contrasenia, string ruc);
        public CambiarContrasenaResult ActualizarContrasenia(CambiarContrasenaParameter parameter);
        public ListarPerfilesResult ObtenerPerfiles(string Nombre, int Activo, string tipo);
        public ObtenerPerfilResult ObtenerPerfil(int IdPerfil);
        public ObtenerPerfilResult ObtenerPerfilPorUsuario(int IdPerfil, int IdUsuario);
        public List<PerfilLogin> ObtenerPerfilesLogin(int IdUsuario, int IdPerfil);
         public List<MenuLogin> ObtenerMenusLogin(int IdUsuario, int IdPerfil);
        public ListarPerfilesResult ObtenerPerfilesPorEntidad(int IdEntidad);
        public ListarMenusPerfilResult ObtenerMenus();
        public PerfilResult EditarPerfil(PerfilParameter parameter);
        public PerfilResult CrearPerfil(PerfilParameter parameter);
        public PerfilResult EliminarPerfil(int IdPerfil);
        public PerfilResult VerificarAccesoPerfil(int IdPerfil);
        
        public ListarTransGroupEmpresaResult ObtenerTransGroupEmpresa();
        public UsuarioResult OtenerUsuario(int IdUsuario);
        public List<string> ObtenerDocumentosPermitidosRevisar(int idperfil);
        public ListarPerfilesActivosResult ObtenerPerfilesActivos(ListarPerfilesActivosParameter parameter);
        
    }
}
