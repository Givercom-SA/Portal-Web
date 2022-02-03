﻿using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Servicio.Usuario.Models.Usuario;
using Dapper;
using Microsoft.Extensions.Logging;

namespace Servicio.Usuario.Repositorio
{
    public class MsSqlUsuarioRepository : IUsuarioRepository
    {
        private IConfiguration _configuration;
        private readonly ILogger<MsSqlUsuarioRepository> _logger;
        private string strConn { get { return _configuration.GetConnectionString("mssqldb"); } }

        public MsSqlUsuarioRepository(IConfiguration configuration, ILogger<MsSqlUsuarioRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public ListarUsuariosResult ObtenerResultados(ListarUsuariosParameter parameter)
        {
            var result = new ListarUsuariosResult();
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_USUARIO_FILTRAR]";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@Correo", parameter.Correo,dbType:DbType.String);
                    queryParameters.Add("@Nombres", parameter.Nombres, dbType: DbType.String);
                    queryParameters.Add("@ApellidoPaterno", parameter.ApellidoPaterno, dbType: DbType.String);
                    queryParameters.Add("@ApellidoMaterno", parameter.ApellidoMaterno, dbType: DbType.String);
                    queryParameters.Add("@IdPerfil", parameter.IdPerdil, dbType: DbType.Int32);
                    queryParameters.Add("@Estado", parameter.isActivo, dbType: DbType.Int32);
                    queryParameters.Add("@registroInicio", parameter.RegistroInicio, dbType: DbType.Int32);
                    queryParameters.Add("@RegistroFin", parameter.RegistroFin, dbType: DbType.Int32);
                    queryParameters.Add("@TotalRegistros",direction:ParameterDirection.Output, dbType: DbType.Int32);



                    result.Usuarios = cnn.Query<Models.Usuario.Usuario>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
                    result.IN_CODIGO_RESULTADO = 0;



                    result.TotalRegistros = queryParameters.Get<System.Int32>("@TotalRegistros");

                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = 2;
                result.STR_MENSAJE_BD = ex.Message;
            }
            return result;
        }
        public LeerUsuariosResult ObtenerUsuario(int IdUsario)
        {
            var result = new LeerUsuariosResult();
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_USUARIO_LEER]";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@IdUsuario", IdUsario, dbType: DbType.Int32);

                    result.Usuario = cnn.Query<Models.Usuario.Usuario>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    result.IN_CODIGO_RESULTADO = 0;
                    result.STR_MENSAJE_BD = "Ok";
                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = 2;
                result.STR_MENSAJE_BD = ex.Message;
            }
            return result;
        }

        public ListarUsuariosResult ObtenerUsuariosSecundarios(ListarUsuariosParameter parameter)
        {
            var result = new ListarUsuariosResult();
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_USUARIO_SECUNDARIO_FILTRAR]";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@IdEntidad", parameter.IdEntidad, dbType: DbType.Int32);
                    queryParameters.Add("@Correo", parameter.Correo, dbType: DbType.String);
                    queryParameters.Add("@registroInicio", parameter.RegistroInicio, dbType: DbType.Int32);
                    queryParameters.Add("@RegistroFin", parameter.RegistroFin, dbType: DbType.Int32);
                    queryParameters.Add("@IdPerfil", parameter.IdPerdil, dbType: DbType.Int32);
                    queryParameters.Add("@IsActivo", parameter.isActivo, dbType: DbType.Int32);
                    queryParameters.Add("@TotalRegistros", direction: ParameterDirection.Output, dbType: DbType.Int32);

                    result.Usuarios = cnn.Query<Models.Usuario.Usuario>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
                    result.IN_CODIGO_RESULTADO = 0;

                    result.TotalRegistros = queryParameters.Get<System.Int32>("@TotalRegistros");
                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = 2;
                result.STR_MENSAJE_BD = ex.Message;
                _logger.LogError(ex, "ObtenerUsuariosSecundarios");
            }
            return result;
        }

        public UsuarioSecundarioResult CrearUsuarioSecundario(CrearUsuarioSecundarioParameter parameter)
        {
            var result = new UsuarioSecundarioResult();

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
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_USUARIO_SECUNDARIO_CREAR]";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@IdEntidad", parameter.IdEntidad);
                    queryParameters.Add("@IdPerfil", parameter.IdPerfil);
                    queryParameters.Add("@Correo", parameter.Correo);
                    queryParameters.Add("@Contrasenia", parameter.Contrasenia);
                    queryParameters.Add("@Nombres", parameter.Nombres);
                    queryParameters.Add("@ApellidoPaterno", parameter.ApellidoPaterno);
                    queryParameters.Add("@ApellidoMaterno", parameter.ApellidoMaterno);
                    queryParameters.Add("@EsAdmin", parameter.EsAdmin);
                    queryParameters.Add("@Activo", parameter.Activo);
                    queryParameters.Add("@IdUsuarioCrea", parameter.IdUsuarioCrea);
                    queryParameters.Add("@ListaMenus", DtListaMenus, DbType.Object);
                    queryParameters.Add("@IdUsuarioNuevo", direction: ParameterDirection.Output, dbType: DbType.Int32);

                    cnn.Query(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    result.IN_CODIGO_RESULTADO = queryParameters.Get<System.Int32>("@IdUsuarioNuevo");
                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -1;
                result.STR_MENSAJE_BD = ex.Message;
            }

            return result;
        }

        public UsuarioSecundarioResult CrearUsuario(CrearUsuarioSecundarioParameter parameter)
        {
            var result = new UsuarioSecundarioResult();

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
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_USUARIO_CREAR]";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@IdEntidad", parameter.IdEntidad);
                    queryParameters.Add("@IdPerfil", parameter.IdPerfil);
                    queryParameters.Add("@Correo", parameter.Correo);
                    queryParameters.Add("@Contrasenia", parameter.Contrasenia);
                    queryParameters.Add("@Nombres", parameter.Nombres);
                    queryParameters.Add("@ApellidoPaterno", parameter.ApellidoPaterno);
                    queryParameters.Add("@ApellidoMaterno", parameter.ApellidoMaterno);
                    queryParameters.Add("@EsAdmin", parameter.EsAdmin);
                    queryParameters.Add("@Activo", parameter.Activo);
                    queryParameters.Add("@IdUsuarioCrea", parameter.IdUsuarioCrea);
                    queryParameters.Add("@ListaMenus", DtListaMenus, DbType.Object);
                    queryParameters.Add("@IdUsuarioNuevo", direction: ParameterDirection.Output, dbType: DbType.Int32);

                    cnn.Query(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    result.IN_CODIGO_RESULTADO = queryParameters.Get<System.Int32>("@IdUsuarioNuevo");
                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -1;
                result.STR_MENSAJE_BD = ex.Message;
            }

            return result;
        }
        



        public UsuarioSecundarioResult EditarUsuarioSecundario(CrearUsuarioSecundarioParameter parameter)
        {
            var result = new UsuarioSecundarioResult();

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
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_USUARIO_SECUNDARIO_EDITAR]";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@IdUsuario", parameter.IdUsuario);
                    queryParameters.Add("@IdPerfil", parameter.IdPerfil);
                    queryParameters.Add("@Activo", parameter.Activo);
                    queryParameters.Add("@IdUsuarioModifica", parameter.IdUsuarioModifica);
                    queryParameters.Add("@ListaMenus", DtListaMenus, DbType.Object);

                    cnn.Query(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    result.IN_CODIGO_RESULTADO = 0;
                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -1;
                result.STR_MENSAJE_BD = ex.Message;
                _logger.LogError(ex, "ObtenerUsuariosSecundarios");
            }

            return result;
        }

        
        
        public UsuarioSecundarioResult EditarUsuarioInterno(CrearUsuarioSecundarioParameter parameter)
        {
            var result = new UsuarioSecundarioResult();

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
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_USUARIO_INTERNO_EDITAR]";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@IdUsuario", parameter.IdUsuario);
                    queryParameters.Add("@IdPerfil", parameter.IdPerfil);
                    queryParameters.Add("@Nombres", parameter.Nombres);
                    queryParameters.Add("@ApellidoPaterno", parameter.ApellidoPaterno);
                    queryParameters.Add("@ApellidoMaterno", parameter.ApellidoMaterno);
                    queryParameters.Add("@EsAdmin", parameter.EsAdmin);
                    queryParameters.Add("@Activo", parameter.Activo);
                    queryParameters.Add("@IdUsuarioModifica", parameter.IdUsuarioModifica);
                    queryParameters.Add("@ListaMenus", DtListaMenus, DbType.Object);

                    result= cnn.Query<UsuarioSecundarioResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    
                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -1;
                result.STR_MENSAJE_BD = ex.Message;
                _logger.LogError(ex, "ObtenerUsuariosSecundarios");
            }

            return result;
        }

        public UsuarioSecundarioResult CambiarClaveUsuario(CrearUsuarioSecundarioParameter parameter)
        {
            var result = new UsuarioSecundarioResult();

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_USUARIO_CAMBIAR_CLAVE]";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@IdUsuario", parameter.IdUsuario);
                    queryParameters.Add("@Correo", parameter.Correo);
                    queryParameters.Add("@Contrasenia", parameter.Contrasenia);

                    result= cnn.Query<UsuarioSecundarioResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -1;
                result.STR_MENSAJE_BD = ex.Message;
            }

            return result;
        }

        public UsuarioSecundarioResult HabilitarUsuario(CrearUsuarioSecundarioParameter parameter)
        {
            var result = new UsuarioSecundarioResult();
            int activo = (parameter.Activo) ? 1 : 0;

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_USUARIO_HABILITAR]";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@IdUsuario", parameter.IdUsuario, dbType: DbType.String);
                    queryParameters.Add("@Activo", activo, dbType: DbType.Int32);

                   result= cnn.Query<UsuarioSecundarioResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                  

                  
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "HabilitarUsuario");
                result.IN_CODIGO_RESULTADO = -200;
                result.STR_MENSAJE_BD = "Estimado usuario, ocurrio un error inesperado por favor volver a intentar nuevamente.";
            }

            return result;
        }

        public bool ExisteUsuario(string Correo)
        {
            bool result = false;
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_USUARIO_EXISTE]";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@Correo", Correo, dbType: DbType.String);
                    queryParameters.Add("@Existe", direction: ParameterDirection.Output, dbType: DbType.Boolean);

                    cnn.Query(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    result = queryParameters.Get<System.Boolean>("@Existe");
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public UsuarioSecundarioResult ObtenerUsuarioSecundario(CrearUsuarioSecundarioParameter parameter)
        {
            var result = new UsuarioSecundarioResult();
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_USUARIO_OBTENER]";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@IdUsuario", parameter.IdUsuario, dbType: DbType.Int32);
                    queryParameters.Add("@Correo", parameter.Correo, dbType: DbType.String);
                    result.usuario = cnn.Query<Models.Usuario.Usuario>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    result.IN_CODIGO_RESULTADO = 0;

                    if (result.usuario != null)
                    {
                        parameter.IdUsuario = result.usuario.IdUsuario;
                        parameter.IdPerfil = result.usuario.IdPerfil;
                        var resultMenus = ObtenerListaUsuarioMenu(parameter);
                        if (resultMenus.IN_CODIGO_RESULTADO == 0)
                        {
                            result.usuario.Menus = resultMenus.Menus;
                        }
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

        public ListarUsuarioMenuResult ObtenerListaUsuarioMenu(CrearUsuarioSecundarioParameter parameter)
        {
            int Modo = 2; // Listar Menus por IdUsuario y IdPerfil
            var result = new ListarUsuarioMenuResult();
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_MENU_FILTRAR]";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@Modo", Modo, dbType: DbType.Int32);
                    queryParameters.Add("@IdMenu", 0, dbType: DbType.Int32);
                    queryParameters.Add("@IdUsuario", parameter.IdUsuario, dbType: DbType.Int32);
                    queryParameters.Add("@IdPerfil", parameter.IdPerfil, dbType: DbType.Int32);
                    result.Menus = cnn.Query<Models.Usuario.UsuarioMenu>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
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
        public CambiarPerfilDefectoResult CambiarPerfilDefecto(CambiarPerfilDefectoParameter parameter)
        {

            var result = new CambiarPerfilDefectoResult();
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_USUARIO_CAMBIAR_PERFIL]";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@IdUsuario", parameter.IdUsuario, dbType: DbType.Int32);
                    queryParameters.Add("@IdPerfil", parameter.IdPerdil, dbType: DbType.Int32);
                    result = cnn.Query<CambiarPerfilDefectoResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -1;
                result.STR_MENSAJE_BD = ex.Message;
            }
            return result;
        }
        public UsuarioSecundarioResult ConfirmarCorreoUsuario(int IdUsuario)
        {
            var result = new UsuarioSecundarioResult();

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_USUARIO_CONFIRMAR_CORREO]";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@IdUsuario", IdUsuario, dbType: DbType.String);

                    result = cnn.Query<UsuarioSecundarioResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -1;
                result.STR_MENSAJE_BD = ex.Message;
            }

            return result;
        }

    }
}
