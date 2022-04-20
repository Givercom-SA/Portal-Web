using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Principal.ServiceExterno;
using Web.Principal.Utils;
using Web.Principal.Areas.GestionarEmbarques.Models;
using Web.Principal.Model;
using Web.Principal.ServiceConsumer;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using ViewModel.Datos.Embarque.GestionarMemo;
using System.IO;
using static Utilitario.Constante.EmbarqueConstante;
using System.Net;
using TransMares.Core;
using Microsoft.Extensions.Logging;
using Service.Common.Logging.Application;
using ViewModel.Datos.Message;

namespace Web.Principal.Areas.GestionarEmbarques.Controllers
{
    [Area("GestionarEmbarques")]
    public class MemoController : BaseController
    {
        private readonly ServicioEmbarques _serviceEmbarques;
        private readonly ServicioEmbarque _serviceEmbarque;
        private readonly ServicioMaestro _serviceMaestro;
        private readonly ServicioMessage _servicioMessage;
        private readonly IMapper _mapper;
        private static ILogger _logger = ApplicationLogging.CreateLogger("MemoController");

        public MemoController(ServicioEmbarques serviceEmbarques,
                                    ServicioEmbarque serviceEmbarque,
                                    ServicioMaestro serviceMaestro,
                                    IMapper mapper,
                                    ServicioMessage servicioMessage)
        {
            _serviceEmbarques = serviceEmbarques;
            _serviceEmbarque = serviceEmbarque;
            _serviceMaestro = serviceMaestro;
            _mapper = mapper;
            _servicioMessage = servicioMessage;
        }

        public async Task<IActionResult> Solicitud(string codigo, string servicio,string origen)
        {
            var listTipoDoc = await _serviceMaestro.ObtenerParametroPorIdPadre(66);

            ViewBag.ListTipoDoc = new SelectList(listTipoDoc.ListaParametros, "ValorCodigo", "NombreDescripcion");
            ViewBag.KeyBL = codigo;
            ViewBag.Servicio = servicio;
            ViewBag.Origen = origen;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GestionarMemo(string KeyBL, string servicio)
        {
            ActionResponse ActionResponse = new();
            string mensaje = string.Empty;

            try
            {
                var embarque = await _serviceEmbarques.ObtenerEmbarque(KeyBL, servicio);

                var parameterNofificacion = new NotificacionMemoParameterVM()
                {
                    IdUsuario = usuario.idUsuario,
                    KeyBLD = KeyBL,
                    FlagVigente = embarque.FLAG_MEMO_VIGENTE
                };


                var result = await _serviceEmbarque.VerificarNotificacionMemo(parameterNofificacion);

                if (result.IN_CODIGO_RESULTADO == -2 || result.IN_CODIGO_RESULTADO == -3)
                {
                    ActionResponse.Codigo = -1;
                    ActionResponse.Titulo = "Alerta";
                    ActionResponse.Mensaje = result.STR_MENSAJE_BD;

                }

                else if (embarque.FLAG_COBROS_FACTURADOS.Equals("0"))
                {
                    ActionResponse.Codigo = -1;
                    ActionResponse.Titulo = "Alerta";
                    ActionResponse.Mensaje = @"Estimado cliente, favor notar que para la generación del memo es necesario que todos los cobros se encuentren facturados. <br/><br/>
                    De estar pendientes agradeceremos continuar con la generación de su solicitud de facturación. <br/><br/>
                    En caso cuente con la facturación respectiva, favor verificar con el operativo asignado a su embarque la recepción de los sustentos correspondientes.";
                }
                else if (embarque.FLAG_CONFIRMACION_AA.Equals("0"))
                {
                    ActionResponse.Codigo = -1;
                    ActionResponse.Titulo = "Alerta";
                    ActionResponse.Mensaje = "Estimado cliente, favor coordinar con la Agencia de Aduanas dado que aún no ha confirmado la asignación de su embarque.";
                }
                else if (embarque.FLAG_PLAZO_TERMINO_DESCARGA.Equals("0"))
                {
                    DateTime oDate = Convert.ToDateTime(embarque.PLAZO_TERMINO_DESCARGA);
                    ActionResponse.Codigo = -1;
                    ActionResponse.Titulo = "Alerta";
                    ActionResponse.Mensaje = string.Format("Estimado cliente, favor considerar que podrá gestionar el memo a partir del {0}.", oDate.ToString());
                }
                else
                {



                    ActionResponse.Codigo = result.IN_CODIGO_RESULTADO;
                    ActionResponse.Mensaje = result.STR_MENSAJE_BD;

                    if (result.IN_CODIGO_RESULTADO == 0)
                    {
                        if (embarque.FLAG_MEMO_VIGENTE.Equals("1"))
                        {

                            MemoCorreoParameterVM parameter = new MemoCorreoParameterVM();
                            parameter.KeyBLD = KeyBL;
                            parameter.Nombres = usuario.obtenerNombreCompleto();
                            parameter.Correo = usuario.CorreoUsuario;
                            parameter.NombreArchivo = result.NombreArchivo;
                            parameter.NroBL = embarque.NROBL;
                            parameter.LogoEmpresa = $"{this.GetUriHost()}/img/{usuario.Sesion.ImagenTransGroupEmpresaSeleccionado}";

                            //Enviar Correo
                            var resultEnvio = await _serviceEmbarque.EnviarCorreoMemo(parameter);
                            //Ejecutar Servicio
                            var resultService = await _serviceEmbarques.ActualizarMemoEnviadoEmbarque(KeyBL, result.NombreArchivo);

                            // Evaluar


                            if (embarque.FLAG_EXONERADO_COBRO_GARANTIA.Equals("1"))
                            {
                                ActionResponse.Codigo = -1;
                                ActionResponse.Titulo = "Alerta";
                                ActionResponse.Mensaje = "Estimado cliente, se le envió el documento memo a su correo electrónico.";


                            }
                            else
                            {
                                ActionResponse.Codigo = 0;
                                ActionResponse.Titulo = "Alerta";
                                ActionResponse.Mensaje = "Estimado cliente, puede generar solicitud de memo.";
                            }
                        }
                        else
                        {
                            ActionResponse.Codigo = -1;
                            ActionResponse.Titulo = "Alerta";
                            ActionResponse.Mensaje = "Estimado cliente, favor contactar con el operador asignado a su embarque para renovar el memo.";
                        }
                    }
                    else
                    {
                        ActionResponse.Codigo = -1;
                        ActionResponse.Titulo = "Alerta";
                        ActionResponse.Mensaje = "Estimado cliente, favor contactar con el operador asignado a su embarque para adjuntar el memo.";

                    }

                }

            }
            catch (Exception e)
            {
                _logger.LogError(e, "GestionarMemo");
                ActionResponse.Codigo = -1;
                ActionResponse.Titulo = "Error";
                ActionResponse.Mensaje = "Ocurrio un errore inesperado, por favor volver a intentar.";
            }

            return Json(ActionResponse);
        }

        [HttpPost]
        public async Task<JsonResult> Solicitud()
        {
            ActionResponse = new ActionResponse();

            var parameter = new SolicitudMemoParameterVM
            {
                KeyBL = Request.Form["KeyBL"].ToString(),
                Servicio = Request.Form["Servicio"].ToString(),
                IdUsuarioCrea = usuario.idUsuario,
                Correo = usuario.CorreoUsuario
            };

            List<DocumentoMemoVM> listDoc = new List<DocumentoMemoVM>();

            foreach (IFormFile item in Request.Form.Files)
            {
                string[] strNombreSeparados = item.Name.Split('_');
                string strCodigoDocumento = strNombreSeparados[1];
                listDoc.Add(new DocumentoMemoVM { CodigoDocumento = strCodigoDocumento, NombreArchivo = item.FileName, UrlArchivo = await SaveArchivo(item) });
            }

            parameter.Documentos = listDoc;
            parameter.ImagenEmpresaLogo = usuario.Sesion.ImagenTransGroupEmpresaSeleccionado;
            
            var embarque = await _serviceEmbarques.ObtenerEmbarque(parameter.KeyBL,parameter.Servicio);
            parameter.NroEmbarque = embarque.NROBL;
            parameter.CodigoEmpresaServicio =this.usuario.Sesion.CodigoTransGroupEmpresaSeleccionado;

            var result = await _serviceEmbarque.CrearSolicitudMemo(parameter);

            if (result.CodigoResultado == 0)
            {



                // envio correo al operador
                EnviarMessageCorreoParameterVM enviarMessageCorreoParameterVM = new EnviarMessageCorreoParameterVM();
                enviarMessageCorreoParameterVM.RequestMessage = new RequestMessage();
                enviarMessageCorreoParameterVM.RequestMessage.Contenido =

                    new FormatoCorreoBody().formatoBodySolicitudMemoInterno(
                    $"<strong>Estimado usuario</strong>, <br/><br/> Considerar que se ha registrado la solicitud de devolución de cobro de garantía Nro. {result.VH_CODSOLICITUD} para que pueda proceder a evaluarla. <br/><br/> Nro. de Embarque: {embarque.NROBL} <br/> Fecha de Registro: {DateTime.Now.ToString("dd/MM/yyyy")}",
                    $"{this.GetUriHost()}/img/{usuario.Sesion.ImagenTransGroupEmpresaSeleccionado}",
                   embarque.NROBL);

                enviarMessageCorreoParameterVM.RequestMessage.Correo = embarque.OPERADOR_MAIL;
                enviarMessageCorreoParameterVM.RequestMessage.Asunto = $"Transmares Group - Solicitud de Devolución de Cobro de Garantía";
                var ressultCorreo = await _servicioMessage.EnviarMensageCorreo(enviarMessageCorreoParameterVM);

                // envio correo al cliente
                EnviarMessageCorreoParameterVM enviarMessageCorreoClienteParameter = new EnviarMessageCorreoParameterVM();
                enviarMessageCorreoClienteParameter.RequestMessage = new RequestMessage();
                enviarMessageCorreoClienteParameter.RequestMessage.Contenido =
                    new FormatoCorreoBody().formatoBodySolicitudMemoCliente($"<strong>Estimado cliente</strong>, <br/><br/>Le informamos que su solicitud de devolución de cobro de garantía Nro. {result.VH_CODSOLICITUD} ha sido registrada.",
                    $"{this.GetUriHost()}/img/{usuario.Sesion.ImagenTransGroupEmpresaSeleccionado}",
                   embarque.NROBL
                   );

                enviarMessageCorreoClienteParameter.RequestMessage.Correo = usuario.CorreoUsuario;
                enviarMessageCorreoClienteParameter.RequestMessage.Asunto = $"Transmares Group - Solicitud de Devolución de Cobro de Garantía";
                var ressultCorreoCliente = await _servicioMessage.EnviarMensageCorreo(enviarMessageCorreoClienteParameter);



                ActionResponse.Codigo = 0;
                ActionResponse.Titulo = $"Solicitud Creada";
                ActionResponse.Mensaje = $"Estimado cliente, se generó su solicitud de devolución de cobro de garantía Nro.{ result.VH_CODSOLICITUD}.";




            }
            else
            {
                ActionResponse.Codigo = -1;
                ActionResponse.Titulo = $"Alerta";
                ActionResponse.Mensaje = result.MensajeResultado;
            }

            return Json(ActionResponse);
        }

        [HttpGet]
        public async Task<IActionResult> ListarSolicitudes(ListarSolicitudesMemoModel model)
        {
            var viewModel = (model == null) ? new ListarSolicitudesMemoModel() : model;

            var listaEstado = await _serviceMaestro.ObtenerParametroPorIdPadre(34);
            ViewBag.ListarEstado = new SelectList(listaEstado.ListaParametros, "ValorCodigo", "NombreDescripcion");


            ListarSolicitudesMemoParameterVM parameter = new ListarSolicitudesMemoParameterVM();

            parameter.nroSolicitud = viewModel.CodSolicitud ?? "0";
            parameter.codEstado = viewModel.CodEstado ?? "0";
            parameter.strRuc = viewModel.Ruc ?? "0";
            parameter.CodigoEmpresaServicio =this.usuario.Sesion.CodigoTransGroupEmpresaSeleccionado;

            var listaSolicitud = await _serviceEmbarque.ObtenerSolicitudesMemo(parameter);


            viewModel.listaResultado = listaSolicitud.ListaSolicitudes;

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> VerSolicitud(string nroSolicitud)
        {
            var viewModel = await _serviceEmbarque.ObtenerSolicitudMemo(nroSolicitud);

            // Obtenermos los motivos de rechazo
            var listaEstado = await _serviceMaestro.ObtenerParametroPorIdPadre(28);
            ViewBag.ListarMotivosRechazos = new SelectList(listaEstado.ListaParametros, "ValorCodigo", "NombreDescripcion");



            return View(viewModel);
        }

        public async Task<JsonResult> ActualizarDocumentoPorSolicitud(SolicitudMemoDocumentoEstadoParameterVM parameterVM)
        {
            ActionResponse actionResponse = new ActionResponse();

            try {

                parameterVM.IdUsuarioEvalua =usuario.idUsuario;

                var mensajeResult = await _serviceEmbarque.ActualizarEstadoDocumentoMemo(parameterVM);
                actionResponse.Codigo = mensajeResult.CodigoResultado;
                actionResponse.Mensaje = mensajeResult.MensajeResultado;

                if (mensajeResult.CodigoResultado == 0) {
                    actionResponse= await ProcesarSolicitud(parameterVM.CodigoSolicitud,this.usuario.idUsuario, parameterVM.codigoEstadoEvalua, parameterVM.CodigoMotivoRechazo);
                }
                

            } catch (Exception err)
            {
                _logger.LogError(err,"ActualizarEstadoDocumento");
                actionResponse.Codigo =-100;
                actionResponse.Mensaje = "Ocurrio un error inesperado, por favor volver a intentar más tarde.";
            }


            return Json(actionResponse);

        }

        private async Task<ActionResponse> ProcesarSolicitud(string codSolicitud, int IdUsuarioEvalua, string codigoEstadoEvaua,string codigoMotivoRechazo)
        {
            ActionResponse actionResponse = new ActionResponse();

            try
            {
                SolicitudMemoEstadoParameterVM parameterVM = new SolicitudMemoEstadoParameterVM();
                parameterVM.CodigoSolicitud = codSolicitud;
                parameterVM.IdUsuarioModifica = IdUsuarioEvalua;
                parameterVM.CodigoMotivoRechazo = codigoMotivoRechazo;
                parameterVM.CodigoEstadoEvalua = codigoEstadoEvaua;
                parameterVM.ImagenEmpresaLogo = $"{this.GetUriHost()}/img/{this.usuario.Sesion.ImagenTransGroupEmpresaSeleccionado}";
                var mensajeResult = await _serviceEmbarque.ProcesarSolicitudMemo(parameterVM);

                actionResponse.Codigo = mensajeResult.CodigoResultado;
                actionResponse.Mensaje = mensajeResult.MensajeResultado;

            }
            catch (Exception err)
            {
                _logger.LogError(err, "ActualizarEstadoDocumento");
                actionResponse.Codigo = -100;
                actionResponse.Mensaje = "Ocurrio un error inesperado";
            }

            return actionResponse;

        }

        private async Task<string> SaveArchivo(IFormFile file)
        {
            string path = "";
            string strnombreFile = "";
            bool iscopied = false;

            try
            {
                if (file.Length > 0)
                {
                    strnombreFile = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\tmpdwac\\memo\\"));
                    using (var filestream = new FileStream(Path.Combine(path, strnombreFile), FileMode.Create))
                    {
                        await file.CopyToAsync(filestream);
                    }
                    iscopied = true;
                }
                else
                {
                    iscopied = false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SaveArchivo");
                throw;
            }


            return strnombreFile;
        }

    }
}