using Dapper;
using Microsoft.Extensions.Configuration;
using Servicio.Acceso.Models;
using Servicio.Acceso.Models.Acceso;
using Servicio.Acceso.Models.LoginUsuario;
using Servicio.Acceso.Models.Menu;
using Servicio.Acceso.Models.Perfil;
using Servicio.Acceso.Models.Vista;
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

                    var resultAccesos = ObtenerAcceso(result.USU_ID);

                    result.ListaAcceso = resultAccesos.Accesos;
                 

                    var resultDocumentosRevisar = ObtenerDocumentosPermitidosRevisar(result.PEFL_ID);
                    result.DocumentosRevisar = resultDocumentosRevisar;

                    result.IN_CODIGO_RESULTADO = 0;
                }
            }


            return result;
        }

        public UsuarioResult OtenerUsuario(int IdUsuario)
        {
            var result = new UsuarioResult();


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

                    var resultMenuVistas = ObtenerMenuVista(result.USU_ID, result.ENTI_ID);
                    result.MenusLogin = resultMenuVistas.Menus;
                    result.MenusLogin.ForEach(x =>
                    {
                        x.VistaMenus = resultMenuVistas.VistaMenus.Where(y => y.IdMenu == x.IdMenu).ToList();
                    });


                    var  resultAccesos =ObtenerAcceso(result.USU_ID);

                    result.ListaAcceso = resultAccesos.Accesos;
                
                    var resultPerfiles = ObtenerPerfilesLogin(result.USU_ID, result.PEFL_ID);
                    result.Perfiles = resultPerfiles;
                    result.IN_CODIGO_RESULTADO = 0;
                }
            }


            return result;
        }

        public List<MenuLogin> ObtenerMenusLogin(int IdUsuario,int IdPerfil)
        {
            var result = new List<MenuLogin>();
            int Modo = 4; // Obtener Menus por Perfil
          
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_MENU_FILTRAR]";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@Modo", Modo, DbType.Int32);
                    queryParameters.Add("@IdUsuario", IdUsuario, DbType.Int32);
                    queryParameters.Add("@IdPerfil", IdPerfil, DbType.Int32);
                    result = cnn.Query<MenuLogin>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
                }
         
            return result;
        }
        public LeerVistaResult LeerVista(int id) {

            var result = new LeerVistaResult();


            using (var cnn = new SqlConnection(strConn))
            {
                string spName = "[SEGURIDAD].[TM_PDWAC_SP_VISTA_LEER]";

                var queryParameters = new DynamicParameters();

                queryParameters.Add("@IdVista", id, DbType.Int32);

                result.Vista = cnn.Query<Vista>(spName, queryParameters, commandType: CommandType.StoredProcedure).First();
            }

            return result;

        }
        public ListarTodoMenuResult ListarTodoMenus() {

            var result = new ListarTodoMenuResult();


            using (var cnn = new SqlConnection(strConn))
            {
                string spName = "[SEGURIDAD].[TM_PDWAC_SP_MENU_LISTAR_TODO]";



                result.Menus = cnn.Query<Menu>(spName, null, commandType: CommandType.StoredProcedure).ToList();
            }

            return result;
        }
        public ListarTodoVistaResult ListarTodoVistas() {

            var result = new ListarTodoVistaResult();


            using (var cnn = new SqlConnection(strConn))
            {
                string spName = "[SEGURIDAD].[TM_PDWAC_SP_VISTA_LISTAR_TODO]";

                

                result.Vistas = cnn.Query<VistaTodo>(spName, null, commandType: CommandType.StoredProcedure).ToList();
            }

            return result;
        }

        public MantenimientoMenuResult RegistrarMenu(MantenimientoMenuParameter parameter) {

            var result = new MantenimientoMenuResult();



            DataTable DtListaVistas = new DataTable("TM_PDWAC_TY_VISTA_MENU_PERFIL");
            DtListaVistas.Columns.Add("MENU_ID", typeof(int));
            DtListaVistas.Columns.Add("PERFIL_ID", typeof(int));
            DtListaVistas.Columns.Add("VISTA_ID", typeof(int));

            foreach (var itemVistas in parameter.Menu.Vistas.Where(x=>x.IdVistaChecked !=null))
            {
                DataRow drog = DtListaVistas.NewRow();
                drog["MENU_ID"] = itemVistas.IdMenu;
                drog["PERFIL_ID"] = itemVistas.IdPerfil;
                drog["VISTA_ID"] = itemVistas.IdVista;
                DtListaVistas.Rows.Add(drog);
            }

            using (var cnn = new SqlConnection(strConn))
            {
                string spName = "[SEGURIDAD].[TM_PDWAC_SP_MENU_REGISTRAR]";

                var queryParameters = new DynamicParameters();

                queryParameters.Add("@MENU_ID", parameter.Menu.IdMenu, DbType.Int32);
                queryParameters.Add("@MENU_NOMBRES", parameter.Menu.Nombre, DbType.String);
                queryParameters.Add("@MENU_AREA", parameter.Menu.Area, DbType.String);
                queryParameters.Add("@MENU_CONTROLLER", parameter.Menu.Controlador, DbType.String);
                queryParameters.Add("@MENU_ACTION", parameter.Menu.Accion, DbType.String);
                queryParameters.Add("@MENU_IDSESION", parameter.Menu.IdSesion, DbType.Int32);
                queryParameters.Add("@MENU_ACTIVO", parameter.Menu.Activo, DbType.Int32);
                queryParameters.Add("@MENU_IDUSUARIO_CREA", parameter.Menu.IdUsuarioCrea, DbType.Int32);
                queryParameters.Add("@MENU_IDUSUARIO_MODIFICA", parameter.Menu.IdUsuarioModifica, DbType.Int32);
                queryParameters.Add("@MENU_TIPO", parameter.Menu.TipoMenu, DbType.String);
                queryParameters.Add("@MENU_IDPADRE", parameter.Menu.IdPadre, DbType.Int32);
                queryParameters.Add("@MENU_VISIBLE", parameter.Menu.Visible, DbType.Int32);
                queryParameters.Add("@MENU_ORDEN", parameter.Menu.Orden, DbType.Int32);
                queryParameters.Add("@ListaVistas", DtListaVistas, DbType.Object);
                result = cnn.Query<MantenimientoMenuResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }

            return result;
        }
         
        public MantenimientoMenuResult ModificarMenu(MantenimientoMenuParameter parameter) {

            var result = new MantenimientoMenuResult();


            DataTable DtListaVistas = new DataTable("TM_PDWAC_TY_VISTA_MENU_PERFIL");
            DtListaVistas.Columns.Add("MENU_ID", typeof(int));
            DtListaVistas.Columns.Add("PERFIL_ID", typeof(int));
            DtListaVistas.Columns.Add("VISTA_ID", typeof(int));

            foreach (var itemVistas in parameter.Menu.Vistas.Where(x => x.IdVistaChecked != null))
            {
                DataRow drog = DtListaVistas.NewRow();
                drog["MENU_ID"] = itemVistas.IdMenu;
                drog["PERFIL_ID"] = itemVistas.IdPerfil;
                drog["VISTA_ID"] = itemVistas.IdVista;
                DtListaVistas.Rows.Add(drog);
            }

            using (var cnn = new SqlConnection(strConn))
            {
                string spName = "[SEGURIDAD].[TM_PDWAC_SP_MENU_MODIFICAR]";

                var queryParameters = new DynamicParameters();


                queryParameters.Add("@MENU_ID", parameter.Menu.IdMenu, DbType.Int32);
                queryParameters.Add("@MENU_NOMBRES", parameter.Menu.Nombre, DbType.String);
                queryParameters.Add("@MENU_AREA", parameter.Menu.Area, DbType.String);
                queryParameters.Add("@MENU_CONTROLLER", parameter.Menu.Controlador, DbType.String);
                queryParameters.Add("@MENU_ACTION", parameter.Menu.Accion, DbType.String);
                queryParameters.Add("@MENU_IDSESION", parameter.Menu.IdSesion, DbType.Int32);
                queryParameters.Add("@MENU_ACTIVO", parameter.Menu.Activo, DbType.Int32);
                queryParameters.Add("@MENU_IDUSUARIO_CREA", parameter.Menu.IdUsuarioCrea, DbType.Int32);
                queryParameters.Add("@MENU_IDUSUARIO_MODIFICA", parameter.Menu.IdUsuarioModifica, DbType.Int32);
                queryParameters.Add("@MENU_TIPO", parameter.Menu.TipoMenu, DbType.String);
                queryParameters.Add("@MENU_IDPADRE", parameter.Menu.IdPadre, DbType.Int32);
                queryParameters.Add("@MENU_VISIBLE", parameter.Menu.Visible, DbType.Int32);
                queryParameters.Add("@MENU_ORDEN", parameter.Menu.Orden, DbType.Int32);
                queryParameters.Add("@ListaVistas", DtListaVistas, DbType.Object);
                result = cnn.Query<MantenimientoMenuResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }

            return result;
        }

        public MantenimientoVistaResult ModificarVista(MantenimientoVistaParameter parameter)
        {
            var result = new MantenimientoVistaResult();


            using (var cnn = new SqlConnection(strConn))
            {
                string spName = "[SEGURIDAD].[TM_PDWAC_SP_VISTA_EDITAR]";

                var queryParameters = new DynamicParameters();

                queryParameters.Add("@VIST_ID", parameter.Vista.IdVista, DbType.Int32);
                queryParameters.Add("@VIST_AREA", parameter.Vista.VistaArea, DbType.String);
                queryParameters.Add("@VIST_NOMBRE", parameter.Vista.VistaNombre, DbType.String);
                queryParameters.Add("@VIST_CONTROLLER", parameter.Vista.VistaController, DbType.String);
                queryParameters.Add("@VIST_ACTION", parameter.Vista.VistaAction, DbType.String);
                queryParameters.Add("@VIST_VERBO", parameter.Vista.VistaVerbo, DbType.String);
                queryParameters.Add("@VIST_VISTA_PRINCIPAL", parameter.Vista.VistaPrincipal, DbType.Int32);
                queryParameters.Add("@VIST_ES_OPCION", parameter.Vista.VistaOpcion, DbType.Int32);
                queryParameters.Add("@VIST_IDUSUARIO_CREA", parameter.Vista.IdUsuarioCrea, DbType.Int32);
                queryParameters.Add("@VIST_IDUSUARIO_MODIFICA", parameter.Vista.IdUsuarioModifica, DbType.Int32);
                queryParameters.Add("@VIST_IDSESION", parameter.Vista.IdSesion, DbType.Int32);
                queryParameters.Add("@VIST_IDPADRE", parameter.Vista.IdPadre, DbType.Int32);
                queryParameters.Add("@VIST_ORDENAR", parameter.Vista.vistaOrden, DbType.Int32);
                queryParameters.Add("@VIST_NOMBRE_CONTROL_HTML", parameter.Vista.NombreControlHtml, DbType.String);

                result = cnn.Query<MantenimientoVistaResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }

            return result;

        }


        public MantenimientoVistaResult RegistrarVista(MantenimientoVistaParameter parameter)
        {
            var result = new MantenimientoVistaResult();


            using (var cnn = new SqlConnection(strConn))
            {
                string spName = "[SEGURIDAD].[TM_PDWAC_SP_VISTA_REGISTRAR]";

                var queryParameters = new DynamicParameters();

                queryParameters.Add("@VIST_ID", parameter.Vista.IdVista, DbType.Int32);
                queryParameters.Add("@VIST_AREA", parameter.Vista.VistaArea, DbType.String);
                queryParameters.Add("@VIST_NOMBRE", parameter.Vista.VistaNombre, DbType.String);
                queryParameters.Add("@VIST_CONTROLLER", parameter.Vista.VistaController, DbType.String);
                queryParameters.Add("@VIST_ACTION", parameter.Vista.VistaAction, DbType.String);
                queryParameters.Add("@VIST_VERBO", parameter.Vista.VistaVerbo, DbType.String);
                queryParameters.Add("@VIST_VISTA_PRINCIPAL", parameter.Vista.VistaPrincipal, DbType.Int32);
                queryParameters.Add("@VIST_ES_OPCION", parameter.Vista.VistaOpcion, DbType.Int32);
                queryParameters.Add("@VIST_IDUSUARIO_CREA", parameter.Vista.IdUsuarioCrea, DbType.Int32);
                queryParameters.Add("@VIST_IDUSUARIO_MODIFICA", parameter.Vista.IdUsuarioModifica, DbType.Int32);
                queryParameters.Add("@VIST_IDSESION", parameter.Vista.IdSesion, DbType.Int32);
                queryParameters.Add("@VIST_IDPADRE", parameter.Vista.IdPadre, DbType.Int32);
                queryParameters.Add("@VIST_ORDENAR", parameter.Vista.vistaOrden, DbType.Int32);
                queryParameters.Add("@VIST_NOMBRE_CONTROL_HTML", parameter.Vista.NombreControlHtml, DbType.String);
                
                result = cnn.Query<MantenimientoVistaResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }

            return result;

        }


        public ListarAreaControllerActionResult ListarSoloAreas(ListarAreaControllerActionParameter parameter)
        {
            var result = new ListarAreaControllerActionResult();


            using (var cnn = new SqlConnection(strConn))
            {
                string spName = "[SEGURIDAD].[TM_PDWAC_SP_MENU_AREA_LISTAR]";
                
                result.AreasControllersActions = cnn.Query<AreaControllerAction>(spName, null, commandType: CommandType.StoredProcedure).ToList();
            }

            return result;

        }

        public ListarAreaControllerActionResult ListarSoloController(ListarAreaControllerActionParameter parameter)
        {
            var result = new ListarAreaControllerActionResult();
            using (var cnn = new SqlConnection(strConn))
            {
                string spName = "[SEGURIDAD].[TM_PDWAC_SP_MENU_CONTROLLER_LISTAR]";
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@Area", parameter.Area, DbType.String);
                result.AreasControllersActions = cnn.Query<AreaControllerAction>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
            }

            return result;

        }
        public ListarAreaControllerActionResult ListarSoloAction(ListarAreaControllerActionParameter parameter)
        {
            var result = new ListarAreaControllerActionResult();
            using (var cnn = new SqlConnection(strConn))
            {
                string spName = "[SEGURIDAD].[TM_PDWAC_SP_MENU_ACTION_LISTAR]";
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@Area", parameter.Area, DbType.String);
                queryParameters.Add("@Controller", parameter.Controller, DbType.String);
                result.AreasControllersActions = cnn.Query<AreaControllerAction>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
            }

            return result;

        }
        public ListarMenuResult ListarMenus(ListarMenuParameter parameter)
        {
            var result = new ListarMenuResult();


            using (var cnn = new SqlConnection(strConn))
            {
                string spName = "[SEGURIDAD].[TM_PDWAC_SP_MENU_LISTAR]";

                var queryParameters = new DynamicParameters();
                
                queryParameters.Add("@IdPadreMenu", parameter.IdMenuPadre, DbType.Int32);
                queryParameters.Add("@Activo", parameter.Estado, DbType.Int32);
                queryParameters.Add("@TipoMenu", parameter.TipoMenu, DbType.String);
                queryParameters.Add("@Nombre", parameter.Nombre, DbType.String);
                result.Menus = cnn.Query<Menu>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
            }

            return result;

        }
        public LeerMenuResult LeerMenus(Int32 id)
        {
            var result = new LeerMenuResult();


            using (var cnn = new SqlConnection(strConn))
            {
                string spName = "[SEGURIDAD].[TM_PDWAC_SP_MENU_LEER]";

                var queryParameters = new DynamicParameters();

                queryParameters.Add("@IdMenu", id, DbType.Int32);
                
                result.Menu = cnn.Query<Menu>(spName, queryParameters, commandType: CommandType.StoredProcedure).First();


                if (result.Menu != null) {

                    var listarPerfileMenuVistaResult =ListarVistasDeMenu(result.Menu.IdMenu);
                    result.Menu.Vistas= listarPerfileMenuVistaResult.VistaMenus;
                }

            }

            return result;

        }

        public LeerMenuResult ListarTodasVistasParaMenu()
        {
            var result = new LeerMenuResult();
            result.Menu = new Menu();

            var listarPerfileMenuVistaResult = ListarVistasDeMenu(0);
            result.Menu.Vistas = listarPerfileMenuVistaResult.VistaMenus;

            return result;

        }

        public ListarVistaResult ListarVistas(ListarVistaParameter parameter)
        {
            var result = new ListarVistaResult();

            using (var cnn = new SqlConnection(strConn))
            {
                string spName = "[SEGURIDAD].[TM_PDWAC_SP_VISTA_LISTAR]";

                var queryParameters = new DynamicParameters();

                queryParameters.Add("@IdVistaPadre", parameter.IdVistaPadre, DbType.Int32);
                queryParameters.Add("@Activo", parameter.Estado, DbType.Int32);
                queryParameters.Add("@Nombre", parameter.Nombre, DbType.String);
                queryParameters.Add("@Area", parameter.Area, DbType.String);
                queryParameters.Add("@Controller", parameter.Controller, DbType.String);
                queryParameters.Add("@Action", parameter.Action, DbType.String);

         
                result.Vistas = cnn.Query<Vista>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
            }

            return result;

        }
        private ListarPerfileMenuVistaResult ObtenerMenuVista(int IdUsuario, int IdEntidad)
        {
            var result = new ListarPerfileMenuVistaResult();
            int Modo = 5; // Obtener Menus por Perfil

            using (var cnn = new SqlConnection(strConn))
            {
                string spName = "[SEGURIDAD].[TM_PDWAC_SP_MENU_FILTRAR]";

                var queryParameters = new DynamicParameters();
                queryParameters.Add("@Modo", Modo, DbType.Int32);
                queryParameters.Add("@IdUsuario", IdUsuario, DbType.Int32);
                queryParameters.Add("@IdEntidad", IdEntidad, DbType.Int32);

                using (var resultCx = cnn.QueryMultiple(spName, queryParameters, commandType: CommandType.StoredProcedure))
                {
                    result.Menus = resultCx.Read<MenuLogin>().ToList();
                    result.VistaMenus = resultCx.Read<VistaMenu>().ToList();
                }


            }

            return result;
        }


        private ListarPerfileMenuVistaResult ListarVistasDeMenu(int IdMenu)
        {
            var result = new ListarPerfileMenuVistaResult();
            int Modo = 7; //

            using (var cnn = new SqlConnection(strConn))
            {
                string spName = "[SEGURIDAD].[TM_PDWAC_SP_MENU_FILTRAR]";

                var queryParameters = new DynamicParameters();
                queryParameters.Add("@Modo", Modo, DbType.Int32);
                queryParameters.Add("@IdMenu", IdMenu, DbType.Int32);
                

                using (var resultCx = cnn.QueryMultiple(spName, queryParameters, commandType: CommandType.StoredProcedure))
                {
                    result.VistaMenus = resultCx.Read<VistaMenu>().ToList();
                }


            }

            return result;
        }

        private ListarAccesosResult ObtenerAcceso(int IdUsuario)
        {
            var result = new ListarAccesosResult();
            int Modo = 6; // Obtener accesos vistas todo y por usuario

            using (var cnn = new SqlConnection(strConn))
            {
                string spName = "[SEGURIDAD].[TM_PDWAC_SP_MENU_FILTRAR]";

                var queryParameters = new DynamicParameters();
                queryParameters.Add("@Modo", Modo, DbType.Int32);
                queryParameters.Add("@IdUsuario", IdUsuario, DbType.Int32);
                queryParameters.Add("@IdEntidad", 0, DbType.Int32);

                using (var resultCx = cnn.QueryMultiple(spName, queryParameters, commandType: CommandType.StoredProcedure))
                {
                    result.Accesos = resultCx.Read<ItemAcceso>().ToList();
                }
            }
            return result;
        }
        public List<PerfilLogin> ObtenerPerfilesLogin(int IdUsuario, int IdPerfil)
        {
            var result = new List<PerfilLogin>();
            int Modo = 3; // Obtener Perfiles Login
          
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_PERFIL_FILTRAR]";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@Modo", Modo, DbType.Int32);
                    queryParameters.Add("@IdUsuario", IdUsuario, DbType.Int32);
                    queryParameters.Add("@IdPerfil", IdPerfil, DbType.Int32);
                    result = cnn.Query<PerfilLogin>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
                }
           

            return result;
        }

        public CambiarContrasenaResult ActualizarContrasenia(CambiarContrasenaParameter parameter)
        {
            CambiarContrasenaResult cambiarContrasenaResult = new CambiarContrasenaResult();

          
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
            

            return cambiarContrasenaResult;
        }

        public ListarPerfilesResult ObtenerPerfiles(string _nombre, int _activo, string tipo)
        {
            var result = new ListarPerfilesResult();
            int Modo = 0; // Listar Todos los Perfiles
          
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_PERFIL_FILTRAR]";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@Modo", Modo, DbType.Int32);
                    queryParameters.Add("@IdPerfil", 0, DbType.Int32);
                    queryParameters.Add("@Nombre", _nombre, DbType.String);
                    queryParameters.Add("@IdEntidad", 0, DbType.Int32);
                    queryParameters.Add("@IsActivo", _activo, DbType.Int32);
                    queryParameters.Add("@Tipo", tipo, DbType.String);
                    result.Perfiles = cnn.Query<Perfil>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();

                    if (result == null)
                    {
                        result.STR_MENSAJE_BD = "Estimado usuario, ocurrio un error interno al recuperar la información.";
                        result.IN_CODIGO_RESULTADO = -1;
                    }
                    else
                        result.IN_CODIGO_RESULTADO = 0;
                }
          
            return result;
        }

        public ListarPerfilesActivosResult ObtenerPerfilesActivos(ListarPerfilesActivosParameter parameter)
        {
            var result = new ListarPerfilesActivosResult();
            
           
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_PERFIL_ACTIVOS]";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@Tipo", parameter.Tipo, DbType.String);
                    result.Perfiles = cnn.Query<Perfil>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
                }
         

            return result;
        }

        private  Perfil ObtenerMenusPorPerfil(int IdPerfil)
        {
            var result = new Perfil();
            result.Menus = new List<MenuPerfil>();
            result.VistaMenu= new List<VistaMenu>();
         
            int Modo = 3; // Obtener Menus por Perfil
           
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_MENU_FILTRAR]";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@Modo", Modo, DbType.Int32);
                    queryParameters.Add("@IdPerfil", IdPerfil, DbType.Int32);
                    

                using (var resultCx = cnn.QueryMultiple(spName, queryParameters, commandType: CommandType.StoredProcedure))
                {
                    result.Menus = resultCx.Read<MenuPerfil>().ToList();
                    result.VistaMenu = resultCx.Read<VistaMenu>().ToList();
                }



            }
           

            return result;
        }

        private Perfil ObtenerMenusPorPerfilUsuario(int IdPerfil, int IdUsuario)
        {
            var result = new Perfil();
            result.Menus = new List<MenuPerfil>();
            result.VistaMenu = new List<VistaMenu>();

            int Modo = 2; // Obtener Menus por Perfil

            using (var cnn = new SqlConnection(strConn))
            {
                string spName = "[SEGURIDAD].[TM_PDWAC_SP_MENU_FILTRAR]";

                var queryParameters = new DynamicParameters();
                queryParameters.Add("@Modo", Modo, DbType.Int32);
                queryParameters.Add("@IdPerfil", IdPerfil, DbType.Int32);
                queryParameters.Add("@IdUsuario", IdUsuario, DbType.Int32);


                using (var resultCx = cnn.QueryMultiple(spName, queryParameters, commandType: CommandType.StoredProcedure))
                {
                    result.Menus = resultCx.Read<MenuPerfil>().ToList();
                    result.VistaMenu = resultCx.Read<VistaMenu>().ToList();
                }



            }


            return result;
        }

        public ObtenerPerfilResult ObtenerPerfil(int IdPerfil)
        {
            var result = new ObtenerPerfilResult();
            int Modo = 1; // Obtener Perfiles por IdPerfil

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
                        result.Perfil.Menus = resultMenus.Menus;
                        result.Perfil.VistaMenu = resultMenus.VistaMenu; ;
                        result.IN_CODIGO_RESULTADO = 0;
                    }

                }
            }

            return result;
        }

        public ObtenerPerfilResult ObtenerPerfilPorUsuario(int IdPerfil, int IdUsuario)
        {
            var result = new ObtenerPerfilResult();
            int Modo = 1; 

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
                    var resultMenus = ObtenerMenusPorPerfilUsuario(result.Perfil.IdPerfil, IdUsuario);

                    if (resultMenus != null)
                    {
                        result.Perfil.Menus = resultMenus.Menus;
                        result.Perfil.VistaMenu = resultMenus.VistaMenu; ;
                        result.IN_CODIGO_RESULTADO = 0;
                    }
                }
            }

            return result;
        }

        public ListarPerfilesResult ObtenerPerfilesPorEntidad(int IdEntidad)
        {
            var result = new ListarPerfilesResult();
            int Modo = 2; // Obtener Perfiles por IdEntidad
           
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_PERFIL_FILTRAR]";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@Modo", Modo, DbType.Int32);
                    queryParameters.Add("@IdPerfil", 0, DbType.Int32);
                    queryParameters.Add("@Nombre", string.Empty, DbType.String);
                    queryParameters.Add("@Tipo", string.Empty, DbType.String);
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
       

            return result;
        }

        public ListarMenusPerfilResult ObtenerMenus()
        {
            var result = new ListarMenusPerfilResult();
            int Modo = 0; // Obtener Todos los Menus

            using (var cnn = new SqlConnection(strConn))
            {
                string spName = "[SEGURIDAD].[TM_PDWAC_SP_MENU_FILTRAR]";

                var queryParameters = new DynamicParameters();
                queryParameters.Add("@Modo", Modo, DbType.Int32);


                using (var resultCx = cnn.QueryMultiple(spName, queryParameters, commandType: CommandType.StoredProcedure))
                {
                    result.Menus = resultCx.Read<MenuPerfil>().ToList();
                    result.VistaMenu = resultCx.Read<VistaMenu>().ToList();
                }

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


            return result;
        }

        public PerfilResult CrearPerfil(PerfilParameter parameter)
        {
            var result = new PerfilResult();
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

            foreach (var itemVistas in parameter.VistasMenu)
            {
                DataRow drog = DtListaVistas.NewRow();
                drog["MENU_ID"] = itemVistas.IdMenu;
                drog["PERFIL_ID"] = itemVistas.IdPerfil;
                drog["VISTA_ID"] = itemVistas.IdVista;
                DtListaVistas.Rows.Add(drog);
            }

            using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_PERFIL_CREAR]";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@Nombre", parameter.Nombre);
                    queryParameters.Add("@Tipo", parameter.Tipo);
                    queryParameters.Add("@Dashboard", parameter.Dashboard);
                    queryParameters.Add("@IdUsuarioCrea", parameter.IdUsuarioCrea);
                    queryParameters.Add("@ListaMenus", DtListaMenus, DbType.Object);
                    queryParameters.Add("@ListaVistas", DtListaVistas, DbType.Object);
                    result = cnn.Query<PerfilResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
      

            return result;
        }

        public PerfilResult EditarPerfil(PerfilParameter parameter)
        {
            var result = new PerfilResult();

         
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

            foreach (var itemVistas in parameter.VistasMenu)
            {
                DataRow drog = DtListaVistas.NewRow();
                drog["MENU_ID"] = itemVistas.IdMenu;
                drog["PERFIL_ID"] = itemVistas.IdPerfil;
                drog["VISTA_ID"] = itemVistas.IdVista;
                DtListaVistas.Rows.Add(drog);
            }

            using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_PERFIL_EDITAR]";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@IdPerfil", parameter.IdPerfil);
                    queryParameters.Add("@Nombre", parameter.Nombre);
                    queryParameters.Add("@Activo", parameter.Activo);
                    queryParameters.Add("@Tipo", parameter.Tipo,DbType.String);
                    queryParameters.Add("@Dashboard", parameter.Dashboard, DbType.String);
                    queryParameters.Add("@IdUsuarioModifica", parameter.IdUsuarioModifica, DbType.Int32);
                    queryParameters.Add("@ListaMenus", DtListaMenus, DbType.Object);
                    queryParameters.Add("@ListaVistas", DtListaVistas, DbType.Object);

                cnn.Query(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    result.IN_CODIGO_RESULTADO = 0;
                }
          
            return result;
        }

        public PerfilResult EliminarPerfil(int IdPerfil)
        {
            var result = new PerfilResult();

           
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_PERFIL_ELIMINAR]";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@IdPerfil", IdPerfil);
                    result = cnn.Query<PerfilResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
         
            return result;
        }
        public PerfilResult VerificarAccesoPerfil(int IdPerfil)
        {
            var result = new PerfilResult();

           
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_PERFIL_VERIFICAR]";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@IdPerfil", IdPerfil);
                    result = cnn.Query<PerfilResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
         

            return result;
        }

        public ListarTransGroupEmpresaResult ObtenerTransGroupEmpresa()
        {
            var result = new ListarTransGroupEmpresaResult();
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
          

            return result;
        }

        public List<string> ObtenerDocumentosPermitidosRevisar(int idperfil)
        {
            var result = new List<string>();

           
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[SEGURIDAD].[TM_PDWAC_SP_DOCUMENTOS_PERFIL]";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@ID_PERFIL", idperfil, DbType.Int32);
                    result = cnn.Query<string>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
                }
        

            return result;
        }
    }
}
