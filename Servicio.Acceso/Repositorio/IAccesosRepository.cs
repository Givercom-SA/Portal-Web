using Servicio.Acceso.Models;
using Servicio.Acceso.Models.LoginUsuario;
using Servicio.Acceso.Models.Perfil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Acceso.Repositorio
{
    public interface IAccesosRepository
    {
        public UsuarioResult ObtenerLogin(string correo, string contrasenia, string ruc);
        public CambiarContrasenaResult ActualizarContrasenia(CambiarContrasenaParameter parameter);
        public ListarPerfilesResult ObtenerPerfiles(string Nombre, int Activo, string tipo);
        public ObtenerPerfilResult ObtenerPerfil(int IdPerfil);
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
