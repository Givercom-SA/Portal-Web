using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Security.Common;
using Service.Common.Logging.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransMares.Core;
using Utilitario.Constante;
using ViewModel.Datos.Acceso;
using ViewModel.Datos.Documento;
using ViewModel.Datos.Message;
using ViewModel.Datos.Parametros;
using ViewModel.Datos.Perfil;
using ViewModel.Datos.SolictudAcceso;
using ViewModel.Datos.UsuarioRegistro;
using Web.Principal.Areas.GestionarUsuarios.Models;
using Web.Principal.Model;
using Web.Principal.ServiceConsumer;
using Web.Principal.ServiceExterno;
using Web.Principal.Util;

namespace Web.Principal.Areas.GestionarUsuarios.Controllers
{
    [Area("GestionarUsuarios")]
    public class UsuarioController : BaseController
    {
        private readonly ServicioAcceso _serviceAcceso;
        private readonly ServicioUsuario _serviceUsuario;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ServicioMaestro _serviceMaestro;
        private readonly ServicioMessage _servicioMessage;
        private readonly ServicioSolicitud _serviceSolicitud;
        private readonly ServicioEmbarques _serviceEmbarqueExterno;
        
        private static ILogger _logger = ApplicationLogging.CreateLogger("UsuarioController");

        public UsuarioController(
            ServicioAcceso serviceAcceso,
            ServicioUsuario serviceUsuario,
            IMapper mapper,
            IConfiguration configuration,
            ServicioMaestro serviceMaestro,
             ServicioMessage servicioMessage,
             ServicioSolicitud serviceSolicitud,
             ServicioEmbarques serviceEmbarqueExterno)
        {
            _serviceAcceso = serviceAcceso;
            _serviceUsuario = serviceUsuario;
            _mapper = mapper;
            _configuration = configuration;
            _serviceMaestro = serviceMaestro;
            _servicioMessage = servicioMessage;
            _serviceSolicitud = serviceSolicitud;
            _serviceEmbarqueExterno = serviceEmbarqueExterno;
        }

        [HttpGet]
        public async Task<IActionResult> CrearUsuario()
        {
            PerfilParameterVM parameter = new PerfilParameterVM();
            parameter.Activo = 1;

            var result = await _serviceAcceso.ObtenerPerfiles(parameter);

            ViewBag.Perfiles = result.Perfiles.Where(x => x.Tipo.Equals(Utilitario.Constante.SeguridadConstante.TipPerfil.INTERNO));



            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditarUsuario(string parkey)
        {
            var dataDesencriptada = Encriptador.Instance.DesencriptarTexto(parkey);
            Int32 Id = Convert.ToInt32(dataDesencriptada);

            CrearUsuarioSecundarioParameterVM parameter = new CrearUsuarioSecundarioParameterVM();
            parameter.IdUsuario = Id;
            var result = await _serviceUsuario.ObtenerUsuarioSecundario(parameter);

            PerfilParameterVM parameterPerfil = new PerfilParameterVM();
            parameterPerfil.Activo = 1;

            var resultPerfiles = await _serviceAcceso.ObtenerPerfiles(parameterPerfil);
            ViewBag.Perfiles = resultPerfiles.Perfiles;

            if (result.usuario.IdEntidad == 0)
            {
                ViewBag.Perfiles = resultPerfiles.Perfiles.Where(x => x.Tipo.Equals(Utilitario.Constante.SeguridadConstante.TipPerfil.INTERNO));
            }
            else {
                ViewBag.Perfiles = resultPerfiles.Perfiles.Where(x => x.Tipo.Equals(Utilitario.Constante.SeguridadConstante.TipPerfil.EXTERNO));
            }



            PerfilParameterVM parameterPerfilUsuario = new PerfilParameterVM();
            parameterPerfilUsuario.IdPerfil = result.usuario.IdPerfil;
            parameterPerfilUsuario.IdUsuario = result.usuario.IdUsuario;
            var resultPerfil = await _serviceAcceso.ObtenerPerfilUsuario(parameterPerfilUsuario);

            resultPerfil.perfil.IdPerfil = result.usuario.IdPerfil;
            resultPerfil.perfil.Menus.ForEach(x => {
                x.VistaMenu = resultPerfil.perfil.VistaMenu.Where(z => z.IdMenu == x.IdMenu).ToArray();

            });


            EditarUsuarioInternoModel model = new EditarUsuarioInternoModel();
            model.Correo = result.usuario.Correo;
            model.Nombres = result.usuario.Nombres;
            model.ApellidoMaterno = result.usuario.ApellidoMaterno;
            model.ApellidoPaterno = result.usuario.ApellidoPaterno;
            model.Activo = result.usuario.Activo;
            model.EsAdmin = result.usuario.EsAdmin;
            model.Perfil = result.usuario.IdPerfil;
            model.Items = result.usuario.Menus;
            model.Menus = resultPerfil.perfil.Menus;
            model.IdEntidad = result.usuario.IdEntidad;
            model.IdUsuario = result.usuario.IdUsuario;

            ViewBag.IdUsuario = Id;
            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> VerUsuario(string parkey)
        {
            var dataDesencriptada = Encriptador.Instance.DesencriptarTexto(parkey);
            Int32 Id = Convert.ToInt32(dataDesencriptada);

            CrearUsuarioSecundarioParameterVM parameter = new CrearUsuarioSecundarioParameterVM();
            parameter.IdUsuario = Id;
            var result = await _serviceUsuario.ObtenerUsuario(parameter);

            PerfilParameterVM parameterPerfil = new PerfilParameterVM();
            parameterPerfil.Activo = 1;

            var resultPerfiles = await _serviceAcceso.ObtenerPerfiles(parameterPerfil);
            ViewBag.Perfiles = resultPerfiles.Perfiles;

            if (result.usuario.IdEntidad == 0)
            {
                ViewBag.Perfiles = resultPerfiles.Perfiles.Where(x => x.Tipo.Equals(Utilitario.Constante.SeguridadConstante.TipPerfil.INTERNO));
            }
            else
            {
                ViewBag.Perfiles = resultPerfiles.Perfiles.Where(x => x.Tipo.Equals(Utilitario.Constante.SeguridadConstante.TipPerfil.EXTERNO));
            }

            PerfilParameterVM parameterPerfilUsuario = new PerfilParameterVM();
            parameterPerfilUsuario.IdPerfil = result.usuario.IdPerfil;
            parameterPerfilUsuario.IdUsuario = result.usuario.IdUsuario;
            var resultPerfil = await _serviceAcceso.ObtenerPerfilUsuario(parameterPerfilUsuario);

            resultPerfil.perfil.IdPerfil = result.usuario.IdPerfil;
            resultPerfil.perfil.Menus.ForEach(x => {
                x.VistaMenu = resultPerfil.perfil.VistaMenu.Where(z => z.IdMenu == x.IdMenu).ToArray();

            });



            EditarUsuarioInternoModel model = new EditarUsuarioInternoModel();
            model.Correo = result.usuario.Correo;
            model.Nombres = result.usuario.Nombres;
            model.ApellidoMaterno = result.usuario.ApellidoMaterno;
            model.ApellidoPaterno = result.usuario.ApellidoPaterno;
            model.Activo = result.usuario.Activo;
            model.EsAdmin = result.usuario.EsAdmin;
            model.Perfil = result.usuario.IdPerfil;
            model.Items = result.usuario.Menus;
            model.IdEntidad = result.usuario.IdEntidad;
            model.IdUsuario = result.usuario.IdUsuario;
            model.Menus = resultPerfil.perfil.Menus;
            model.UsuarioModifica = result.usuario.UsuarioModifica;
            model.UsuarioCrea = result.usuario.UsuarioCrea;
            model.FechaCrea = result.usuario.FechaRegistro;
            model.FechaModifica = result.usuario.FechaModificacion;
            model.CambioContrasenia = result.usuario.CambioContrasenia;
            model.ConfirmarCuenta = result.usuario.CorreoConfirmado;
            model.PerfilNombre = resultPerfiles.Perfiles.Where(x=>x.IdPerfil== result.usuario.IdPerfil).First().Nombre;


            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> ListarUsuarios(string parkey)
        {
            ListarUsuariosModel model = new ListarUsuariosModel();

            try
            {

                if (parkey != null)
                {
                    var dataDesencriptada = Encriptador.Instance.DesencriptarTexto(parkey);

                    string[] parametros = dataDesencriptada.Split('|');

                    if (parametros.Count() > 1)
                    {
                        string Correo = parametros[0];
                        string Nombres = parametros[1];
                        string isActivo = parametros[2];
                        string IdPerfil = parametros[3];
                 
                        model.Correo = Correo ;
                        model.Nombres = Nombres;
                        model.isActivo = isActivo == "" ? 0 : Convert.ToInt32(isActivo);
                        model.IdPerfil = IdPerfil == "" ? 0 : Convert.ToInt32(IdPerfil) ;
                    

                    }
                }

                ListarUsuarioParameterVM listarUsuarioParameterVM = new ListarUsuarioParameterVM();
                listarUsuarioParameterVM.Nombres = model.Nombres;
                listarUsuarioParameterVM.Correo = model.Correo;
                listarUsuarioParameterVM.IdPerdil = model.IdPerfil;
                listarUsuarioParameterVM.isActivo = model.isActivo;
                listarUsuarioParameterVM.RegistroInicio = 1;
                listarUsuarioParameterVM.RegistroFin = 100;

                var result = await _serviceUsuario.ObtenerListadoUsuarios(listarUsuarioParameterVM);
                await cargarListas(Utilitario.Constante.EmbarqueConstante.TipoPerfil.INTERNO);
                model.ListUsuarios = result;
                model.ListEstado = await ListarEstados();

            }
            catch (Exception err)
            {
                _logger.LogError(err, "ListarUsuarios");
            }
            return View(model);
        }


        [HttpPost]
        public async Task<JsonResult> ListarEncriptar(ListarUsuariosModel model)
        {
            ActionResponse = new ActionResponse();

            try
            {

                string url = $"{model.Correo}|{model.Nombres}|{model.isActivo}|{model.IdPerfil}";
                

                string urlEncriptado = this.GetUriHost() + Url.Action("ListarUsuarios", "Usuario", new { area = "GestionarUsuarios" }) + "?parkey=" + Encriptador.Instance.EncriptarTexto(url);

                ActionResponse.Codigo = 0;
                ActionResponse.Mensaje = urlEncriptado;



            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "BuscarEncriptar");
                ActionResponse.Codigo = -100;
                ActionResponse.Mensaje = "Error inesperado, por favor volver a intentar mas tarde.";
            }
            return Json(ActionResponse);
        }


        public async Task<SelectList> ListarEstados() {

            var listServiceEstado = await _serviceMaestro.ObtenerParametroPorIdPadre(76);

           var result = new SelectList(listServiceEstado.ListaParametros, "ValorCodigo", "NombreDescripcion");

            return result;
        }

        
        private async Task  cargarListas(string tipo) 
        {
            ListarPerfilActivosParameterVM parameter = new ListarPerfilActivosParameterVM();
            parameter.Tipo = tipo;
            var resultPerfiles = await _serviceAcceso.ObtenerPerfilesActivos(parameter);

            ViewBag.ListaPerfilActivos = resultPerfiles;
        }

        [HttpPost]
        public async Task<JsonResult> CrearUsuario(CrearUsuarioIntenoModel usuario)
        {
            ActionResponse = new ActionResponse();
            if (ModelState.IsValid)
            {
                if (usuario.Perfiles.Any() && usuario.Perfiles.Single().Menus.Where(x=>x.IdMenuChecked !=null).Any())
                {
                    var UrlInfo = Url.ActionContext.HttpContext.Request;
                    CrearUsuarioSecundarioParameterVM parameterVM = new CrearUsuarioSecundarioParameterVM();
                    parameterVM.IdEntidad = 0;
                    parameterVM.IdPerfil = Convert.ToInt32(usuario.Perfil);
                    parameterVM.Correo = usuario.Correo;
                    parameterVM.Nombres = usuario.Nombres;
                    parameterVM.ApellidoMaterno = usuario.ApellidoMaterno;
                    parameterVM.ApellidoPaterno = usuario.ApellidoPaterno;
                    parameterVM.EsAdmin = false;
                    parameterVM.Activo = true;
                    parameterVM.IdUsuarioCrea = Convert.ToInt32(ViewData["IdUsuario"]);
                    parameterVM.PerfilesMenus = usuario.Perfiles;
                    parameterVM.Contrasenia = new Utilitario.Seguridad.Encrypt().GetSHA256(usuario.Contrasenia);
                    parameterVM.ContraseniaNoCifrado = usuario.Contrasenia;
                    parameterVM.RequiereConfirmacion = true;
                    parameterVM.UrlConfirmacion = string.Format("{0}/{1}", this.GetUriHost(), "Account/ConfirmarCorreo");
                    parameterVM.ImagenGrupoTrans = $"{this.GetUriHost()}/{_configuration[Utilitario.Constante.ConfiguracionConstante.Imagen.ImagenGrupo]}"; ;
                    
                    var result = await _serviceUsuario.CrearUsuario(parameterVM);
                    if (result.CodigoResultado == 0)
                    {
                        ActionResponse.Codigo = 0;
                        ActionResponse.Mensaje = "El usuario ha sido creado correctamente y se envió sus datos de acceso al correo.";
                    }
                    else
                    {
                        ActionResponse.Codigo = -1;
                        ActionResponse.Mensaje = "Error, no se pudo crear al usuario.";
                    }
                }
                else
                {
                    ActionResponse.Codigo = -1;
                    ActionResponse.Mensaje = "Debe Seleccionar accesos para el usuario";
                }
            }
            else
            {
                ActionResponse.Codigo = -1;
                ActionResponse.Mensaje = "Error, en la validación de los datos.";
            }

            return Json(ActionResponse);

        }

        [HttpGet]
        public async Task<IActionResult> CuentaUsuario() {

            CuentaUsuarioVM model = new CuentaUsuarioVM();
            usuario = HttpContext.Session.GetUserContent();
            model.Usuario = usuario;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CuentaEntidad()
        {

            ClienteDetalleModel model = new ClienteDetalleModel();
            model.Solicitud = new ViewModel.Datos.Solicitud.SolicitudVM();
            var resultSesion = HttpContext.Session.GetUserContent();
            var resultEntidad = await _serviceUsuario.LeerCliente(resultSesion.IdEntidad);
            model.Entidad = resultEntidad.Cliente;
            model.Solicitud = await _serviceSolicitud.leerSolicitud(model.Entidad.IdSolicitud);
            model.CodigoSolicitud = model.Solicitud.CodigoSolicitud;

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> SolicitudAcceso()
        {

            ClienteDetalleModel model = new ClienteDetalleModel();
            model.Solicitud = new ViewModel.Datos.Solicitud.SolicitudVM();
            var resultSesion = HttpContext.Session.GetUserContent();
            var resultEntidad = await _serviceUsuario.LeerCliente(resultSesion.IdEntidad);
            model.Entidad = resultEntidad.Cliente;
            model.Solicitud = await _serviceSolicitud.leerSolicitud(model.Entidad.IdSolicitud);
            model.CodigoSolicitud = model.Solicitud.CodigoSolicitud;

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> SolicitarAccesoActualizar()
        {
            var resultSesion = HttpContext.Session.GetUserContent();
            var resultEntidad = await _serviceUsuario.LeerCliente(resultSesion.IdEntidad);
          
            SolicitarAccesoModel model = new SolicitarAccesoModel();
            var listEntidades = await _serviceMaestro.ObtenerParametroPorIdPadre(1);

            if (listEntidades.CodigoResultado == 0)
            {
                var resultTipoentidad = listEntidades.ListaParametros;
                model.ListTipoEntidad2 = new ListTipoEntidadModel();
                model.ListTipoEntidad2.TiposEntidad = new List<TipoEntidad>();

                List<string> listTIpoPerfil = new List<string>();
                if(resultEntidad.Cliente.AgenteAduana!=null)
                    listTIpoPerfil.Add(resultEntidad.Cliente.AgenteAduana);

                if (resultEntidad.Cliente.ClienteForwarder != null)
                    listTIpoPerfil.Add(resultEntidad.Cliente.ClienteForwarder);

                if (resultEntidad.Cliente.ClienteFinal != null)
                    listTIpoPerfil.Add(resultEntidad.Cliente.ClienteFinal);

                if (resultEntidad.Cliente.ClienteBroker != null)
                    listTIpoPerfil.Add(resultEntidad.Cliente.ClienteBroker);

                foreach (ParametrosVM item in resultTipoentidad)
                {
                    var result = listTIpoPerfil.Where(x => x.Equals(item.ValorCodigo)).FirstOrDefault();
                    if (result == null) result = "";

                    if (result.Trim() == "" )
                    {
                        model.ListTipoEntidad2.TiposEntidad.Add(new TipoEntidad()
                        {
                            Check = false,
                            CodTipoEntidad = item.ValorCodigo,
                            IdParametro = item.IdParametro,
                            NombreTipoEntidad = item.NombreDescripcion
                        });
                    }
                }
            }
            else
                model.MensajeError = model.MensajeError + " " + listEntidades.MensajeResultado;

            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> SolicitarAccesoActualizar(SolicitarAccesoModel Input) {

            var resultSesion = HttpContext.Session.GetUserContent();

            bool blDocumentosValido = true;
            ActionResponse ActionResponse = new ActionResponse();
            ActionResponse.ListActionListResponse = new List<ActionErrorResponse>();

            ListDocumentoTipoEntidadParameterVM listarDocumentoTipoEntidadVM = new ListDocumentoTipoEntidadParameterVM();
            listarDocumentoTipoEntidadVM.BrindaCargaFCL = Input.seBrindaOperacionesCargaFCL;
            listarDocumentoTipoEntidadVM.AcuerdoSeguridadCadenaSuministro = Input.acuerdoSeguridadCadenaSuministra;
            listarDocumentoTipoEntidadVM.SeBrindaAgenciamientodeAduanas = Input.seBrinaAgenciaAdeuanas;
            listarDocumentoTipoEntidadVM.TiposEntidad = new List<ViewModel.Datos.Entidad.TipoEntidadVM>();

            foreach (var item in Input.ListTipoEntidad2.TiposEntidad.Where(x => x.Check == true))
            {
                listarDocumentoTipoEntidadVM.TiposEntidad.Add(new ViewModel.Datos.Entidad.TipoEntidadVM() { CodTipoEntidad = item.CodTipoEntidad });
            }

            var listDocumentosSeleccionados = await _serviceMaestro.ObtenerDocumentoPorTipoEntidad(listarDocumentoTipoEntidadVM);

            var listVerificar = Input.ListTipoEntidad2.TiposEntidad.Where(x => x.Check == true).ToList();

            if (listVerificar == null || listVerificar.Count() <= 0)
            {
                ActionResponse.ListActionListResponse.Add(new ActionErrorResponse() { Mensaje = "Debe seleccionar al menos un tipo de entidad", NombreCampo = "Input.TipoEntidad2" });
            }

            if (ModelState.IsValid && (blDocumentosValido) && (listVerificar.Count() > 0))
            {
                var resultValidarentidad = await EntidadPermitoRegistrar(Input);
                if (resultValidarentidad.Respuesta == false)
                {
                    ActionResponse.Codigo = -3;
                    ActionResponse.Mensaje = "Estimado usuario, no esta registrado en nuestra base de Transmares como:";
                    ActionResponse.Mensaje += "<br/>";
                    ActionResponse.Mensaje += string.Join(" ", resultValidarentidad.EntidadRespuestas.Select(e => e.Mensaje));
                }
                else
                {
                    string document = resultSesion.TipoDocumento;
                    SolicitarAccesoParameterVM solicitarAccesoVM = new SolicitarAccesoParameterVM();
                    solicitarAccesoVM.TipoDocumento = resultSesion.TipoDocumento;
                    solicitarAccesoVM.NumeroDocumento = resultSesion.NumeroDocumento;
                    solicitarAccesoVM.RazonSocial = resultSesion.RazonSocial;
                    solicitarAccesoVM.RepresentaLegalNombre = resultSesion.NombresUsuario;
                    solicitarAccesoVM.RepresentaLegalApellidoPaterno = resultSesion.ApellidoPaternousuario;
                    solicitarAccesoVM.RepresentaLegalMaterno = resultSesion.ApellidoMaternoUsuario;
                    solicitarAccesoVM.Correo = resultSesion.CorreoUsuario;

                    solicitarAccesoVM.AcuerdoEndoceElectronico = Input.acuerdoCorrectoUsoEndosesElectronico;
                    solicitarAccesoVM.BrindaOpeCargaFCL = Input.seBrindaOperacionesCargaFCL;
                    solicitarAccesoVM.AcuerdoSeguroCadenaSuministro = Input.acuerdoSeguridadCadenaSuministra;
                    solicitarAccesoVM.DeclaracionJuradaVeracidadInfo = Input.declaracionJUridaVeracidadInformacio;
                    solicitarAccesoVM.BrindaAgenciamientoAduanas = Input.seBrinaAgenciaAdeuanas;

                    if (Input.ListTipoEntidad2.TiposEntidad.Exists(x => x.CodTipoEntidad.Equals(EmbarqueConstante.TipoEntidad.AGENTE_ADUANAS)))
                    {
                        solicitarAccesoVM.CodigoSunat = Input.CodigoSunat;
                        solicitarAccesoVM.ProcesoFacturacion = Input.HabiliarProcesoFacturacion;
                        solicitarAccesoVM.TerminoCondicionGeneralContraTCGC = Input.TerminoCondicionesGeneralesContraTCGC;
                    }
                    else
                    {
                        solicitarAccesoVM.CodigoSunat = null;
                        solicitarAccesoVM.ProcesoFacturacion = null;
                        solicitarAccesoVM.TerminoCondicionGeneralContraTCGC = null;
                    }



                    solicitarAccesoVM.Documento = new List<ViewModel.Datos.SolictudAcceso.DocumentoVM>();

                    foreach (DocumentoTipoEntidadVM item in listDocumentosSeleccionados.listarDocumentosTipoEntidad)
                    {
                        solicitarAccesoVM.Documento.Add(new DocumentoVM() { CodigoDocumento = item.CodigoDocumento });
                    }

                    solicitarAccesoVM.TipoEntidad = new List<ViewModel.Datos.SolictudAcceso.TipoEntidadVM>();

                    foreach (TipoEntidad item in Input.ListTipoEntidad2.TiposEntidad.Where(x => x.Check == true))
                    {
                        solicitarAccesoVM.TipoEntidad.Add(new TipoEntidadVM() { CodigoTipoEntidad = item.CodTipoEntidad });
                    }


                    var jsonSolicitarAcceso = JsonConvert.SerializeObject(solicitarAccesoVM);

                    CodigoGeneradoValidacionParameterVM codigoGeneradoValidacionParameterVM = new CodigoGeneradoValidacionParameterVM();
                    codigoGeneradoValidacionParameterVM.CodigoTipoDocumento = solicitarAccesoVM.TipoDocumento;
                    codigoGeneradoValidacionParameterVM.NumeroDocumento = solicitarAccesoVM.NumeroDocumento;
                    codigoGeneradoValidacionParameterVM.Correo = solicitarAccesoVM.Correo;
                    codigoGeneradoValidacionParameterVM.Nombres = solicitarAccesoVM.RepresentaLegalNombre;


                    solicitarAccesoVM.ImagenGrupoTrans = $"{this.GetUriHost()}/{_configuration[Utilitario.Constante.ConfiguracionConstante.Imagen.ImagenGrupo]}";
                    solicitarAccesoVM.TipoRegistro = Utilitario.Constante.SolicitudAccesoConstante.SolicitudAcceso.REGISTRO_SOLICITUD_ACTUALIZADO.ToString();
                    solicitarAccesoVM.IdUsuarioCreaModifica = resultSesion.idUsuario;
                    solicitarAccesoVM.IdEntidad =resultSesion.IdEntidad;
                    solicitarAccesoVM.UrlTransmares = $"{this.GetUriHost()}";
                    var listTipoDocumnentoResult = await _serviceAcceso.SolicitarAcceso(solicitarAccesoVM);

                    if (listTipoDocumnentoResult.CodigoResultado == 0)
                    {

                     

                        ActionResponse.Mensaje = "Estimado cliente, su solicitud fue resgistrado con éxito, por favor volver a iniciar sesión";
                        ActionResponse.Codigo = 0;



                    }
                    else
                    {
                        ActionResponse.Mensaje = "Ocurrio un error interno, intentar más tarde por favor.";
                        ActionResponse.Codigo = -1;
                    }

                }

            }
            else
            {

                var erroresCampos = ModelState.Where(ms => ms.Value.Errors.Any())
                                      .Select(x => new { x.Key, x.Value.Errors });

                foreach (var erroneousField in erroresCampos)
                {
                    var fieldKey = erroneousField.Key;
                    var fieldErrors = string.Join(" | ", erroneousField.Errors.Select(e => e.ErrorMessage));

                    ActionResponse.ListActionListResponse.Add(new ActionErrorResponse()
                    {
                        Mensaje = fieldErrors,
                        NombreCampo = fieldKey
                    }); ;
                }

                ActionResponse.Codigo = -1;
                ActionResponse.Mensaje = "Por favor ingresar los campos requeridos.";

            }



            return new JsonResult(ActionResponse);
        }

        class EntidadTransmaresResponse
        {

            public string Mensaje { get; set; }
            public string TipoEntidad { get; set; }
            public int Respuesta { get; set; }


        }

        class EntidadValidar
        {
            public List<EntidadTransmaresResponse> EntidadRespuestas { get; set; }
            public bool Respuesta { get; set; }
        }

        private async Task<EntidadValidar> EntidadPermitoRegistrar(SolicitarAccesoModel Input)
        {
            var resultSesion = HttpContext.Session.GetUserContent();

            EntidadValidar entidadValidar = new EntidadValidar();
            List<EntidadTransmaresResponse> listResponseService = new List<EntidadTransmaresResponse>();

            bool ExisteAgenteAduanasProcesoFacturacion = false;

            var agenteAduanasSelecciono = Input.ListTipoEntidad2.TiposEntidad.Where(x => x.Check == true && x.CodTipoEntidad.Equals(EmbarqueConstante.TipoEntidad.AGENTE_ADUANAS)).FirstOrDefault();

            if (agenteAduanasSelecciono != null && Input.HabiliarProcesoFacturacion == false)
            {
                ExisteAgenteAduanasProcesoFacturacion = true;
            }

            int resultValidarRegistro = 0;

            if (ExisteAgenteAduanasProcesoFacturacion == false)
            {
                // ini validar registro en transmares



                foreach (var itemTipoEntidad in Input.ListTipoEntidad2.TiposEntidad.Where(x => x.Check == true))
                {

                    EntidadTransmaresResponse entidadTransmaresResponse = new EntidadTransmaresResponse();


                    string codigoTipoEntidad = itemTipoEntidad.CodTipoEntidad;

                    string strTipoEntidadTransmares = "";

                    if (codigoTipoEntidad.Equals(EmbarqueConstante.TipoEntidad.AGENTE_ADUANAS))
                    {
                        strTipoEntidadTransmares = EmbarqueConstante.TipoEntidadTransmares.AGENTE_ADUANAS;
                        entidadTransmaresResponse.TipoEntidad = "Agente de Aduanas";
                    }
                    else if (codigoTipoEntidad.Equals(EmbarqueConstante.TipoEntidad.CLIENTE_FINAL))
                    {
                        strTipoEntidadTransmares = EmbarqueConstante.TipoEntidadTransmares.CLIENTE_FINAL;
                        entidadTransmaresResponse.TipoEntidad = "Cliente Final";
                    }
                    else if (codigoTipoEntidad.Equals(EmbarqueConstante.TipoEntidad.BROKER))
                    {
                        strTipoEntidadTransmares = EmbarqueConstante.TipoEntidadTransmares.BROKET;
                        entidadTransmaresResponse.TipoEntidad = "Broker";
                    }
                    else if (codigoTipoEntidad.Equals(EmbarqueConstante.TipoEntidad.CLIENTE_FORWARDER))
                    {
                        strTipoEntidadTransmares = EmbarqueConstante.TipoEntidadTransmares.CLIENTE_FORWARDER;
                        entidadTransmaresResponse.TipoEntidad = "Forwarder";
                    }


                    int intRespuestaServicioTrans = await _serviceEmbarqueExterno.ValidarRegistroEntidad(strTipoEntidadTransmares, resultSesion.NumeroDocumento, resultSesion.CorreoUsuario);
                    entidadTransmaresResponse.Respuesta = intRespuestaServicioTrans;

                    if (intRespuestaServicioTrans == 0)
                    {
                        entidadTransmaresResponse.Mensaje = @$"<li>{ entidadTransmaresResponse.TipoEntidad}</li>";

                        listResponseService.Add(entidadTransmaresResponse);
                    }

                }

                if (listResponseService.Exists(x => x.Respuesta == 0))
                {
                    resultValidarRegistro = 0;
                }
                else
                {
                    resultValidarRegistro = 1;
                }




                if (resultValidarRegistro == 0)
                {
                    entidadValidar.Respuesta = false;

                }
                else
                {
                    entidadValidar.Respuesta = true;

                }


            }
            else
            {
                entidadValidar.Respuesta = true;

            }

            entidadValidar.EntidadRespuestas = listResponseService;

            return entidadValidar;





        }

        [HttpPost]
        public async Task<JsonResult> ActualizarUsuario(EditarUsuarioInternoModel usuario)
        {
            ActionResponse = new ActionResponse();


            if (ModelState.IsValid)
            {
                if (usuario.Perfiles.Any())
                {
                    CrearUsuarioSecundarioParameterVM parameterVM = new CrearUsuarioSecundarioParameterVM();
                    parameterVM.IdUsuario = usuario.IdUsuario;
                    parameterVM.IdPerfil = usuario.Perfil;
                    parameterVM.Nombres = usuario.Nombres;
                    parameterVM.ApellidoMaterno = usuario.ApellidoMaterno;
                    parameterVM.ApellidoPaterno = usuario.ApellidoPaterno;
                    parameterVM.EsAdmin = usuario.EsAdmin;
                    parameterVM.Activo = usuario.Activo;
                    parameterVM.IdUsuarioModifica = this.usuario.idUsuario;

                    parameterVM.PerfilesMenus = usuario.Perfiles;

                    var result = await _serviceUsuario.EditarUsuarioInterno(parameterVM);
                    ActionResponse.Codigo = result.CodigoResultado;
                    ActionResponse.Mensaje = result.MensajeResultado ;

                }
                else
                {
                    ActionResponse.Codigo = -1;
                    ActionResponse.Mensaje = "Estimado usuario, debe Seleccionar al menos un acceso.";
                }
            }
            else
            {
                ActionResponse.Codigo = -1;
                ActionResponse.Mensaje = "Estimado usuario, ocurrio un error en la validación de los datos.";
            }

            return Json(ActionResponse);

        }

        [HttpPost]
        public async Task<JsonResult> DesactivarUsuario(string IdUsuario)
        {
            ActionResponse actionResponse = new ActionResponse();

            try
            {
                CrearUsuarioSecundarioParameterVM crearUsuarioSecundarioParameter = new CrearUsuarioSecundarioParameterVM();
                crearUsuarioSecundarioParameter.IdUsuario = Convert.ToInt32(IdUsuario);
                crearUsuarioSecundarioParameter.Activo = false;
                var result = await _serviceUsuario.HabilitarUsuario(crearUsuarioSecundarioParameter);

                actionResponse.Codigo = result.CodigoResultado;
                actionResponse.Mensaje = result.MensajeResultado;
            }
            catch (Exception err)
            {
                _logger.LogError(err, "DesactivarUsuario");
                actionResponse.Codigo = -100;
                actionResponse.Mensaje = "Estimado usuario, error inesperado por favor volver a intentar más tarde.";
            }

            return Json(actionResponse);

        }

        [HttpPost]
        public async Task<JsonResult> ActivarUsuario(string IdUsuario)
        {
            ActionResponse actionResponse = new ActionResponse();

            try
            {
                CrearUsuarioSecundarioParameterVM crearUsuarioSecundarioParameter = new CrearUsuarioSecundarioParameterVM();
                crearUsuarioSecundarioParameter.IdUsuario = Convert.ToInt32(IdUsuario);
                crearUsuarioSecundarioParameter.Activo = true;
                var result = await _serviceUsuario.HabilitarUsuario(crearUsuarioSecundarioParameter);

                actionResponse.Codigo = result.CodigoResultado;
                actionResponse.Mensaje = result.MensajeResultado;
            }
            catch (Exception err)
            {
                _logger.LogError(err, "DesactivarUsuario");
                actionResponse.Codigo = -100;
                actionResponse.Mensaje = "Estimado usuario, error inesperado por favor volver a intentar más tarde.";
            }

            return Json(actionResponse);

        }

        [HttpPost]
        public async Task<JsonResult> RestablercContrasenia(string IdUsuario)
        {
            ActionResponse actionResponse = new ActionResponse();

            try
            {
              var resultUsuario=await  _serviceUsuario.ObtenerUsuario(Convert.ToInt32(IdUsuario));

                CrearUsuarioSecundarioParameterVM parameterVM = new CrearUsuarioSecundarioParameterVM();
                parameterVM.IdUsuario = Convert.ToInt32(IdUsuario);
                parameterVM.Correo = resultUsuario.Usuario.Correo;

                string strContrasenia = new Utilitario.Seguridad.SeguridadCodigo().GenerarCadenaLongitud(6);
                strContrasenia = strContrasenia.ToUpper();
                string strNuevaContrasenia = new Utilitario.Seguridad.Encrypt().GetSHA256(strContrasenia);
                parameterVM.Contrasenia = strNuevaContrasenia;
                var result = await _serviceUsuario.CambiarClaveUsuario(parameterVM);

                actionResponse.Codigo = result.CodigoResultado;
                actionResponse.Mensaje = result.MensajeResultado;

                if (actionResponse.Codigo == 0) {
                    enviarCorreo(new FormatoCorreoBody().formatoBodyRestablecerContrasenia(resultUsuario.Usuario.Nombres,
                     strContrasenia,
                     $"{this.GetUriHost()}/{_configuration[Utilitario.Constante.ConfiguracionConstante.Imagen.ImagenGrupo]}"),
                     resultUsuario.Usuario.Correo,
                     "!Transmares Group! Restablecer Contraseña"
                     
                     );
                }

            }
            catch (Exception err)
            {
                _logger.LogError(err, "RestablercContrasenia");
                actionResponse.Codigo = -100;
                actionResponse.Mensaje = "Estimado usuario, error inesperado por favor volver a intentar más tarde.";
            }

            return Json(actionResponse);

        }

        [HttpGet]
        public async Task<JsonResult> ExisteCorreo(string Correo)
        {
            bool CorreoDisponible = true;

            CrearUsuarioSecundarioParameterVM parameter = new CrearUsuarioSecundarioParameterVM();
            parameter.Correo = Correo;
            var result = await _serviceUsuario.ObtenerUsuarioSecundario(parameter);
            //var ExisteUsuario = await _serviceUsuario.ExisteUsuario(Correo);
            //if (ExisteUsuario)
            if (result.usuario != null)
            {
                if (result.usuario.Correo.ToLower().Equals(Correo.ToLower()))
                    CorreoDisponible = false;
            }

            return Json(CorreoDisponible);

        }

        [HttpGet]
        public async Task<IActionResult> MenusPorPerfil(int IdPerfil, int IdUsuario)
        {
       

            PerfilParameterVM parameterPerfilUsuario = new PerfilParameterVM();
            parameterPerfilUsuario.IdPerfil = IdPerfil;
            parameterPerfilUsuario.IdUsuario = IdUsuario;
            var resultPerfil = await _serviceAcceso.ObtenerPerfilUsuario(parameterPerfilUsuario);

            resultPerfil.perfil.IdPerfil = IdPerfil;
            resultPerfil.perfil.Menus.ForEach(x => {
                x.VistaMenu = resultPerfil.perfil.VistaMenu.Where(z => z.IdMenu == x.IdMenu).ToArray();

            });


            return PartialView("_MenusPorPerfil", resultPerfil.perfil.Menus);
        }

        [HttpPost]
        public async Task<JsonResult> CambiarClave(int IdUsuario, string Clave)
        {
            ActionResponse = new ActionResponse();
            if (IdUsuario > 0)
            {
                CrearUsuarioSecundarioParameterVM parameterVM = new CrearUsuarioSecundarioParameterVM();
                parameterVM.IdUsuario = IdUsuario;
                //parameterVM.Correo = Correo;
                parameterVM.Contrasenia = new Utilitario.Seguridad.Encrypt().GetSHA256(Clave);
                var result = await _serviceUsuario.CambiarClaveUsuario(parameterVM);
                ActionResponse.Codigo = 0;
                ActionResponse.Mensaje = "Su clave ha sido cambiada correctamente.";
            }
            else
            {
                ActionResponse.Codigo = -1;
                ActionResponse.Mensaje = "Error no se pudo crear al usuario.";
            }

            return Json(ActionResponse);

        }

        [HttpGet]
        public async Task<IActionResult> CambiarContrasenia(string nuevo)
        {
            CambiarContraseniaModel model = new CambiarContraseniaModel();

            if (!string.IsNullOrEmpty(nuevo))
            {
                ViewBag.mensaje = "";
                ViewBag.codigo = "";
                ViewBag.EsNuevo = nuevo;
                model.Correo =this.usuario.CorreoUsuario;
                model.EsNuevo = nuevo;
            }
            else
            {
                return RedirectToAction("Home", "Inicio", new { area = "GestionarDashboards" });
            }

            return View(model);

        }


        [HttpPost]
        public async Task<IActionResult> CambiarContrasenia(CambiarContraseniaModel model)
        {
            ViewBag.mensaje = "";
            ViewBag.codigo = "";
            ViewBag.EsNuevo = model.EsNuevo;

            if (ModelState.IsValid)
            {
                CambiarContrasenaParameterVM cambiarContrasenaParameterVM = new CambiarContrasenaParameterVM();
                if (usuario.AdminSistema == 1)
                {
                    cambiarContrasenaParameterVM.IdUsuario = Int32.Parse(usuario.IdUsuarioInicioSesion.ToString());
                }
                else
                {
                    cambiarContrasenaParameterVM.IdUsuario = usuario.idUsuario;
                }

                cambiarContrasenaParameterVM.ContrasenaActual = new Utilitario.Seguridad.Encrypt().GetSHA256(model.contraseniaActual);
                cambiarContrasenaParameterVM.ContrasenaNuevo = new Utilitario.Seguridad.Encrypt().GetSHA256(model.contraseniaNueva);

                if (String.IsNullOrEmpty(model.EsNuevo))
                { cambiarContrasenaParameterVM.EsUsuarioNuevo = null; }

                else if (model.EsNuevo.Equals("1"))
                {
                    cambiarContrasenaParameterVM.EsUsuarioNuevo = true;
                }
                else if (model.EsNuevo.Equals("0"))
                {
                    cambiarContrasenaParameterVM.EsUsuarioNuevo = false;
                }

                var cambiarContrasenaResultVM = await _serviceAcceso.ActualizarContrasena(cambiarContrasenaParameterVM);

                ViewBag.mensaje = cambiarContrasenaResultVM.MensajeResultado;
                ViewBag.codigo = cambiarContrasenaResultVM.CodigoResultado;

            }

            return View(model);
        }


        private async void enviarCorreo(string _contenido, string _correo, string _asunto) {

            EnviarMessageCorreoParameterVM enviarMessageCorreoParameterVM = new EnviarMessageCorreoParameterVM();
            enviarMessageCorreoParameterVM.RequestMessage = new RequestMessage();
            enviarMessageCorreoParameterVM.RequestMessage.Contenido = _contenido;
            enviarMessageCorreoParameterVM.RequestMessage.Correo = _correo;
            enviarMessageCorreoParameterVM.RequestMessage.Asunto = _asunto;
            await _servicioMessage.EnviarMensageCorreo(enviarMessageCorreoParameterVM);
        }

    }
}
