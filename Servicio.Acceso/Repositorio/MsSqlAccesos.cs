using Dapper;
using Microsoft.Extensions.Configuration;
using Servicio.Acceso.Models;
using Servicio.Acceso.Models.LoginUsuario;
using Servicio.Acceso.Models.Perfil;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Acceso.Repositorio
{
    public class MsSqlAccesos : IAccesosRepository
    {
        private IConfiguration _configuration;
        private string strConn { get { return _configuration.GetConnectionString("mssqldb"); } }

        public MsSqlAccesos(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public UsuarioResult ObtenerLogin(string correo, string contrasenia, string ruc)
        {
            var result = new UsuarioResult();

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_OBTENER_LOGIN";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@USU_CORREO", correo, DbType.String);
                    queryParameters.Add("@USU_CONTRASENIA", contrasenia, DbType.String);
                    queryParameters.Add("@ENTIDAD_NUMERODOC", ruc, DbType.String);
                    queryParameters.Add("@MENSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);

                    result = cnn.Query<UsuarioResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    if (result == null)
                    {
                        result = new UsuarioResult();
                        result.STR_MENSAJE_BD = queryParameters.Get<string>("@MENSAGE");
                        result.IN_CODIGO_RESULTADO = 1;
                    }
                    else
                    {
                        // Traer Menus y Perfiles
                        var resultMenus = ObtenerMenusLogin(result.USU_ID, result.PEFL_ID);
                        result.Menus = resultMenus;
                        var resultPerfiles = ObtenerPerfilesLogin(result.USU_ID, result.PEFL_ID);
                        result.Perfiles = resultPerfiles;

                        var resultDocumentosRevisar = ObtenerDocumentosPermitidosRevisar(result.PEFL_ID);
                        result.DocumentosRevisar = resultDocumentosRevisar;

                        result.IN_CODIGO_RESULTADO = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = 2;
                result.STR_MENSAJE_BD = ex.Message;
            }

            return result;
        }

        public UsuarioResult OtenerUsuario(int IdUsuario)
        {
            var result = new UsuarioResult();

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    
                    string spName = "[dbo].[TM_PDWAC_SP_USUARIO_LEER]";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@USU_ID", IdUsuario, DbType.Int32);
                    queryParameters.Add("@MENSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);

                    result = cnn.Query<UsuarioResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    if (result == null)
                    {
                        result = new UsuarioResult();
                        result.STR_MENSAJE_BD = queryParameters.Get<string>("@MENSAGE");
                        result.IN_CODIGO_RESULTADO = 1;
                    }
                    else
                    {
                        // Traer Menus y Perfiles
                        var resultMenus = ObtenerMenusLogin(result.USU_ID, result.PEFL_ID);
                        result.Menus = resultMenus;
                        var resultPerfiles = ObtenerPerfilesLogin(result.USU_ID, result.PEFL_ID);
                        result.Perfiles = resultPerfiles;
                        result.IN_CODIGO_RESULTADO = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -200;
                result.STR_MENSAJE_BD = "Ocurrio un error inesperado, por favor volver a intentar mas tarde";
            }

            return result;
        }

        private List<MenuLogin> ObtenerMenusLogin(int IdUsuario,int IdPerfil)
        {
            var result = new List<MenuLogin>();
            int Modo = 4; // Obtener Menus por Perfil
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_MENU_FILTRAR]";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@Modo", Modo, DbType.Int32);
                    queryParameters.Add("@IdUsuario", IdUsuario, DbType.Int32);
                    queryParameters.Add("@IdPerfil", IdPerfil, DbType.Int32);
                    result = cnn.Query<MenuLogin>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        private List<PerfilLogin> ObtenerPerfilesLogin(int IdUsuario, int IdPerfil)
        {
            var result = new List<PerfilLogin>();
            int Modo = 3; // Obtener Perfiles Login
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_PERFIL_FILTRAR]";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@Modo", Modo, DbType.Int32);
                    queryParameters.Add("@IdUsuario", IdUsuario, DbType.Int32);
                    queryParameters.Add("@IdPerfil", IdPerfil, DbType.Int32);
                    result = cnn.Query<PerfilLogin>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        public CambiarContrasenaResult ActualizarContrasenia(CambiarContrasenaParameter parameter)
        {
            CambiarContrasenaResult cambiarContrasenaResult = new CambiarContrasenaResult();

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_ACTUALIZAR_CONTRASENIA";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@ID_USUARIO", parameter.IdUsuario, DbType.Int32);
                    queryParameters.Add("@CONTRASENIA_ACTUAL", parameter.ContrasenaActual, DbType.String);
                    queryParameters.Add("@CONTRASENIA_NUEVA", parameter.ContrasenaNuevo, DbType.String);
                    queryParameters.Add("@ES_USUARIO_NUEOV", parameter.EsUsuarioNuevo, DbType.Boolean);
                    
                    cambiarContrasenaResult = cnn.Query<CambiarContrasenaResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

            
                }
            }
            catch (Exception ex)
            {
                cambiarContrasenaResult.IN_CODIGO_RESULTADO = -200;
                cambiarContrasenaResult.STR_MENSAJE_BD = "Ocurrio un error inesperado, por favor volver a intentar mas tarde";
            }

            return cambiarContrasenaResult;
        }

        public ListarPerfilesResult ObtenerPerfiles(string _nombre, int _activo)
        {
            var result = new ListarPerfilesResult();
            int Modo = 0; // Listar Todos los Perfiles
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_PERFIL_FILTRAR]";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@Modo", Modo, DbType.Int32);
                    queryParameters.Add("@IdPerfil", 0, DbType.Int32);
                    queryParameters.Add("@Nombre", _nombre, DbType.String);
                    queryParameters.Add("@IdEntidad", 0, DbType.Int32);
                    queryParameters.Add("@IsActivo", _activo, DbType.Int32);
                    result.Perfiles = cnn.Query<Perfil>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();

                    if (result == null)
                    {
                        result.STR_MENSAJE_BD = "Error interno al recuperar la información.";
                        result.IN_CODIGO_RESULTADO = -1;
                    }
                    else
                        result.IN_CODIGO_RESULTADO = 0;
                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -1;
                result.STR_MENSAJE_BD = ex.Message;
            }

            return result;
        }

        public ListarPerfilesActivosResult ObtenerPerfilesActivos(ListarPerfilesActivosParameter parameter)
        {
            var result = new ListarPerfilesActivosResult();
            
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_PREFIL_ACTIVOS]";
                    result.Perfiles = cnn.Query<Perfil>(spName, null, commandType: CommandType.StoredProcedure).ToList();

                
                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -1;
                result.STR_MENSAJE_BD = ex.Message;
            }

            return result;
        }

        private List<MenuPerfil> ObtenerMenusPorPerfil(int IdPerfil)
        {
            var result = new List<MenuPerfil>();
            int Modo = 3; // Obtener Menus por Perfil
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_MENU_FILTRAR]";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@Modo", Modo, DbType.Int32);
                    queryParameters.Add("@IdPerfil", IdPerfil, DbType.Int32);
                    result = cnn.Query<MenuPerfil>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        public ObtenerPerfilResult ObtenerPerfil(int IdPerfil)
        {
            var result = new ObtenerPerfilResult();
            int Modo = 1; // Obtener Perfiles por IdPerfil
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_PERFIL_FILTRAR]";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@Modo", Modo, DbType.Int32);
                    queryParameters.Add("@IdPerfil", IdPerfil);
                    queryParameters.Add("@Nombre", string.Empty);
                    queryParameters.Add("@IdEntidad", 0);
                    result.Perfil = cnn.Query<Perfil>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    if (result == null)
                    {
                        result.STR_MENSAJE_BD = "Error interno al recuperar la información.";
                        result.IN_CODIGO_RESULTADO = -1;
                    }
                    else
                    {
                        result.IN_CODIGO_RESULTADO = 0;
                        var resultMenus = ObtenerMenusPorPerfil(result.Perfil.IdPerfil);
                        if (resultMenus != null)
                        {
                            result.Perfil.Menus = new List<MenuPerfil>();
                            result.Perfil.Menus = resultMenus;
                            result.IN_CODIGO_RESULTADO = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -1;
                result.STR_MENSAJE_BD = ex.Message;
            }

            return result;
        }

        public ListarPerfilesResult ObtenerPerfilesPorEntidad(int IdEntidad)
        {
            var result = new ListarPerfilesResult();
            int Modo = 2; // Obtener Perfiles por IdEntidad
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_PERFIL_FILTRAR]";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@Modo", Modo, DbType.Int32);
                    queryParameters.Add("@IdPerfil", 0);
                    queryParameters.Add("@Nombre", string.Empty);
                    queryParameters.Add("@IdEntidad", IdEntidad, DbType.Int32);
                    result.Perfiles = cnn.Query<Perfil>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();

                    if (result == null)
                    {
                        result.STR_MENSAJE_BD = "Error interno al recuperar la información.";
                        result.IN_CODIGO_RESULTADO = -1;
                    }
                    else
                    {
                        result.IN_CODIGO_RESULTADO = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -1;
                result.STR_MENSAJE_BD = ex.Message;
            }

            return result;
        }

        public ListarMenusPerfilResult ObtenerMenus()
        {
            var result = new ListarMenusPerfilResult();
            int Modo = 0; // Obtener Todos los Menus
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_MENU_FILTRAR]";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@Modo", Modo, DbType.Int32);
                    result.Menus = cnn.Query<MenuPerfil>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();

                    if (result == null)
                    {
                        result.STR_MENSAJE_BD = "Error interno al recuperar la información.";
                        result.IN_CODIGO_RESULTADO = -1;
                    }
                    else
                    {
                        result.IN_CODIGO_RESULTADO = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -1;
                result.STR_MENSAJE_BD = ex.Message;
            }

            return result;
        }

        public PerfilResult CrearPerfil(PerfilParameter parameter)
        {
            var result = new PerfilResult();

            try
            {
                DataTable DtListaMenus = new DataTable("TM_PDWAC_TY_MENU");
                DtListaMenus.Columns.Add("MENU_ID", typeof(int));

                foreach (int MenuId in parameter.Menus)
                {
                    DataRow drog = DtListaMenus.NewRow();
                    drog["MENU_ID"] = MenuId;
                    DtListaMenus.Rows.Add(drog);
                }

                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_PERFIL_CREAR]";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@Nombre", parameter.Nombre);
                    queryParameters.Add("@ListaMenus", DtListaMenus, DbType.Object);

                    result= cnn.Query<PerfilResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    
                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -1;
                result.STR_MENSAJE_BD = ex.Message;
            }

            return result;
        }

        public PerfilResult EditarPerfil(PerfilParameter parameter)
        {
            var result = new PerfilResult();

            try
            {
                DataTable DtListaMenus = new DataTable("TM_PDWAC_TY_MENU");
                DtListaMenus.Columns.Add("MENU_ID", typeof(int));

                foreach (int MenuId in parameter.Menus)
                {
                    DataRow drog = DtListaMenus.NewRow();
                    drog["MENU_ID"] = MenuId;
                    DtListaMenus.Rows.Add(drog);
                }

                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_PERFIL_EDITAR]";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@IdPerfil", parameter.IdPerfil);
                    queryParameters.Add("@Nombre", parameter.Nombre);
                    queryParameters.Add("@Activo", parameter.Activo);
                    queryParameters.Add("@ListaMenus", DtListaMenus, DbType.Object);

                    cnn.Query(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    result.IN_CODIGO_RESULTADO = 0;
                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -1;
                result.STR_MENSAJE_BD = ex.Message;
            }

            return result;
        }

        public PerfilResult EliminarPerfil(int IdPerfil)
        {
            var result = new PerfilResult();

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_PERFIL_ELIMINAR]";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@IdPerfil", IdPerfil);
                    result = cnn.Query<PerfilResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -1;
                result.STR_MENSAJE_BD = ex.Message;
            }

            return result;
        }

        public ListarTransGroupEmpresaResult ObtenerTransGroupEmpresa()
        {
            var result = new ListarTransGroupEmpresaResult();
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "SEGURIDAD.TM_PDWAC_SP_GTRM_EMPRESA_LISTAR";
                    result.Empresa = cnn.Query<TransGroupEmpresa>(spName, null, commandType: CommandType.StoredProcedure).ToList();
                    if (result == null)
                    {
                        result.STR_MENSAJE_BD = "Error interno al recuperar la información.";
                        result.IN_CODIGO_RESULTADO = -1;
                    }
                    else
                        result.IN_CODIGO_RESULTADO = 0;
                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -1;
                result.STR_MENSAJE_BD = ex.Message;
            }

            return result;
        }

        public List<string> ObtenerDocumentosPermitidosRevisar(int idperfil)
        {
            var result = new List<string>();

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_DOCUMENTOS_PERFIL]";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@ID_PERFIL", idperfil, DbType.Int32);
                    result = cnn.Query<string>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }
    }
}
