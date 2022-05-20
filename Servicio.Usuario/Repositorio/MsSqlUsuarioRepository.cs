using AccesoDatos.Utils;
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
using Servicio.Usuario.Models.Cliente;
using ViewModel.Datos.Perfil;

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


        public DashboardClienteResult DashboardCliente(DashboardClienteParameter parameter) {

            var result = new DashboardClienteResult();

            using (var cnn = new SqlConnection(strConn))
            {
                string spName = "[dbo].[TM_PDWAC_SP_DASHBOARD_ENTIDAD_EXTERNO]";
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@FechaInicio", parameter.FechaInicio, dbType: DbType.DateTime);
                queryParameters.Add("@FechaFin", parameter.FechaFin, dbType: DbType.DateTime);
                queryParameters.Add("@IdUsuario", parameter.IdUsuario, dbType: DbType.Int32);
    

                using (var resultCx = cnn.QueryMultiple(spName, queryParameters, commandType: CommandType.StoredProcedure)) {
                    result.DashboardsXFecha = resultCx.Read<DashboardFecha>().ToList();
                    result.DashboardsXEstado = resultCx.Read<DashboardEstado>().ToList();
                }

                result.IN_CODIGO_RESULTADO = 0;
                result.STR_MENSAJE_BD = "Ok";
            }

            return result;

        }

        public DashboardAdminResult DashboardAdmin(DashboardAdminParameter parameter)
        {

            var result = new DashboardAdminResult();

            using (var cnn = new SqlConnection(strConn))
            {
                string spName = "[dbo].[TM_PDWAC_SP_DASHBOARD_ADMINISTRADOR]";
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@FechaInicio", parameter.FechaInicio, dbType: DbType.DateTime);
                queryParameters.Add("@FechaFin", parameter.FechaFin, dbType: DbType.DateTime);
                
                using (var resultCx = cnn.QueryMultiple(spName, queryParameters, commandType: CommandType.StoredProcedure))
                {
                    result.DashboardsXFecha = resultCx.Read<DashboardFecha>().ToList();
                    result.DashboardsXEstado = resultCx.Read<DashboardEstado>().ToList();
                }

                result.IN_CODIGO_RESULTADO = 0;
                result.STR_MENSAJE_BD = "Ok";
            }

            return result;

        }
        public ListarUsuariosResult ObtenerResultados(ListarUsuariosParameter parameter)
        {
            var result = new ListarUsuariosResult();
          
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
         
            return result;
        }

        public ListarClienteResult ListarClientes(ListarClienteParameter parameter)
        {
            var result = new ListarClienteResult();
            
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_ENTIDAD_FILTRAR";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@TipoDocumento", parameter.TipoDocumento, dbType: DbType.String);
                    queryParameters.Add("@NumeroDocumento", parameter.NumeroDocumento, dbType: DbType.String);
                    queryParameters.Add("@RazonSocialRepresentanteLegal", parameter.RazonSocialRepresentanteLegal, dbType: DbType.String);
                    queryParameters.Add("@IdPerfil", parameter.IdPerfil, dbType: DbType.Int32);
                    queryParameters.Add("@IsActivo", parameter.isActivo, dbType: DbType.Int32);
                    
                    
                    result.Clientes = cnn.Query<Models.Cliente.Cliente>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
                    result.IN_CODIGO_RESULTADO = 0;
                    result.STR_MENSAJE_BD = "";

                }
         

            return result;
        }

        public ListarUsuariosResult ListarClienteUsuarios(ListarUsuariosParameter parameter)
        {
            var result = new ListarUsuariosResult();
           
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_CIENTE_USUARIO_FILTRAR]";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@Correo", parameter.Correo, dbType: DbType.String);
                    queryParameters.Add("@Nombres", parameter.Nombres, dbType: DbType.String);
                    queryParameters.Add("@ApellidoPaterno", parameter.ApellidoPaterno, dbType: DbType.String);
                    queryParameters.Add("@ApellidoMaterno", parameter.ApellidoMaterno, dbType: DbType.String);
                    queryParameters.Add("@IdPerfil", parameter.IdPerdil, dbType: DbType.Int32);
                    queryParameters.Add("@IdEntidad", parameter.IdEntidad, dbType: DbType.Int32);
                    queryParameters.Add("@EsAdmin", parameter.IsAdmin, dbType: DbType.Int32);
                    queryParameters.Add("@Estado", parameter.isActivo, dbType: DbType.Int32);
                    queryParameters.Add("@registroInicio", parameter.RegistroInicio, dbType: DbType.Int32);
                    queryParameters.Add("@RegistroFin", parameter.RegistroFin, dbType: DbType.Int32);
                    queryParameters.Add("@TotalRegistros", direction: ParameterDirection.Output, dbType: DbType.Int32);

                    result.Usuarios = cnn.Query<Models.Usuario.Usuario>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
                    result.IN_CODIGO_RESULTADO = 0;

                    result.TotalRegistros = queryParameters.Get<System.Int32>("@TotalRegistros");

                }
      
            return result;
        }

        public LeerClienteResult LeerCliente(Int64 id)
        {
            var result = new LeerClienteResult();
       
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_ENTIDAD_LEER";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@IdEntidad", id, dbType: DbType.Int64);
                  

                    result.Cliente = cnn.Query<Models.Cliente.Cliente>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    result.IN_CODIGO_RESULTADO = 0;
                    result.STR_MENSAJE_BD = "";

                }
       

            return result;
        }


        public LeerUsuariosResult ObtenerUsuario(int IdUsario)
        {
            var result = new LeerUsuariosResult();
          
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_USUARIO_LEER]";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@IdUsuario", IdUsario, dbType: DbType.Int32);

                    result.Usuario = cnn.Query<Models.Usuario.Usuario>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    result.IN_CODIGO_RESULTADO = 0;
                    result.STR_MENSAJE_BD = "Ok";
                }
        
            return result;
        }

        public ListarUsuariosResult ObtenerUsuariosSecundarios(ListarUsuariosParameter parameter)
        {
            var result = new ListarUsuariosResult();
            
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
         
            return result;
        }

        public UsuarioSecundarioResult CrearUsuarioSecundario(CrearUsuarioSecundarioParameter parameter)
        {
            var result = new UsuarioSecundarioResult();

           
                DataTable DtListaMenus = new DataTable("TM_PDWAC_TY_MENU_PERFIL");
                DtListaMenus.Columns.Add("MENU_ID", typeof(int));
                DtListaMenus.Columns.Add("PERFIL_ID", typeof(int));

                foreach (var itemMenu in parameter.MenusPerfil)
                {
                    DataRow drog = DtListaMenus.NewRow();
                    drog["MENU_ID"] = itemMenu.IdMenu;
                    drog["PERFIL_ID"] = itemMenu.IdPerfil;
                    DtListaMenus.Rows.Add(drog);
                }

                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_USUARIO_SECUNDARIO_CREAR]";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@IdEntidad", parameter.IdEntidad);
                    queryParameters.Add("@IdPerfil", parameter.IdPerfil);
                    queryParameters.Add("@CodTipoEntidad", null);
                    
                    queryParameters.Add("@Correo", parameter.Correo, size: 50);
                    queryParameters.Add("@Contrasenia", parameter.Contrasenia, size: 100);
                    queryParameters.Add("@Nombres", parameter.Nombres, size: 100);
                    queryParameters.Add("@ApellidoPaterno", parameter.ApellidoPaterno, size: 100);
                    queryParameters.Add("@ApellidoMaterno", parameter.ApellidoMaterno, size: 100);
                    queryParameters.Add("@EsAdmin", parameter.EsAdmin);
                    queryParameters.Add("@Activo", parameter.Activo);
                    queryParameters.Add("@IdUsuarioCrea", parameter.IdUsuarioCrea);
                    queryParameters.Add("@ListaMenus", DtListaMenus, DbType.Object);
                    queryParameters.Add("@IdUsuarioNuevo", direction: ParameterDirection.Output, dbType: DbType.Int32);
                    queryParameters.Add("@CodigoRespuesta", direction: ParameterDirection.Output, dbType: DbType.Int32);
                    queryParameters.Add("@MensajeRespuesta", direction: ParameterDirection.Output, dbType: DbType.String, size: 200);
          
                    cnn.Query(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    result.IdUsuario = queryParameters.Get<System.Int32>("@IdUsuarioNuevo");
                    result.IN_CODIGO_RESULTADO = queryParameters.Get<System.Int32>("@CodigoRespuesta");
                    result.STR_MENSAJE_BD = queryParameters.Get<System.String>("@MensajeRespuesta");
                }
        

            return result;
        }

        public UsuarioSecundarioResult CrearUsuario(CrearUsuarioSecundarioParameter parameter)
        { 
            var result = new UsuarioSecundarioResult();

           
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
         

            return result;
        }
        
        public UsuarioSecundarioResult EditarUsuarioSecundario(CrearUsuarioSecundarioParameter parameter)
        {
            var result = new UsuarioSecundarioResult();

           
                DataTable DtListaMenus = new DataTable("TM_PDWAC_TY_MENU_PERFIL");
                DtListaMenus.Columns.Add("MENU_ID", typeof(int));
                DtListaMenus.Columns.Add("PERFIL_ID", typeof(int));

                foreach (var itemMenu in parameter.MenusPerfil)
                {
                    DataRow drog = DtListaMenus.NewRow();
                    drog["MENU_ID"] = itemMenu.IdMenu;
                    drog["PERFIL_ID"] = itemMenu.IdPerfil;
                    DtListaMenus.Rows.Add(drog);
                }

                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_USUARIO_CLIENTE_EDITAR]";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@IdUsuario", parameter.IdUsuario);
                    queryParameters.Add("@IdPerfil", parameter.IdPerfil);
                    queryParameters.Add("@Nombres", parameter.Nombres);
                    queryParameters.Add("@ApellidoPaterno", parameter.ApellidoPaterno);
                    queryParameters.Add("@ApellidoMaterno", parameter.ApellidoMaterno);
                    queryParameters.Add("@Activo", parameter.Activo);
                    queryParameters.Add("@IdUsuarioModifica", parameter.IdUsuarioModifica);
                    queryParameters.Add("@ListaMenus", DtListaMenus, DbType.Object);

                    result= cnn.Query<UsuarioSecundarioResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    result.IN_CODIGO_RESULTADO = 0;
                }
           

            return result;
        }

        public UsuarioSecundarioResult EditarUsuarioInterno(CrearUsuarioSecundarioParameter parameter)
        {
            var result = new UsuarioSecundarioResult();

            
                DataTable DtListaMenus = new DataTable("TM_PDWAC_TY_MENU");
                DtListaMenus.Columns.Add("MENU_ID", typeof(int));

                foreach (int MenuId in parameter.Menus)
                {
                    DataRow drog = DtListaMenus.NewRow();
                    drog["MENU_ID"] = MenuId;
                    DtListaMenus.Rows.Add(drog);
                }

            DataTable DtListaVistas = new DataTable("TM_PDWAC_TY_VISTA_MENU_PERFIL");
            DtListaVistas.Columns.Add("MENU_ID", typeof(int));
            DtListaVistas.Columns.Add("PERFIL_ID", typeof(int));
            DtListaVistas.Columns.Add("VISTA_ID", typeof(int));

            List<VistaMenuVM> vistaMenus= new List<VistaMenuVM>();

            parameter.PerfilMenu.Menus.ForEach(x=> {

                if (x.VistaMenu != null) {
                    var vistaMenu = x.VistaMenu.Where(z => z.IdVistaChecked != null).ToList();
                    if (vistaMenu != null) {

                        vistaMenu.ForEach(y=> {
                            y.IdPerfil = parameter.IdPerfil;
                            y.IdMenu = x.IdMenu;
                            vistaMenus.Add(y);
                        });
                    }
                }
            });

            foreach (var itemVistas in vistaMenus)
            {
                DataRow drog = DtListaVistas.NewRow();
                drog["MENU_ID"] = itemVistas.IdMenu;
                drog["PERFIL_ID"] = itemVistas.IdPerfil;
                drog["VISTA_ID"] = itemVistas.IdVista;
                DtListaVistas.Rows.Add(drog);
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
                    queryParameters.Add("@ListaVistas", DtListaVistas, DbType.Object);
                result = cnn.Query<UsuarioSecundarioResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    
                }
           
            return result;
        }

        public UsuarioSecundarioResult CambiarClaveUsuario(CrearUsuarioSecundarioParameter parameter)
        {
            var result = new UsuarioSecundarioResult();

            
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_USUARIO_CAMBIAR_CLAVE]";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@IdUsuario", parameter.IdUsuario);
                    queryParameters.Add("@Correo", parameter.Correo);
                    queryParameters.Add("@Contrasenia", parameter.Contrasenia);

                    result= cnn.Query<UsuarioSecundarioResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                }
         

            return result;
        }

        public UsuarioSecundarioResult HabilitarUsuario(CrearUsuarioSecundarioParameter parameter)
        {
            var result = new UsuarioSecundarioResult();
            int activo = (parameter.Activo) ? 1 : 0;

         
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_USUARIO_HABILITAR]";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@IdUsuario", parameter.IdUsuario, dbType: DbType.String);
                    queryParameters.Add("@Activo", activo, dbType: DbType.Int32);

                   result= cnn.Query<UsuarioSecundarioResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                  

                  
                }
         

            return result;
        }

        public bool ExisteUsuario(string Correo)
        {
            bool result = false;
           
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_USUARIO_EXISTE]";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@Correo", Correo, dbType: DbType.String);
                    queryParameters.Add("@Existe", direction: ParameterDirection.Output, dbType: DbType.Boolean);

                    cnn.Query(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    result = queryParameters.Get<System.Boolean>("@Existe");
                }
          
            return result;
        }

        public UsuarioSecundarioResult ObtenerUsuarioSecundario(CrearUsuarioSecundarioParameter parameter)
        {
            var result = new UsuarioSecundarioResult();
         
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
           
            return result;
        }

        public ListarUsuarioMenuResult ObtenerListaUsuarioMenu(CrearUsuarioSecundarioParameter parameter)
        {
            int Modo = 2; // Listar Menus por IdUsuario y IdPerfil
            var result = new ListarUsuarioMenuResult();
            List<VistaMenu> vistaMenus = new List<VistaMenu>();
            using (var cnn = new SqlConnection(strConn))
            {
                string spName = "[SEGURIDAD].[TM_PDWAC_SP_MENU_FILTRAR]";
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@Modo", Modo, dbType: DbType.Int32);
                queryParameters.Add("@IdMenu", 0, dbType: DbType.Int32);
                queryParameters.Add("@IdUsuario", parameter.IdUsuario, dbType: DbType.Int32);
                queryParameters.Add("@IdPerfil", parameter.IdPerfil, dbType: DbType.Int32);
                result.Menus = cnn.Query<Models.Usuario.UsuarioMenu>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();

                using (var resultCx = cnn.QueryMultiple(spName, queryParameters, commandType: CommandType.StoredProcedure))
                {
                    result.Menus = resultCx.Read<Models.Usuario.UsuarioMenu>().ToList();
                    vistaMenus = resultCx.Read<VistaMenu>().ToList();
                }

                result.Menus.ForEach(x =>
                {
                    if (vistaMenus != null)
                    {
                        x.VistaMenu = vistaMenus.Where(y => y.IdMenu == x.IdMenu).ToList();
                    }
                });

                result.IN_CODIGO_RESULTADO = 0;

            }

            return result;
        }
        public CambiarPerfilDefectoResult CambiarPerfilDefecto(CambiarPerfilDefectoParameter parameter)
        {

            var result = new CambiarPerfilDefectoResult();
           
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_USUARIO_CAMBIAR_PERFIL]";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@IdUsuario", parameter.IdUsuario, dbType: DbType.Int32);
                    queryParameters.Add("@IdPerfil", parameter.IdPerdil, dbType: DbType.Int32);
                    result = cnn.Query<CambiarPerfilDefectoResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                }
       
            return result;
        }
        public UsuarioSecundarioResult ConfirmarCorreoUsuario(int IdUsuario)
        {
            var result = new UsuarioSecundarioResult();

           
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_USUARIO_CONFIRMAR_CORREO]";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@IdUsuario", IdUsuario, dbType: DbType.String);

                    result = cnn.Query<UsuarioSecundarioResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                }
          

            return result;
        }

    }
}
