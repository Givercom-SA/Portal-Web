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
using ViewModel.Datos.Embarque.SolicitudDireccionamiento;
using System.IO;
using static Utilitario.Constante.EmbarqueConstante;
using Service.Common.Logging.Application;
using Microsoft.Extensions.Logging;
using Utilitario.Constante;

namespace Web.Principal.Areas.GestionarEmbarques.Controllers
{
    [Area("GestionarEmbarques")]
    public class DireccionamientoController : BaseController
    {
        private readonly ServicioEmbarques _serviceEmbarques;
        private readonly ServicioEmbarque _serviceEmbarque;
        private readonly ServicioMaestro _serviceMaestro;

        private readonly IMapper _mapper;
        private static ILogger _logger = ApplicationLogging.CreateLogger("DireccionamientoController");

        public DireccionamientoController(ServicioEmbarques serviceEmbarques,
                                    ServicioEmbarque serviceEmbarque,
                                    ServicioMaestro serviceMaestro,
                                    IMapper mapper)
        {
            _serviceEmbarques = serviceEmbarques;
            _serviceEmbarque = serviceEmbarque;
            _serviceMaestro = serviceMaestro;
            _mapper = mapper;
        }

        public async Task<IActionResult> Solicitud(string codigo, string servicio, string origen)
        {
            var embarque = await _serviceEmbarques.ObtenerEmbarque(codigo, servicio);
            var listModalidad = await _serviceMaestro.ObtenerParametroPorIdPadre(62);

            DireccionamientoSolicitud model = new();
            model.FlagCargaPeligrosa = embarque.FLAG_CARGA_PELIGROSA;
            model.FlagLOI = embarque.FLAG_LOI;
            model.CodTipoConsignatario = embarque.COD_TIPO_CONSIGNATARIO;
            model.FlagPlazoEta = embarque.FLAG_PLAZO_ETA;
            model.VenciemientoPlazo = embarque.VENCIMIENTO_PLAZO;
            model.KeyBL = codigo;
            model.ListModalidad = new SelectList(listModalidad.ListaParametros, "ValorCodigo", "NombreDescripcion");
            ViewBag.IdPerfil = usuario.IdPerfil;
            model.FlagDireccionamientoPermanente = embarque.FLAG_DIRECCIONAMIENTO_PERMANENTE;
            model.Almacen = embarque.ALMACEN;
            model.CantidadCtn = embarque.CANTIDAD_CNT;
            model.Consignatario = embarque.CONSIGNATARIO;
            model.NaveViaje = embarque.NAVEVIAJE;
            model.NroBL = embarque.NROBL;

            model.Servicio = servicio;
            model.Origen = origen;
         

            //if(model.FlagPlazoEta.Equals("0")) // No cumple con el plazo (Mostrar solo modalidad Diferido)
            //{
            //    model.ListModalidad = new SelectList(listModalidad.ListaParametros.Where(x=>x.ValorCodigo=="1").ToList(), "ValorCodigo", "NombreDescripcion");
            //}
            //else
            //{
            //    model.ListModalidad = new SelectList(listModalidad.ListaParametros, "ValorCodigo", "NombreDescripcion");
            //}
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> Solicitud(DireccionamientoSolicitud model)
        {
            ActionResponse = new ActionResponse();

            var parameter = new SolicitudDireccionamientoParameterVM
            {
                KeyBL = model.KeyBL,
                CodAlmacen = model.CodigoAlmacen,
                CodModalidad = model.Modalidad,
                TipoDocumento = EmbarqueConstante.TipoDocumento.RUC.ToString(),
                NumeroDocumento = model.RUC,
                CodigoTaf = model.CodigoTaf,
                RazonSocial = model.RazonSocial,
                Correo = usuario.CorreoUsuario,
                VencimientoPlazo = model.VenciemientoPlazo,
                IdUsuario = usuario.idUsuario,
                Identidad = usuario.IdEntidad,
                IdEntidadSeleccionado = usuario.Sesion.CodigoTransGroupEmpresaSeleccionado,
                FlagDireccionamientoPermanente = model.FlagDireccionamientoPermanente,
                Almacen = model.Almacen,
                CantidadCtn = model.CantidadCtn,
                NaveViaje = model.NaveViaje,
                Consignatario = model.Consignatario,
                NroBL = model.NroBL,
                ImagenEmpresaLogo=$"{this.GetUriHost()}/img/{usuario.Sesion.ImagenTransGroupEmpresaSeleccionado}"
            };

            List<DocumentoVM> listDoc = new List<DocumentoVM>();

            if (model.Modalidad == "2") // Sada descarga  directa
            {
                if (model.FlagCargaPeligrosa == "1")
                {
                    listDoc.Add(new DocumentoVM { CodigoDocumento = "DR15", NombreArchivo = model.FileMSDS.FileName, UrlArchivo = await SaveArchivo(model.FileMSDS) });
                    listDoc.Add(new DocumentoVM { CodigoDocumento = "DR16", NombreArchivo = model.FileSeguridad.FileName, UrlArchivo = await SaveArchivo(model.FileSeguridad) });
                }
            }

            if (model.FlagLOI == "1")
                listDoc.Add(new DocumentoVM { CodigoDocumento = "DR17", NombreArchivo = model.FileLOI.FileName, UrlArchivo = await SaveArchivo(model.FileLOI) });

            listDoc.Add(new DocumentoVM { CodigoDocumento = "DR16", NombreArchivo = model.FileLetter.FileName, UrlArchivo = await SaveArchivo(model.FileLetter) });

            if (usuario.IdPerfil == 7 || usuario.IdPerfil == 8)
                listDoc.Add(new DocumentoVM { CodigoDocumento = "DR19", NombreArchivo = model.FileCartaResponsabilidad.FileName, UrlArchivo = await SaveArchivo(model.FileCartaResponsabilidad) });

            if (model.CodTipoConsignatario.ToUpper() == "CL")
                listDoc.Add(new DocumentoVM { CodigoDocumento = "DR20", NombreArchivo = model.FileCartaGarantia.FileName, UrlArchivo = await SaveArchivo(model.FileCartaGarantia) });

            parameter.Documentos = listDoc;
            parameter.NombreCompleto =usuario.obtenerNombreCompleto();
            var result = await _serviceEmbarque.SolicitudDireccionamientoCrear(parameter);

            if (result.CodigoResultado == 0)
            {
                ActionResponse.Codigo = 0;
                ActionResponse.Mensaje = string.Format("Estimado cliente, se generó su solicitud de direccionamiento {0}.", result.VH_CODSOLICITUD);

            }
            else
            {
                ActionResponse.Codigo = -1;
                ActionResponse.Mensaje = result.MensajeResultado;
            }

            return Json(ActionResponse);
        }

        [HttpGet]
        public async Task<JsonResult> ValidarSolicitudDireccionamiento(string KeyBL, string servicio)
        {
            ActionResponse = new ActionResponse();

            try
            {
                var embarque = await _serviceEmbarques.ObtenerEmbarque(KeyBL, servicio);

                var result = await _serviceEmbarque.ValidarSolicitudDireccionamiento(KeyBL);
                ActionResponse.Codigo = result.CodigoResultado;
                ActionResponse.Mensaje = result.MensajeResultado;

                if (ActionResponse.Codigo == 0){

                    if (embarque.FLAG_DIRECCIONAMIENTO_PERMANENTE.Equals("1") && embarque.FLAG_DIRECCIONAMIENTO_PERMANENTE_BL.Equals("1")) {
                        ActionResponse.Codigo = 2;
                        ActionResponse.Mensaje = $"Estimado cliente, favor considerar que ya confirmó la instrucción de dirección al almacén {embarque.RAZON_SOCIAL_DEPOSITO_PERMANENTE}.";
                        
                    }
                  

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ValidarSolicitudDireccionamiento()");


                ActionResponse.Codigo = -1;
                ActionResponse.Mensaje = "Ocurrió un error inesperado, intente más tarde.";
            }
            return Json(ActionResponse);
        }

        public async Task<JsonResult> ObtenerEntidad(string CodModalidad, string NroDocumento)
        {
            ActionResponse = new ActionResponse();
            string TipoEntidad = (CodModalidad == "2") ? EntidadTransmaresTipoEntidad.GESTION_DIRECCIONAMIENTO_MODALIDAD_SADA_DD : EntidadTransmaresTipoEntidad.GESTION_DIRECCIONAMIENTO_MODALIDAD_NO_SADA_DD;
            var entidad = await _serviceEmbarques.ObtenerEntidades(EntidadTransmaresTipoDocumento.RUC,
                                                                    NroDocumento,
                                                                    string.Empty,
                                                                    EntidadTransmaresOpcion.GESTION_DIRECCIONAMIENTO,
                                                                    TipoEntidad);

            if (entidad != null && entidad.Count > 0)
            {
                ActionResponse.Codigo = 0;
                ActionResponse.Mensaje = string.Empty;

                ActionResponse.Objeto = entidad[0];
            }
            else
            {
                ActionResponse.Codigo = -1;
                ActionResponse.Mensaje = "Datos no encontrados.";
            }
            return Json(ActionResponse);
        }

        [HttpGet]
        public async Task<IActionResult> ListarSolicitudes(ListarSolicitudesModel model)
        {
            var viewModel = (model == null) ? new ListarSolicitudesModel() : model;

            var listaEstado = await _serviceMaestro.ObtenerParametroPorIdPadre(34);
            ViewBag.ListarEstado = new SelectList(listaEstado.ListaParametros, "ValorCodigo", "NombreDescripcion");

            var listaSolicitud = await _serviceEmbarque.ObtenerSolicitudes(viewModel.CampoCodSolicitud ?? "0",
                viewModel.CampoRuc ?? "0", viewModel.CodEstado ?? "0");

            if (listaSolicitud.CodigoResultado == 0)
                viewModel.listaResultado = listaSolicitud.ListaSolicitudes;

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> VerSolicitud(string nroSolicitud)
        {
            var viewModel = await _serviceEmbarque.ObtenerSolicitudPorCodigo(nroSolicitud);

            // Obtenermos los motivos de rechazo
            var listaEstado = await _serviceMaestro.ObtenerParametroPorIdPadre(28);
            ViewBag.ListarMotivosRechazos = new SelectList(listaEstado.ListaParametros, "ValorCodigo", "NombreDescripcion");

            return View(viewModel);
        }

        [HttpPost]
        public async Task<JsonResult> ActualizarEstadoPorSolicitud(ParameterDireccionamientoDocumentoModel model)
        {

            EvaluarDireccionamientoVM evaluarDireccionamientoVM = new EvaluarDireccionamientoVM();

            ProcesarDireccionamientoParameterVM parameter = new ProcesarDireccionamientoParameterVM();
            parameter.codSolicitud = model.CodigoSolicitudDireccionamiento;
            parameter.CodigoEstado = model.EstadoCodigo;
            parameter.CodigoMotivoRechazo = model.CodigoMotivoRechazo=="0"?"" : model.CodigoMotivoRechazo;
            parameter.NombreCompletoUsuario = usuario.obtenerNombreCompleto();
            parameter.CorreoUsuarioOperador = usuario.CorreoUsuario;
            
            parameter.nombreImagenEmpresa =$"{this.GetUriHost()}/img/{usuario.Sesion.ImagenTransGroupEmpresaSeleccionado}" ;
            parameter.idUsuario = usuario.idUsuario;

            if (model.CantidadSeleccionado > 0) {
                evaluarDireccionamientoVM =await _serviceEmbarque.ProcesarSolicitud(parameter);
            }


            return Json(evaluarDireccionamientoVM);
         
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
                    path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\tmpdwac\\Direccionamiento\\"));
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
                _logger.LogError(ex, "Error interno Direccionamiento");
                throw;
            }


            return strnombreFile;
        }

        [HttpGet]
        public async Task<JsonResult> RegistrarDireccionamientoPermanente(string KeyBL, string CodigoTaf, string RazonSocial)
        {
            ActionResponse = new ActionResponse();

            try
            {
                var result = await _serviceEmbarques.RegistrarDireccionamientoPermanente(usuario.Sesion.CodigoTransGroupEmpresaSeleccionado,
                    KeyBL,CodigoTaf,usuario.CorreoUsuario);

                if(result == 1)
                {
                    ActionResponse.Codigo = 1;
                    ActionResponse.Mensaje = string.Format("Estimado cliente, de acuerdo a su confirmación se procederá con la instrucción al almacén {0}.", RazonSocial);
                }
                else
                {
                    ActionResponse.Codigo = 0;
                    ActionResponse.Mensaje = "Estimado cliente, no se registro el direccionamiento permanete. Ocurrio un error interno, por favor volver a intentar más tarde.";
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RegistrarDireccionamientoPermanente()");

                ActionResponse.Codigo = -1;
                ActionResponse.Mensaje = "Ocurrió un error inesperado, intente más tarde.";
                
            }
            return Json(ActionResponse);
        }


    }
}
