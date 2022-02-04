using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Servicio.Solicitud.Models;
using Servicio.Solicitud.Repositorio;
using Servicio.Solicitud.ServiceConsumer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TransMares.Core;
using ViewModel.Datos.ListarEventos;
using ViewModel.Datos.ListarSolicitudes;
using ViewModel.Datos.ListarTipoEntidadSolicitud;
using ViewModel.Datos.Message;
using ViewModel.Datos.Solicitud;
using ViewModel.Datos.SolictudAcceso;

namespace Servicio.Solicitud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudController : ControllerBase
    {
        private readonly ISolicitudRepository _repository;
        private readonly IMapper _mapper;
        private readonly string UrlArchivoDocbusinessPartner;
        private readonly ServicioMessage _servicioMessage;
        
        public SolicitudController(ISolicitudRepository repository, IMapper mapper, IConfiguration configuration, ServicioMessage servicioMessage)
        {
            _repository = repository;
            _mapper = mapper;
            _servicioMessage = servicioMessage;
            UrlArchivoDocbusinessPartner = $"{configuration["RutaArchivos:DocumentoBusinessPartners"]}";
        }

        [HttpPost]
        [Route("obtenerSolicitudes")]
        public ActionResult<ListarSolicitudesVW> ObtenerSolicitudes(ListarSolicitudesParameterVM parameter)
        {
            var result = _repository.ObtenerSolicitudes(_mapper.Map<ListarSolicitudesParameter>(parameter));
            return _mapper.Map<ListarSolicitudesVW>(result);
        }

        [HttpGet]
        [Route("obtenerSolicitudPorCodigo")]
        public ActionResult<SolicitudVM> obtenerSolicitudPorCodigo(string codSol)
        {
            var result = _repository.ObtenerSolicitudPorCodigo(codSol);

            /*var listaDoc = _repository.ObtenerDocumentosPorSolicitud(result.SOLI_CODIGO);
            var listaEve = _repository.ObtenerEventosPorSolicitud(result.SOLI_CODIGO);
            

            if (listaDoc.IN_CODIGO_RESULTADO == 0)
                result.ListaDocumentos = listaDoc.ListaDocumentos;

            if (listaEve.IN_CODIGO_RESULTADO == 0)
                result.ListaEventos = listaEve.ListaEventos;*/

            var listaTipoEntidad = _repository.ObtenerTipoEntidadPorSolicitud(result.SOLI_CODIGO);

            if (listaTipoEntidad.IN_CODIGO_RESULTADO == 0)
                result.ListaTipoEntidad = listaTipoEntidad.ListaEntidades;

            return _mapper.Map<SolicitudVM>(result);
        }

        [HttpPut]
        [Route("actualizarEstadoDocumento/{codSolicitud}/{codDocumento}/{CodEstado}/{CodEstadoRechazo}/{userId}")]
        public ActionResult<string> actualizarEstadoDocumento(string codSolicitud, string codDocumento, string CodEstado, string CodEstadoRechazo, int userId)
        {
            var result = _repository.ActualizarSolicitudPorCodigo(codSolicitud, codDocumento, CodEstado, CodEstadoRechazo, userId);

            return string.Empty;
        }

        //[HttpPut]
        //[Route("procesarSolicitud/{codSolicitud}")]
        //public ActionResult<string> procesarSolicitud(string codSolicitud)
        //{
        //    _repository.ProcesarSolicitud(codSolicitud);
        //    var solicitud = _repository.ObtenerSolicitudPorCodigo(codSolicitud);

        //    if(!solicitud.SOLI_ESTADO_CODIGO.Trim().Equals("SP"))
        //    {
        //        if (solicitud.SOLI_ESTADO_CODIGO.Trim().Equals("SA"))
        //        {
        //            enviarCorreo(solicitud.SOLI_CORREO,
        //                "!Bienvenido a Transmares Group!",
        //                new FormatoCorreoBody().formatoBodyBienvenidaAprobado(
        //                    string.Format("{0}: {1}", solicitud.SOLI_TIPODOCUMENTO, solicitud.SOLI_NUMERO_DOCUMENTO),
        //                    solicitud.SOLI_RAZON_SOCIAL,
        //                    string.Format("{0} {1} {2}", solicitud.SOLI_RELEGAL_NOMBRE, solicitud.SOLI_RLEGAL_APELLIDO_PATERNO,
        //                    solicitud.SOLI_RLEGAL_APELLIDO_MATERNO), solicitud.SOLI_CORREO),
        //                UrlArchivoDocbusinessPartner,
        //                (solicitud.SOLI_ACUERDO_SEGUR_CADENA_SUMINI == 1) ? true : false);
        //        }
        //        else if (solicitud.SOLI_ESTADO_CODIGO.Trim().Equals("SR"))
        //        {
        //            var listaDoc = _repository.ObtenerDocumentosPorSolicitud(codSolicitud);
        //            IList<string> listadocumentos = new List<string>();

        //            foreach (var item in listaDoc.ListaDocumentos.Where(w => w.SADO_ESTADO.Trim().Equals("SR")))
        //                listadocumentos.Add(string.Format("{0}|{1}",item.SADO_NOMDOCUMENTO,item.SADO_NOMMOTIVORECHAZO));

        //            enviarCorreo(solicitud.SOLI_CORREO,
        //                "!Comunicado de Transmares Group!",
        //                new FormatoCorreoBody().formatoBodyBienvenidaRechazada(
        //                    string.Format("{0}: {1}", solicitud.SOLI_TIPODOCUMENTO, solicitud.SOLI_NUMERO_DOCUMENTO),
        //                    solicitud.SOLI_RAZON_SOCIAL,
        //                    string.Format("{0} {1} {2}", solicitud.SOLI_RELEGAL_NOMBRE, solicitud.SOLI_RLEGAL_APELLIDO_PATERNO, solicitud.SOLI_RLEGAL_APELLIDO_MATERNO),
        //                    listadocumentos),
        //                string.Empty,
        //                false);
        //        }
        //    }

        //    return string.Empty;
        //}

        [HttpPost]
        [Route("aprobarSolicitud")]
        public ActionResult<SolicitudAccesoAprobarResultVM> aprobarSolicitud(SolicitudAccesoAprobarParameterVM parameter)
        {
            SolicitudAccesoAprobarResultVM resultVM = new SolicitudAccesoAprobarResultVM();

            try
            {
                AprobarSolicitudResult resulAprobacioSolicitud = _repository.AprobarSolicitud(_mapper.Map<AprobarSolicitudParameter>(parameter));

                if (resulAprobacioSolicitud.IN_CODIGO_RESULTADO == 0)
                {

                    var solicitud = _repository.ObtenerSolicitudPorCodigo(parameter.CodigoSolicitud);
                    enviarCorreo(solicitud.SOLI_CORREO,
                                "!Bienvenido a Transmares Group!",
                                new FormatoCorreoBody().formatoBodyBienvenidaAprobado(
                                    string.Format("{0}: {1}", solicitud.SOLI_TIPODOCUMENTO, solicitud.SOLI_NUMERO_DOCUMENTO),
                                    solicitud.SOLI_RAZON_SOCIAL,
                                    string.Format("{0} {1} {2}", solicitud.SOLI_RELEGAL_NOMBRE, solicitud.SOLI_RLEGAL_APELLIDO_PATERNO,
                                    solicitud.SOLI_RLEGAL_APELLIDO_MATERNO
                                    ), solicitud.SOLI_CORREO, resulAprobacioSolicitud.Contrasenia,
                                    parameter.ImagenGrupTransmares),
                                UrlArchivoDocbusinessPartner,
                                (solicitud.SOLI_ACUERDO_SEGUR_CADENA_SUMINI == 1) ? true : false);
                }

                resultVM.CodigoResultado = resulAprobacioSolicitud.IN_CODIGO_RESULTADO;
                resultVM.MensajeResultado = resulAprobacioSolicitud.STR_MENSAJE_BD;
            }
            catch (Exception err) {


                resultVM.CodigoResultado = -200;
                resultVM.MensajeResultado = err.Message;
            }

            return resultVM;


        }

        [HttpPost]
        [Route("rechazarSolicitud")]
        public ActionResult<SolicitudAccesoAprobarResultVM> rechazarSolicitud(SolicitudAccesoAprobarParameterVM parameter)
        {
            SolicitudAccesoAprobarResultVM resultVM = new SolicitudAccesoAprobarResultVM();

            try
            {
                AprobarSolicitudResult resulAprobacioSolicitud = _repository.rechazarSolicitud(_mapper.Map<AprobarSolicitudParameter>(parameter));
                if (resulAprobacioSolicitud.IN_CODIGO_RESULTADO == 0)
                {
                    var solicitud = _repository.ObtenerSolicitudPorCodigo(parameter.CodigoSolicitud);

                    enviarCorreo(solicitud.SOLI_CORREO,
                            "!Comunicado de Transmares Group!",
                            new FormatoCorreoBody().formatoBodyBienvenidaRechazada(
                                string.Format("{0}: {1}", solicitud.SOLI_TIPODOCUMENTO, solicitud.SOLI_NUMERO_DOCUMENTO),
                                solicitud.SOLI_RAZON_SOCIAL,
                                string.Format("{0} {1} {2}", solicitud.SOLI_RELEGAL_NOMBRE, solicitud.SOLI_RLEGAL_APELLIDO_PATERNO, solicitud.SOLI_RLEGAL_APELLIDO_MATERNO),
                                parameter.MotivoRechazo,
                                  parameter.ImagenGrupTransmares),
                            string.Empty,
                            false);
                }

                resultVM.CodigoResultado = resulAprobacioSolicitud.IN_CODIGO_RESULTADO;
                resultVM.MensajeResultado = resulAprobacioSolicitud.STR_MENSAJE_BD;
            }
            catch (Exception err)
            {
                resultVM.CodigoResultado = -200;
                resultVM.MensajeResultado = err.Message;
            }
            return resultVM;
        }

        [HttpGet]
        [Route("obtenereventossolicitud")]
        public ActionResult<ListarEventosVW> ObtenerEventosPorSolicitud(string codSolicitud)
        {
            var result = _repository.ObtenerEventosPorSolicitud(codSolicitud);
            return _mapper.Map<ListarEventosVW>(result);
        }

        [HttpGet]
        [Route("obtenertipoentidadsolicitud")]
        public ActionResult<ListarEntidadesVW> ObtenerTipoEntidadPorSolicitud(string codSolicitud)
        {
            var result = _repository.ObtenerTipoEntidadPorSolicitud(codSolicitud);
            return _mapper.Map<ListarEntidadesVW>(result);
        }

        private async void  enviarCorreo(string _correo, string _asunto, string _contenido, string archivo, bool adjunto = false)
        {
       

            EnviarMessageCorreoParameterVM enviarMessageCorreoParameterVM = new EnviarMessageCorreoParameterVM();
            enviarMessageCorreoParameterVM.RequestMessage = new RequestMessage();
            enviarMessageCorreoParameterVM.RequestMessage.Contenido = _contenido;
            enviarMessageCorreoParameterVM.RequestMessage.Correo = _correo;
            enviarMessageCorreoParameterVM.RequestMessage.Asunto = _asunto;
            if (adjunto) {
                enviarMessageCorreoParameterVM.RequestMessage.Archivos = new string[1];
                enviarMessageCorreoParameterVM.RequestMessage.Archivos[0] = archivo;
            }
            

            await _servicioMessage.EnviarMensageCorreo(enviarMessageCorreoParameterVM);
           

        }
    }
}
