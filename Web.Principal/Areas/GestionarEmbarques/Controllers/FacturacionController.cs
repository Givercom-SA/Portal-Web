using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Principal.ServiceExterno;
using Web.Principal.Utils;
using Web.Principal.Areas.GestionarEmbarques.Models;
using Web.Principal.Model;
using Web.Principal.ServiceConsumer;
using ViewModel.Datos.Embarque.AsignarAgente;
using ViewModel.Datos.RegistrarNotificacionArribo;using ViewModel.Datos.Embarque.CobroPendienteFacturar;
using ViewModel.Datos.Embarque.SolicitudFacturacionTercero;
using static Utilitario.Constante.EmbarqueConstante;
using System.IO;
using Microsoft.AspNetCore.Http;
using TransMares.Core;
using Service.Common.Logging.Application;
using Microsoft.Extensions.Logging;
using Utilitario.Constante;
using ViewModel.Datos.Embarque.SolicitudFacturacion;
using ViewModel.Datos.Message;
using Microsoft.AspNetCore.SignalR;
using Web.Principal.Hubs;
using Web.Principal.Interface;

namespace Web.Principal.Areas.GestionarEmbarques.Controllers
{
    [Area("GestionarEmbarques")]
    public class FacturacionController : BaseController
    {
        private readonly ServicioEmbarques _serviceEmbarques;
        private readonly ServicioEmbarque _serviceEmbarque;
        private readonly ServicioMaestro _servicMaestro;
        private readonly ServicioMessage _servicioMessage;
        
        private readonly IMapper _mapper;
        private static ILogger _logger = ApplicationLogging.CreateLogger("FacturacionController");

        private readonly IHubContext<NotificationHub> _notificationHubContext;
        private readonly IHubContext<NotificationUserHub> _notificationUserHubContext;
        private readonly IUserConnectionManager _userConnectionManager;

        public FacturacionController(ServicioEmbarques serviceEmbarques,
                                    ServicioMaestro servicMaestro,
                                    ServicioEmbarque serviceEmbarque,
                                    IMapper mapper,
                                    ServicioMessage servicioMessage,
                                    IHubContext<NotificationHub> notificationHubContext,
            IHubContext<NotificationUserHub> notificationUserHubContext,
            IUserConnectionManager userConnectionManage)
        {
            _serviceEmbarques = serviceEmbarques;
            _serviceEmbarque = serviceEmbarque;
            _servicMaestro = servicMaestro;
            _servicioMessage = servicioMessage;
            _mapper = mapper;

            _notificationHubContext = notificationHubContext;
            _notificationUserHubContext = notificationUserHubContext;
            _userConnectionManager = userConnectionManage;

        }



        [HttpGet]
        public async Task<IActionResult> RegistroIntruccionFacturaTercero(string keyBl, string NroBL)
        {


            var userSesion = this.usuario;

            var CobrosPendientes = await _serviceEmbarque.ListarFacturacionTercerosDetallePorKeybl(keyBl);


            ListCobrosPendienteEmbarqueVM model = new ListCobrosPendienteEmbarqueVM();

            model.CobrosPendientesEmbarque = await _serviceEmbarques.ObtenerCobrosPendienteEmbarque(keyBl,"1");

            if (CobrosPendientes != null)
            {

                if (CobrosPendientes.ListFacturacionTerceroDetalle != null)
                {
                    List<string> uids = new List<string>();

                    foreach (var item in CobrosPendientes.ListFacturacionTerceroDetalle) {
                        uids.Add(item.IdProvision);
                    }
                  
                    model.CobrosPendientesEmbarque.RemoveAll(xx => uids.Contains(xx.ID));

                }
            }
        

            model.KEYBL = keyBl;
            model.BL = NroBL;
            model.SolicitarFacturacionTercero = new SolicitarFacturacionTerceroParameterVM();
            model.SolicitarFacturacionTercero.KEYBLD = keyBl;

            var listTipoDocumnentoResult = await _servicMaestro.ObtenerParametroPorIdPadre(38);

            if (listTipoDocumnentoResult.CodigoResultado == 0)
            {
                ViewBag.ListTipoDocumento = new SelectList(listTipoDocumnentoResult.ListaParametros, "ValorCodigo", "NombreDescripcion");
            }

            model.SolicitarFacturacionTercero.TipoDocumento = "";
            model.SolicitarFacturacionTercero.NumeroDocumento = "";
            model.SolicitarFacturacionTercero.RazonSocialNombres = "";


            VerificarAsignacionAgenteAduanasParameterVM parameterAgenteAduanasVeri = new VerificarAsignacionAgenteAduanasParameterVM();
            parameterAgenteAduanasVeri.KEYBLD = keyBl;

            var resultVerificarAsignacionAgenteAduanas = await _serviceEmbarque.VerificarAsignacionAgenteAduanas(parameterAgenteAduanasVeri);

            model.EstaAsginadoAgenteAduanas = "0";
            model.SolicitarFacturacionTercero.TipoEntidad = "CE";

            if (resultVerificarAsignacionAgenteAduanas.CodigoResultado==1
                && resultVerificarAsignacionAgenteAduanas.EstadoAsignacion == EmbarqueEstadoAgenteAduanas.APROBADO) {
                model.EstaAsginadoAgenteAduanas = "1";
                model.SolicitarFacturacionTercero.AgenteAduanasRazonSocial = resultVerificarAsignacionAgenteAduanas.RazonSocial.Trim();
                model.SolicitarFacturacionTercero.AgenteAduanasTipoDocumento = resultVerificarAsignacionAgenteAduanas.TipoDocumento.Trim();
                model.SolicitarFacturacionTercero.AgenteAduanasNumeroDocumento = resultVerificarAsignacionAgenteAduanas.NumeroDocumento.Trim();
            }

            model.Tenor = new Tenor();
            model.Tenor.NroEmbarque = NroBL;
            model.Tenor.RazonSocialEmpresaLogeada = userSesion.RazonSocial ;
            model.Tenor.RucEmpresaLogeada = userSesion.NumeroDocumento;
            model.Tenor.RepresentanteLegalEmpresaLogeada = userSesion.obtenerNombreCompleto();

            return View(model);
        }


        [HttpPost("/FacturarTercero/BuscarClienteExterno")]
        public async Task<IActionResult> BuscarSolicitudFacturaTerceroClientes(ListCobrosPendienteEmbarqueVM model)
        {

                var viewmodel = model.SolicitarFacturacionTercero;

                viewmodel.ClientesFacturarTerceros = new List<ClienteFacturarTerceroVM>();

                string tipoDocumento = "";


                if (viewmodel.TipoDocumento.Equals(TipoDocumento.DNI))
                {
                    tipoDocumento = EntidadTransmaresTipoDocumento.DNI;
                }
                else
                    if (viewmodel.TipoDocumento.Equals(TipoDocumento.RUC))
                {
                    tipoDocumento = EntidadTransmaresTipoDocumento.RUC;
                }

                viewmodel.ClientesFacturarTerceros = await _serviceEmbarques.ObtenerEntidades(tipoDocumento,
                                                                                                viewmodel.NumeroDocumento == null ? "" : viewmodel.NumeroDocumento,
                                                                                                viewmodel.RazonSocialNombres == null ? "" : viewmodel.RazonSocialNombres,
                                                                                                EntidadTransmaresOpcion.REGISTRO_INSTRUCCION_FACTU_TERCERO,
                                                                                                EntidadTransmaresTipoEntidad.REGISTRO_INSTRUCCION_FACTU_TERCERO);
                model.SolicitarFacturacionTercero = viewmodel;


            return PartialView("_SolicitudFactTerceroClientes", model);

        }


        [HttpGet()]
        public async Task<IActionResult> SolicitudFacturacion(string keyBl, string servicio) { 
        
            SolicitarFacturacionParameterVM model = new SolicitarFacturacionParameterVM();
            var embarqueSeleccionado = await _serviceEmbarques.ObtenerEmbarque(keyBl, servicio);
            model.CobrosPendientesCliente = new List<CobroClienteVM>();
            var cobrosPendientesFacturar = await _serviceEmbarques.ObtenerCobrosPendienteEmbarque(keyBl, "1");
            model.KEYBLD = keyBl;
            model.NroBl = embarqueSeleccionado.NROBL;
            model.Servicio = servicio;
         
            var listTipoDocumnentoResult = await _servicMaestro.ObtenerParametroPorIdPadre(38);
            if (listTipoDocumnentoResult.CodigoResultado == 0)
            {
                ViewBag.ListTipoDocumento = new SelectList(listTipoDocumnentoResult.ListaParametros, "ValorCodigo", "NombreDescripcion");
            }

            ListarProvisionFacturacionTerceroParameterVM listarProvisionFacturacionTerceroParameterVM = new ListarProvisionFacturacionTerceroParameterVM();
            listarProvisionFacturacionTerceroParameterVM.Provision = new List<ProvisionVM>();
            listarProvisionFacturacionTerceroParameterVM.KeyBl =keyBl;

            foreach (var item in cobrosPendientesFacturar) 
            {
                ProvisionVM provisionVM = new ProvisionVM();
                provisionVM.keyBl =item.keyBl;
                provisionVM.NroBl = item.NroBl;
                provisionVM.IdProvision = item.ID;
                listarProvisionFacturacionTerceroParameterVM.Provision.Add(provisionVM);
            }
            
            var ProvisionFacturacionTercero = await _serviceEmbarque.ObtenerProvicionFacturacionTercero(listarProvisionFacturacionTerceroParameterVM);

            if (ProvisionFacturacionTercero.PrivisionFacturacionTercero.Count() == 0)
            {
                model.CobrosPendientesCliente = new List<CobroClienteVM>();
                var objCobroCLiente = new CobroClienteVM();
                objCobroCLiente.CobrosPendientesEmbarque = cobrosPendientesFacturar;
                objCobroCLiente.TipoDocumentoCliente = "RUC";
                objCobroCLiente.NroDocumentoCliente = usuario.Sesion.RucIngresadoUsuario;
                objCobroCLiente.RazonSocialCliente = usuario.RazonSocial;
                objCobroCLiente.IdFacturacionTercero = "0";

                objCobroCLiente.MontoTotal = Math.Truncate(cobrosPendientesFacturar.Sum(x => Convert.ToDouble(x.Total)) * 100) / 100 ;

                model.CobrosPendientesCliente.Add(objCobroCLiente);

            }
            else
            {

                var facturacionTercero = ProvisionFacturacionTercero.PrivisionFacturacionTercero.GroupBy(x => x.IdFacturacionTercero);

                foreach (var itemFacturacionTercero in facturacionTercero)
                {

                    int idFacturacion = itemFacturacionTercero.Key;
                    var objFacturacionTercero = ProvisionFacturacionTercero.PrivisionFacturacionTercero.Where(x => x.IdFacturacionTercero == idFacturacion).FirstOrDefault();


                    var cobroCLiente = new CobroClienteVM();
                    cobroCLiente.IdFacturacionTercero = objFacturacionTercero.IdFacturacionTercero.ToString();


                    if (objFacturacionTercero.TipoEntidad == "CE")
                    {
                        cobroCLiente.IdCliente = objFacturacionTercero.CodigoClienteAgente;
                        cobroCLiente.TipoDocumentoCliente = "RUC";
                        cobroCLiente.NroDocumentoCliente = objFacturacionTercero.ClienteNroDocumento;
                        cobroCLiente.RazonSocialCliente = objFacturacionTercero.ClienteNombre;

                    }
                    else
                    {
                        cobroCLiente.IdCliente = objFacturacionTercero.CodigoClienteAgente;
                        cobroCLiente.TipoDocumentoCliente = objFacturacionTercero.AgenteTipoDocumento;
                        cobroCLiente.NroDocumentoCliente = objFacturacionTercero.AgenteNroDocumento;
                        cobroCLiente.RazonSocialCliente = objFacturacionTercero.AgenteRazonSocial;
                    }


                    List<string> idProvisiones = new List<string>();

                    foreach (var item in ProvisionFacturacionTercero.PrivisionFacturacionTercero.Where(x => x.IdFacturacionTercero == idFacturacion))
                    {
                        idProvisiones.Add(item.IdProvision.ToString());
                    }

                    cobroCLiente.CobrosPendientesEmbarque = cobrosPendientesFacturar.Where(x => idProvisiones.Contains(x.ID)).ToList();

                    cobroCLiente.MontoTotal= cobroCLiente.CobrosPendientesEmbarque.Sum(x => Convert.ToDouble( x.Total));

                    model.CobrosPendientesCliente.Add(cobroCLiente);
                }


                List<string> IDsProvosion = new List<string>();
                foreach (var item in ProvisionFacturacionTercero.PrivisionFacturacionTercero)
                {
                    IDsProvosion.Add(item.IdProvision.ToString());
                }
                cobrosPendientesFacturar.RemoveAll(xx => IDsProvosion.Contains(xx.ID));

                if (cobrosPendientesFacturar.Count() > 0)
                {
                    var objCobroCLiente = new CobroClienteVM();
                    objCobroCLiente.CobrosPendientesEmbarque = cobrosPendientesFacturar;
                    objCobroCLiente.TipoDocumentoCliente ="RUC";
                    objCobroCLiente.NroDocumentoCliente = usuario.Sesion.RucIngresadoUsuario;
                    objCobroCLiente.RazonSocialCliente = usuario.RazonSocial;
                    objCobroCLiente.IdFacturacionTercero = "0";

                    model.CobrosPendientesCliente.Add(objCobroCLiente);
                }

                // Excluir provisiones registrados
                var listSolicitudFacturacionRegistrado = await _serviceEmbarque.ListarSolicitudFacturacionPorKeyBl(keyBl);
                List<string> IDsProvosionRegistrado = new List<string>();

                foreach (var itemrReg in listSolicitudFacturacionRegistrado.SolicitudFacturaciones)
                {
                    foreach (var itemrDet in itemrReg.DetalleFacturacion)
                    {
                        IDsProvosionRegistrado.Add(itemrDet.IdProvision.ToString());
                    }

                }

                foreach (var itemCobro in model.CobrosPendientesCliente)
                {
                    itemCobro.CobrosPendientesEmbarque.RemoveAll(xx => IDsProvosionRegistrado.Contains(xx.ID));
                }

                model.CobrosPendientesCliente.RemoveAll(x => x.CobrosPendientesEmbarque.Count() == 0);
            }

            // verificar si tiene credito

            var resultCredito= await  _serviceEmbarques.AplicaCredito(keyBl,
              usuario.obtenerTipoEntidadTransmares(),
              usuario.NumeroDocumento,
              null,0);

            List<CreditoCobroModel> listCreditos = new List<CreditoCobroModel>();

            if (resultCredito == 1)
            {
                model.AplicaCredito = true;

                var listarFormaCredito = await _serviceEmbarques.ObtenerCreditoCobro(keyBl,
               usuario.obtenerTipoEntidadTransmares(),
               usuario.NumeroDocumento,
               null, 1);

                listCreditos.AddRange(listarFormaCredito);
            }

            ViewBag.ListaCredito = new SelectList(listCreditos, "COD_FORMA_CREDITO", "DES_FORMA_CREDITO");


            model.FechaTope = DateTime.Now.ToString("yyyy-MM-dd");

            return View(model);
        }


        [HttpGet()]
        public async Task<IActionResult> VerSolicitud(int codigoSolicitud)
        {

            VerSolicitudFacturacionModel model = new VerSolicitudFacturacionModel();

            LeerSolicitudFacturacionBandejaParameterVM parameterVM = new LeerSolicitudFacturacionBandejaParameterVM();
            parameterVM.IdSolicitudFacturacion = codigoSolicitud;

            var resultLeerSolicitudFacturacion = await _serviceEmbarque.LeerSolicitudFacturacionBandeja(parameterVM);

            if (resultLeerSolicitudFacturacion != null)
            {
                model.LeerSolicitudFacturacionBandejaResult = resultLeerSolicitudFacturacion;
            }
            
            




            return View(model);

        }

        [HttpGet()]
        public async Task<IActionResult> ListarSolicitudes(ListarSolicitudFacturacionBandejaModel parametro)
        {

            ListarSolicitudFacturacionBandejaModel model = new ListarSolicitudFacturacionBandejaModel();

            model.SolicitudFacturacionBandeja = new ListarSolicitudFacturacionBandejaParameterVM();
            model.SolicitudFacturacionBandeja.CodigoFacturacion = parametro.CodigoFacturacion;
            model.SolicitudFacturacionBandeja.Estado = parametro.Estado;
            model.SolicitudFacturacionBandeja.FechaRegistro = parametro.FechaRegistro;
            model.SolicitudFacturacionBandeja.NroBl = parametro.NroBl;
            model.SolicitudFacturacionBandeja.SolicitanteNombre = parametro.SolicitanteNombre;
            model.SolicitudFacturacionBandeja.NroDocumentoConsigntario = parametro.NroDocumentoConsignatario;

            model.TipoPerfil = usuario.TipoPerfil;

            model.SolicitudFacturacionBandejaResult = await _serviceEmbarque.ListarSolicitudFacturacionBandeja(model.SolicitudFacturacionBandeja);

            model.SolicitudFacturacionBandejaResult.TipoPerfil = usuario.TipoPerfil;

            var listEstado = await _servicMaestro.ObtenerParametroPorIdPadre(34);
            if (listEstado.CodigoResultado == 0)
            {
                ViewBag.ListEstado = new SelectList(listEstado.ListaParametros, "ValorCodigo", "NombreDescripcion");
            }


            return View(model);

        }

        [HttpGet]
        public async Task<IActionResult> GestionarSolicitudFacturacion(string KeyBL, string servicio)
        {
            ActionResponse ActionResponse = new();            
            ActionResponse.Codigo = 0;
            ActionResponse.Mensaje = "";

            try
            {
                var embarque = await _serviceEmbarques.ObtenerEmbarque(KeyBL, servicio);

                var listSolicitudFacturacionRegistrado = await _serviceEmbarque.ListarSolicitudFacturacionPorKeyBl(KeyBL);

                if (await PendienteRegistrar(KeyBL.ToString(), embarque.NROBL, listSolicitudFacturacionRegistrado))
                {


                    if (embarque.CONDICION.Equals(EmbarqueConstante.TipoCondicion.CONDICION_FCL) &&
                          usuario.TipoEntidad.Equals(Utilitario.Constante.EmbarqueConstante.TipoEntidad.CLIENTE_FINAL.ToString()) &&
                          embarque.FLAG_DESGLOSE.Equals("N")
                        )
                    {
                        ActionResponse.Codigo = 0;
                        ActionResponse.Mensaje = "";
                    }
                    else
                    {
                        var listCobrosPagar = await _serviceEmbarque.ObtenerCobrosPagar(KeyBL, string.Empty, string.Empty, string.Empty);
                        if (listCobrosPagar.ListaCobrosPagar != null)
                        {
                            if (listCobrosPagar.ListaCobrosPagar.Count() > 0)
                            {
                                ActionResponse.Codigo = 0;
                                ActionResponse.Mensaje = "";
                            }
                            else
                            {
                                ActionResponse.Codigo = 1;
                                if (usuario.TipoEntidad.Equals(Utilitario.Constante.EmbarqueConstante.TipoEntidad.CLIENTE_FORWARDER.ToString()))
                                {

                                    ActionResponse.Mensaje = "No hay cobros asignados por pagar.";
                                }
                                else
                                {
                                    ActionResponse.Mensaje = "No hay cobros asignados por pagar, por favor, coordinar con el forwarder.";
                                }
                            }
                        }
                        else
                        {
                            ActionResponse.Codigo = 1;
                            if (usuario.TipoEntidad.Equals(Utilitario.Constante.EmbarqueConstante.TipoEntidad.CLIENTE_FORWARDER.ToString()))
                            {
                                ActionResponse.Mensaje = "No hay cobros asignados por pagar.";
                            }
                            else
                            {
                                ActionResponse.Mensaje = "No hay cobros asignados por pagar, por favor, coordinar con el forwarder.";
                            }
                        }
                    }
                }
                else {
                    ActionResponse.Codigo = 1;
                    if (listSolicitudFacturacionRegistrado.SolicitudFacturaciones.Count() == 1)
                    {
                        if (listSolicitudFacturacionRegistrado.SolicitudFacturaciones.ElementAt(0).Estado.Equals(Utilitario.Constante.EmbarqueConstante.EstadoGeneral.APROBADO))
                            ActionResponse.Mensaje = $"Estimado cliente, ya existe una solicitud procesada (Nro. Solicitud: {listSolicitudFacturacionRegistrado.SolicitudFacturaciones.ElementAt(0).CodigoSolicitud}).";
                        else
                            ActionResponse.Mensaje = $"Estimado cliente, ya existe una solicitud en proceso (Nro. Solicitud: {listSolicitudFacturacionRegistrado.SolicitudFacturaciones.ElementAt(0).CodigoSolicitud}).";
                    }
                    else {
                        ActionResponse.Mensaje = $"Estimado cliente, ya existe más de una solicitud de facturación. (Nro. de Solicitudes: {   String.Join(", ", listSolicitudFacturacionRegistrado.SolicitudFacturaciones.Select(x=>x.CodigoSolicitud).ToArray())})";
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e,"FacturacionController");
                ActionResponse.Codigo = -1;
                ActionResponse.Mensaje = "Error al consultar el servicio.";
            }
            return Json(ActionResponse);
        }

        public async Task<bool> PendienteRegistrar(string keyBl, string nroBl, LeerSolicitudFacturacionKeyBlResultVM listSolicitudFacturacionRegistrado) {

            SolicitarFacturacionParameterVM model = new SolicitarFacturacionParameterVM();
            var cobrosPendientesFacturar = await _serviceEmbarques.ObtenerCobrosPendienteEmbarque(keyBl, "1");

        
            model.KEYBLD = keyBl;
            model.NroBl = nroBl;

            model.CobrosPendientesCliente = new List<CobroClienteVM>();

            ListarProvisionFacturacionTerceroParameterVM listarProvisionFacturacionTerceroParameterVM = new ListarProvisionFacturacionTerceroParameterVM();
            listarProvisionFacturacionTerceroParameterVM.Provision = new List<ProvisionVM>();
            listarProvisionFacturacionTerceroParameterVM.KeyBl = keyBl;

            foreach (var item in cobrosPendientesFacturar)
            {


                ProvisionVM provisionVM = new ProvisionVM();
                provisionVM.keyBl = item.keyBl;
                provisionVM.NroBl = item.NroBl;
                provisionVM.IdProvision = item.ID;

                listarProvisionFacturacionTerceroParameterVM.Provision.Add(provisionVM);
            }

            var ProvisionFacturacionTercero = await _serviceEmbarque.ObtenerProvicionFacturacionTercero(listarProvisionFacturacionTerceroParameterVM);

            if (ProvisionFacturacionTercero.PrivisionFacturacionTercero.Count() == 0)
            {
                model.CobrosPendientesCliente = new List<CobroClienteVM>();
                var objCobroCLiente = new CobroClienteVM();
                objCobroCLiente.CobrosPendientesEmbarque = cobrosPendientesFacturar;
   

                objCobroCLiente.MontoTotal = Math.Truncate(cobrosPendientesFacturar.Sum(x => Convert.ToDouble(x.Total)) * 100) / 100;

                model.CobrosPendientesCliente.Add(objCobroCLiente);

            }
            else
            {

                var facturacionTercero = ProvisionFacturacionTercero.PrivisionFacturacionTercero.GroupBy(x => x.IdFacturacionTercero);

                foreach (var itemFacturacionTercero in facturacionTercero)
                {

                    int idFacturacion = itemFacturacionTercero.Key;
                    var objFacturacionTercero = ProvisionFacturacionTercero.PrivisionFacturacionTercero.Where(x => x.IdFacturacionTercero == idFacturacion).FirstOrDefault();


                    var cobroCLiente = new CobroClienteVM();
                    cobroCLiente.IdFacturacionTercero = objFacturacionTercero.IdFacturacionTercero.ToString();



                    List<string> idProvisiones = new List<string>();

                    foreach (var item in ProvisionFacturacionTercero.PrivisionFacturacionTercero.Where(x => x.IdFacturacionTercero == idFacturacion))
                    {
                        idProvisiones.Add(item.IdProvision.ToString());
                    }

                    cobroCLiente.CobrosPendientesEmbarque = cobrosPendientesFacturar.Where(x => idProvisiones.Contains(x.ID)).ToList();

                    cobroCLiente.MontoTotal = cobroCLiente.CobrosPendientesEmbarque.Sum(x => Convert.ToDouble(x.Total));

                    model.CobrosPendientesCliente.Add(cobroCLiente);
                }


                List<string> IDsProvosion = new List<string>();
                foreach (var item in ProvisionFacturacionTercero.PrivisionFacturacionTercero)
                {
                    IDsProvosion.Add(item.IdProvision.ToString());
                }
                cobrosPendientesFacturar.RemoveAll(xx => IDsProvosion.Contains(xx.ID));

                if (cobrosPendientesFacturar.Count() > 0)
                {
                    var objCobroCLiente = new CobroClienteVM();
                    objCobroCLiente.CobrosPendientesEmbarque = cobrosPendientesFacturar;
                    objCobroCLiente.TipoDocumentoCliente = "RUC";
                    objCobroCLiente.NroDocumentoCliente = usuario.Sesion.RucIngresadoUsuario;
                    objCobroCLiente.RazonSocialCliente = usuario.RazonSocial;
                    objCobroCLiente.IdFacturacionTercero = "0";

                    model.CobrosPendientesCliente.Add(objCobroCLiente);
                }
            }


            if (model.CobrosPendientesCliente == null)
                return true;

            if (model.CobrosPendientesCliente.Count() <=0 )
                return true;

            if (listSolicitudFacturacionRegistrado == null)
                return true;

            if (listSolicitudFacturacionRegistrado.SolicitudFacturaciones == null)
                return true;

            if (listSolicitudFacturacionRegistrado.SolicitudFacturaciones.Count() <= 0)
                return true;



          if(  listSolicitudFacturacionRegistrado.SolicitudFacturaciones.Count() == model.CobrosPendientesCliente.Count())
                return false;


            return true;



        }


        [HttpPost()]
        public async Task<IActionResult> SolicitudFacturacion(SolicitarFacturacionParameterVM model)
        {
            var ActionResponse = new ActionResponse();
            ActionResponse.Codigo = 0;
            ActionResponse.Mensaje= "";
            model.FechaTope = DateTime.Now.ToString("yyyy-MM-dd");

            try
            {
                if (model.CobrosPendientesCliente != null)
                {
                    if (model.CobrosPendientesCliente.Count() > 0)
                    {
                        var embarque = await _serviceEmbarques.ObtenerEmbarque(model.KEYBLD, model.Servicio);
                        model.IdEntidadSolicita = usuario.IdEntidad;
                        model.IdUsuarioSolicita = usuario.idUsuario;
                        model.CodigoTipoEntidad = usuario.TipoEntidad;
                        var result = await _serviceEmbarque.SolicitarFacturacionRegistrar(model);

                        if (result.CodigoResultado == 0)
                        {
                            // envio correo al operador
                            EnviarMessageCorreoParameterVM enviarMessageCorreoParameterVM = new EnviarMessageCorreoParameterVM();
                            enviarMessageCorreoParameterVM.RequestMessage = new RequestMessage();
                            enviarMessageCorreoParameterVM.RequestMessage.Contenido =
                                new FormatoCorreoBody().formatoBodySolicitudFacturacionInterno(
                                $"<strong>Estimado usuario</strong>, <br/><br/> Considerar que se ha registrado la solicitud de facturación Nro. {result.CodigoSolicitud} para que pueda proceder a evaluarla.",
                                model,
                                $"{this.GetUriHost()}/img/{usuario.Sesion.ImagenTransGroupEmpresaSeleccionado}",
                                      result.CodigoSolicitud,
                               embarque.NROBL);

                            enviarMessageCorreoParameterVM.RequestMessage.Correo = embarque.FINANZAS_MAIL;
                            enviarMessageCorreoParameterVM.RequestMessage.Asunto = $"Transmares Group - Solicitud de Facturación";
                            var ressultCorreo = await _servicioMessage.EnviarMensageCorreo(enviarMessageCorreoParameterVM);
                            // envio correo al cliente

                            EnviarMessageCorreoParameterVM enviarMessageCorreoClienteParameter = new EnviarMessageCorreoParameterVM();
                            enviarMessageCorreoClienteParameter.RequestMessage = new RequestMessage();
                            enviarMessageCorreoClienteParameter.RequestMessage.Contenido = new FormatoCorreoBody().formatoBodySolicitudFacturacionCliente($"<strong>Estimado cliente</strong>, <br/><br/> Le informamos que su solicitud de facturación Nro. {result.CodigoSolicitud} ha sido registrada.",
                                model,
                                $"{this.GetUriHost()}/img/{usuario.Sesion.ImagenTransGroupEmpresaSeleccionado}",
                               result.CodigoSolicitud,
                               embarque.NROBL
                               );

                            enviarMessageCorreoClienteParameter.RequestMessage.Correo = usuario.CorreoUsuario;
                            enviarMessageCorreoClienteParameter.RequestMessage.Asunto = $"Transmares Group - Solicitud de Facturación";
                            var ressultCorreoCliente = await _servicioMessage.EnviarMensageCorreo(enviarMessageCorreoClienteParameter);


                            ActionResponse.Codigo = 0;
                            ActionResponse.Mensaje = $"Estimado cliente, se generó su solicitud de facturación Nro. {result.CodigoSolicitud}.";

                        }
                        else if (result.CodigoResultado > 1)
                        {
                            ActionResponse.Codigo = result.CodigoResultado;
                            ActionResponse.Mensaje = result.MensajeResultado;
                        }
                        else
                        {

                            ActionResponse.Codigo = -200;
                            ActionResponse.Mensaje = "Estimado cliente, ocurrio un error inesperado. Por favor volver a intentar nuevamenente.";
                        }
                    }
                    else
                    {

                        ActionResponse.Codigo = 200;
                        ActionResponse.Mensaje = "Estimado cliente, para registrar una solicitud de facturación debe existir al menos una provisión.";
                    }


                }
                else
                {
                    ActionResponse.Codigo = 200;
                    ActionResponse.Mensaje = "Estimado cliente, para registrar una solicitud de facturación debe existir al menos una provisión.";

                }

            }
            catch (Exception err)
            {
                _logger.LogError(err, "Registro de Solicitud de Facturación");
                ActionResponse.Codigo = -100;
                ActionResponse.Mensaje = "Registro de solicitud de facturación, ocurrio un error inesperado, por favor volver a intentar más tarde";
            }

            return Json(ActionResponse);
        }


        [HttpPost()]
        public async Task<IActionResult> RegistrarEstadoFacturacion(SolicitudFacturacionRegistrarEstadoModel model)
        {

            var ActionResponse = new ActionResponse();
            ActionResponse.Codigo = 0;
            ActionResponse.Mensaje = "";

            try
            {
                var resultSolicitudFacturacion = 
                    await _serviceEmbarque.LeerSolicitudFacturacionBandeja(
                        new LeerSolicitudFacturacionBandejaParameterVM() {  IdSolicitudFacturacion = model.IdSolicitudFacturacion });

                if (model.Estado.Equals("SR"))
                {
                    SolicitarFacturacionEstadoParameterVM solicitarFacturacionEstadoParameterVM = new SolicitarFacturacionEstadoParameterVM();
                    solicitarFacturacionEstadoParameterVM.Estado = "SR";
                    solicitarFacturacionEstadoParameterVM.IdSolicitudFacturacion = model.IdSolicitudFacturacion;
                    solicitarFacturacionEstadoParameterVM.IdUsuarioEvalua = usuario.idUsuario;
                    solicitarFacturacionEstadoParameterVM.ObservacionRechazo = model.Mensaje;

                    var resultRegistroSolicitudFacturacionEstado = await _serviceEmbarque.RegistrarSolicitudFacturacionEstado(solicitarFacturacionEstadoParameterVM);
                    if (resultRegistroSolicitudFacturacionEstado.CodigoResultado == 0)
                    {


                        await EnviarAlerta("Solicitud de Facturación",
                            $"Rechazado Nro. {resultSolicitudFacturacion.SolicitudFacturacion.CodigoSolicitud}",
                            DateTime.Now.ToString("dd/MM/yyyy"));

                        EnviarCorreoRespuestaSolicitud(resultSolicitudFacturacion.SolicitudFacturacion.CodigoSolicitud,
                            resultSolicitudFacturacion.SolicitudFacturacion.NroBl,
                            "Rechazada", 
                            resultSolicitudFacturacion.SolicitudFacturacion.SolicitanteCorreo,
                            $"Motivo: {model.Mensaje}");

                        ActionResponse.Codigo = 0;
                        ActionResponse.Mensaje = $"Se rechazó la solicitud de facturación Nro. {resultSolicitudFacturacion.SolicitudFacturacion.CodigoSolicitud}.";
                    }
                    else
                    {
                        ActionResponse.Codigo = 1;
                        ActionResponse.Mensaje = $"Se registro la solicitud de facturación, pero ocurrio un error al momento de actualizar los datos de la solicitud.";
                    }

                }
                else if(model.Estado.Equals("SA")) 
                {
                    RegistroSolicitudRequestModel registroSolicitudRequestModel = new RegistroSolicitudRequestModel();
                    registroSolicitudRequestModel.pIdSolicitudPW = resultSolicitudFacturacion.SolicitudFacturacion.IdSolicitudFacturacion.ToString();
                    registroSolicitudRequestModel.pNroSolicitudPW = resultSolicitudFacturacion.SolicitudFacturacion.CodigoSolicitud;
                    registroSolicitudRequestModel.pFechaSolicitud = resultSolicitudFacturacion.SolicitudFacturacion.FechaRegistro?.ToString("dd/MM/yyyy HH:mm:ss");
                    registroSolicitudRequestModel.pId_Transaccion = "";
                    registroSolicitudRequestModel.pEmpresa = usuario.Sesion.CodigoTransGroupEmpresaSeleccionado;
                    registroSolicitudRequestModel.pKeybld = resultSolicitudFacturacion.SolicitudFacturacion.KeyBld;
                    registroSolicitudRequestModel.pRucCliente = resultSolicitudFacturacion.SolicitudFacturacion.SolicitanteRUC;
                    registroSolicitudRequestModel.pTipoCliente =usuario.obtenerTipoEntidadTransmares(resultSolicitudFacturacion.SolicitudFacturacion.CodigoTipoEntidad);
                    registroSolicitudRequestModel.pUsuarioPW = resultSolicitudFacturacion.SolicitudFacturacion.SolicitanteCorreo ;
                    registroSolicitudRequestModel.pUsuarioFinPW = usuario.CorreoUsuario;
                    registroSolicitudRequestModel.pFechaEvaluacion =DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                    if (
                        (resultSolicitudFacturacion.SolicitudFacturacion.TipoPago == Utilitario.Constante.EmbarqueConstante.TipoPago.CONTADO) &&
                        resultSolicitudFacturacion.SolicitudFacturacion.MetodoPago == Utilitario.Constante.EmbarqueConstante.MetodoPago.TRANSFERENCIA)
                    {
                        registroSolicitudRequestModel.pFormaPago = "TR";
                    }
                    if (
                      (resultSolicitudFacturacion.SolicitudFacturacion.TipoPago == Utilitario.Constante.EmbarqueConstante.TipoPago.CONTADO) &&
                      resultSolicitudFacturacion.SolicitudFacturacion.MetodoPago == Utilitario.Constante.EmbarqueConstante.MetodoPago.TARJETA)
                    {
                        registroSolicitudRequestModel.pFormaPago = "PT";
                    }
                    else
                    if (resultSolicitudFacturacion.SolicitudFacturacion.TipoPago == Utilitario.Constante.EmbarqueConstante.TipoPago.CREDITO)
                    {
                        registroSolicitudRequestModel.pFormaPago = resultSolicitudFacturacion.SolicitudFacturacion.CodigoCredito;
                    } 

                    if (resultSolicitudFacturacion.SolicitudFacturacion.TipoPago == Utilitario.Constante.EmbarqueConstante.TipoPago.CONTADO)
                    {
                        registroSolicitudRequestModel.pTipoPago = "0";

                        if (resultSolicitudFacturacion.SolicitudFacturacion.MetodoPago == Utilitario.Constante.EmbarqueConstante.MetodoPago.TRANSFERENCIA)
                        {
                            registroSolicitudRequestModel.pFechaTransferencia = resultSolicitudFacturacion.SolicitudFacturacion.FechaTransferencia.ToString("dd/MM/yyyy");
                            registroSolicitudRequestModel.pImporte = Double.Parse(resultSolicitudFacturacion.SolicitudFacturacion.ImporteTransferencia);
                            registroSolicitudRequestModel.pNumeroOperacion = resultSolicitudFacturacion.SolicitudFacturacion.CodigoOperacionTransferencia;
                        }
                        else {
                            registroSolicitudRequestModel.pFechaTransferencia = null;
                            registroSolicitudRequestModel.pImporte = 0;
                            registroSolicitudRequestModel.pNumeroOperacion = null;
                        }
                    }
                    else {
                        registroSolicitudRequestModel.pFechaTransferencia = null;
                        registroSolicitudRequestModel.pImporte = 0;
                        registroSolicitudRequestModel.pNumeroOperacion = null;
                        registroSolicitudRequestModel.pTipoPago = "1";
                    }
                   
                     var resultRegistrarSolicitudFacturacion = await _serviceEmbarques.RegistrarSolicitudFacturacion(registroSolicitudRequestModel);
                    // Registrar detalle en TAF

                    bool blDetalleFacturacionOk = true;

                    foreach (var item in resultSolicitudFacturacion.SolicitudFacturacion.DetalleFacturacion)
                    {
                        RegistroSolicitudFacturacionDetalleRequestModel requestFacturacionModel = new RegistroSolicitudFacturacionDetalleRequestModel();
                        requestFacturacionModel.pIdSolicitud = resultRegistrarSolicitudFacturacion.IdSolicitudTAF;
                        requestFacturacionModel.pConcepCCodigo = item.CodigoConcepto == null ? "" : item.CodigoConcepto;
                        requestFacturacionModel.pImporte = Double.Parse(item.Importe);
                        requestFacturacionModel.pMoneda = item.Moneda == null ? "" : item.Moneda;
                        requestFacturacionModel.pRubroCCodigo = item.CodigoRubro == null ? "" : item.CodigoRubro;

                        var resultRegistrarSolicitudFacturacionDetalle = await _serviceEmbarques.RegistrarSolicitudFacturacionDetalle(requestFacturacionModel);
                        if (resultRegistrarSolicitudFacturacionDetalle.Respuesta == 0)
                        {
                            blDetalleFacturacionOk = false;
                            break;
                        }
                    }

                    if (resultRegistrarSolicitudFacturacion.Respuesta == 1 && blDetalleFacturacionOk) {

                        SolicitarFacturacionEstadoParameterVM solicitarFacturacionEstadoParameterVM = new SolicitarFacturacionEstadoParameterVM();
                        solicitarFacturacionEstadoParameterVM.Estado ="SA";
                        solicitarFacturacionEstadoParameterVM.IdSolicitudFacturacion =model.IdSolicitudFacturacion;
                        solicitarFacturacionEstadoParameterVM.IdUsuarioEvalua =usuario.idUsuario ;
                        solicitarFacturacionEstadoParameterVM.ObservacionRechazo =model.Mensaje;
                        solicitarFacturacionEstadoParameterVM.IdSolicitudTAFF = resultRegistrarSolicitudFacturacion.IdSolicitudTAF;

                        var resultRegistroSolicitudFacturacionEstado = await _serviceEmbarque.RegistrarSolicitudFacturacionEstado(solicitarFacturacionEstadoParameterVM);
                        if (resultRegistroSolicitudFacturacionEstado.CodigoResultado == 0)
                        {

                          await  EnviarAlerta("Solicitud de Facturación",
                              $"Aprobado Nro. {resultSolicitudFacturacion.SolicitudFacturacion.CodigoSolicitud}" ,
                              DateTime.Now.ToString("dd/MM/yyyy"));

                            EnviarCorreoRespuestaSolicitud(resultSolicitudFacturacion.SolicitudFacturacion.CodigoSolicitud,
                                resultSolicitudFacturacion.SolicitudFacturacion.NroBl, "Aprobada", 
                                resultSolicitudFacturacion.SolicitudFacturacion.SolicitanteCorreo,
                                "");

                            ActionResponse.Codigo = 0;
                            ActionResponse.Mensaje = $"Se aprobó la solicitud de facturación Nro. {resultSolicitudFacturacion.SolicitudFacturacion.CodigoSolicitud}.";
                        }
                        else {
                            ActionResponse.Codigo = 1;
                            ActionResponse.Mensaje = $"Se registro la solicitud de facturación, pero ocurrio un error al momento de actualizar los datos de la solicitud.";
                        }
                        
                    }else {
                        ActionResponse.Codigo = 1;
                        ActionResponse.Mensaje = $"Ocurrio un error inesperado en los servicios de Transmares Group. Por favor volver a intentar más tarde.";
                    }
                }
            }
            catch (Exception err)
            {
                _logger.LogError(err, "Registro de estado de solicitud de facturacion.");
                ActionResponse.Codigo = -100;
                ActionResponse.Mensaje = "Registro de estado de solicitud de facturación, ocurrio un error inesperado, por favor volver a intentar más tarde.";
            }

            return Json(ActionResponse);
        }



        private async Task  EnviarAlerta (string titulo, string mensaje, string fecha) {

            //await  _notificationHubContext.Clients.All.SendAsync("sendToUser", titulo, mensaje, fecha);
            try
            {
                var connections = _userConnectionManager.GetConnections();
                if (connections != null && connections.Count > 0)
                {
                    foreach (var connectionId in connections)
                    {
                        await _notificationUserHubContext.Clients.Client(connectionId).SendAsync("sendToUser", titulo, mensaje, fecha);
                    }
                }
            }
            catch (Exception err) {

                _logger.LogError(err,"Enviar Alerta");

            }

        }

        private  async void EnviarCorreoRespuestaSolicitud(string pSolicitud, string pNroBl, string pEstado, string pCorreo, string MotivoRechazo)
        {
            EnviarMessageCorreoParameterVM enviarMessageCorreoParameterVM = new EnviarMessageCorreoParameterVM();
            enviarMessageCorreoParameterVM.RequestMessage = new RequestMessage();
            enviarMessageCorreoParameterVM.RequestMessage.Contenido = new FormatoCorreoBody().formatoBodySolicitudFacturacionAprobadoRechazo(pSolicitud,
                pEstado, $"{this.GetUriHost()}/img/{usuario.Sesion.ImagenTransGroupEmpresaSeleccionado}",
                MotivoRechazo);
            enviarMessageCorreoParameterVM.RequestMessage.Correo = pCorreo;
            enviarMessageCorreoParameterVM.RequestMessage.Asunto = $"Transmares Group - Solicitud de Facturación {pEstado}";
            await _servicioMessage.EnviarMensageCorreo(enviarMessageCorreoParameterVM);
        }

        [HttpPost()]
        public async Task<IActionResult> ValidarMetodoPagoSolicFacturacion(SolicitarFacturacionParameterVM model)
        {
            ActionResponse actionResponse = new ActionResponse();
            try
            {
                if (model.TipoPago.Equals(TipoPago.CREDITO)) {
                    actionResponse = await validarDisponibilidadCredito(model.TipoPago, model.KEYBLD, model.CodigoCredito);
                }
                

            }
            catch (Exception err) {


                _logger.LogError(err, "Registro de Solicitud de Facturacion");
                ActionResponse.Codigo = -100;
                ActionResponse.Mensaje = "ocurrio un error inesperado cuando se registraba una solicitud de facturación, por favor volver a intentar más tarde.";
            }



            return Json(actionResponse);


        }

        public async Task<ActionResponse> validarDisponibilidadCredito(string tipoPago, string keybl, string codigoFormaCredito)
        {

            var ActionResponse = new ActionResponse();
            ActionResponse.Codigo = 0;
            ActionResponse.Mensaje = "Validado con éxito";


            if (tipoPago.Equals(EmbarqueConstante.TipoPago.CREDITO))
            {
                var resultCreditoDisponible = await _serviceEmbarques.DisponibilidadCredito(keybl,
                          usuario.obtenerTipoEntidadTransmares(),
                          usuario.NumeroDocumento,
                         codigoFormaCredito, 3);

                if (resultCreditoDisponible.Resultado == 0)
                {
                    ActionResponse.Codigo = 1;
                    ActionResponse.Mensaje = $"Estimado cliente, favor considerar que la forma de crédito seleccionada no está disponible.<br/><br/><strong>Motivo: {resultCreditoDisponible.Motivo}</strong>.";
                }
                else if (resultCreditoDisponible.Resultado < 0)
                {
                    ActionResponse.Codigo = resultCreditoDisponible.Resultado;
                    ActionResponse.Mensaje = "Estimado cliente, ocurrio un error interno con los servicios de Transmares, por favor volver a intentar más tarde. ";
                }
                else
                {
                    ActionResponse.Codigo = 0;
                    ActionResponse.Mensaje = $"";
                }
            }

            return ActionResponse;

        }

        [HttpPost()]
        public async Task<JsonResult> RegistrarFacturaTercero(ListCobrosPendienteEmbarqueVM model)
        {
            var ActionResponse = new ActionResponse();

            try
            {
                if (ModelState.IsValid)
                {
                    var resultSesion = HttpContext.Session.GetUserContent();

                    RegistrarFacturacionTerceroParameterVM parameter = new RegistrarFacturacionTerceroParameterVM();
                    parameter.CobrosPendientesEmbarque = model.CobrosPendientesEmbarque.Where(x => x.Check == true).ToList();
                    parameter.EmbarqueKeyBL = model.KEYBL;
                    parameter.EmbarqueNroBL = model.BL;
                    parameter.IdEntidad = resultSesion.IdEntidad.ToString();
                    parameter.Archivo = "";
                    parameter.ClienteNombres = model.SolicitarFacturacionTercero.ClienteRazonNombre;
                    parameter.CodigoCliente = model.SolicitarFacturacionTercero.ClienteSeleccionado;
                    parameter.ClienteNroDocumeto = model.SolicitarFacturacionTercero.ClienteNroDocumento;
                    parameter.IdUsuario = resultSesion.idUsuario.ToString();
                    parameter.IdUsuarioCrea = resultSesion.idUsuario.ToString();

                    parameter.TipoEntidad = model.SolicitarFacturacionTercero.TipoEntidad.ToString();
                    parameter.AgenteAduanaNumeroDocumento = model.SolicitarFacturacionTercero.AgenteAduanasNumeroDocumento;
                    parameter.AgenteAduanaRazonSocial = model.SolicitarFacturacionTercero.AgenteAduanasRazonSocial;
                    parameter.AgenteAduanaTipoDocumento = model.SolicitarFacturacionTercero.AgenteAduanasTipoDocumento;

                    var resultRegistroFacturacionTercero = await _serviceEmbarque.RegistrarFacturacionTercero(parameter);
                    ActionResponse.Codigo = resultRegistroFacturacionTercero.CodigoResultado;
                    ActionResponse.Mensaje = resultRegistroFacturacionTercero.MensajeResultado;


                    if (ActionResponse.Codigo == 0)
                    {

                        // Lista de correos para enviar
                        var listaFinanzas = await _servicMaestro.ObtenerCorreosPorPerfil(4);
                        
                        // Envio de correo
               

                        EnviarMessageCorreoParameterVM enviarMessageCorreoParameterVM = new EnviarMessageCorreoParameterVM();
                        enviarMessageCorreoParameterVM.RequestMessage = new RequestMessage();
                        enviarMessageCorreoParameterVM.RequestMessage.Contenido = new FormatoCorreoBody().formatoBodyFacturacionTerceros(resultSesion.RazonSocial,
                            model.BL,
                           $"{this.GetUriHost()}/img/{resultSesion.Sesion.ImagenTransGroupEmpresaSeleccionado}", resultRegistroFacturacionTercero.CodigoFacturaTercero);

                        enviarMessageCorreoParameterVM.RequestMessage.Cuentas = listaFinanzas.ListaCorreos.ToArray();

                        enviarMessageCorreoParameterVM.RequestMessage.Asunto = string.Format("Facturación a Terceros - Numeración de  Embarque: {0}", model.BL);
                        var ressult = await _servicioMessage.EnviarMensageCorreo(enviarMessageCorreoParameterVM);



                    }


                }
                else
                {

                    var erroneousFields = ModelState.Where(ms => ms.Value.Errors.Any())
                                          .Select(x => new { x.Key, x.Value.Errors });

                    foreach (var erroneousField in erroneousFields)
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
            }
            catch (Exception err)
            {
                _logger.LogError(err,"Registro de Facturacion a Tercero");
                ActionResponse.Codigo = -100;
                ActionResponse.Mensaje = "Error inesperado, por favor volver a intentar mas tarde";
            }

            return Json(ActionResponse);
        }

        private async Task<string> saveArchivo(IFormFile file)
        {
            string path = "";
            string strnombreFile = "";
            bool iscopied = false;


            try
            {
                if (file.Length > 0)
                {
                    strnombreFile = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\tmpdwac\\facturacion_tercero\\"));
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
                _logger.LogError(ex, "Guardar archivo");
                throw;
            }


            return strnombreFile;
        }

        [HttpPost]
        public async Task<JsonResult> VerificarSiAplicaCredito(AplicaCreditoVirtualModel parametro)
        {
            var ActionResponse = new ActionResponse();
            ActionResponse.Codigo = 0;
            ActionResponse.Mensaje = "";

            try
            {

                var aplicaCreditoVirtualResult = await _serviceEmbarques.AplicaCreditoVirtal(parametro.KeyBl,
                       usuario.obtenerTipoEntidadTransmares(),
                       usuario.NumeroDocumento,
                       parametro.CodFormaCredito, 2);

                if (aplicaCreditoVirtualResult == 1)
                {

                    var resultCreditoDisponible = await validarDisponibilidadCredito(EmbarqueConstante.TipoPago.CREDITO.ToString(), parametro.KeyBl, parametro.CodFormaCredito);

                    ActionResponse.Codigo = resultCreditoDisponible.Codigo;
                    ActionResponse.Mensaje = resultCreditoDisponible.Mensaje;

                }
                else
                {
                    ActionResponse.Codigo = 1;
                    ActionResponse.Mensaje = "Estimado cliente, favor considerar que la forma de crédito seleccionada sólo aplica para atención presencial.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "VerificarSiAplicaCredito");
                ActionResponse.Codigo = -1;
                ActionResponse.Mensaje = "Error inesperado, por favor volver a intentar mas tárde.";
            }

            return Json(ActionResponse);
        }

        [HttpGet]
        public async Task<IActionResult> ListarFacturacionTerceroHistorial(int id)
        {
            ListarFacturacionTerceroDetalleResultVM model = new();

            model = await _serviceEmbarque.ListarFacturacionTercerosDetalle(id);

            return PartialView("_ResultadoFacturacionTerceroHistorial", model);

        }
        public async Task<IActionResult> VerFacturacionTercero(int Id)
        {
            ListarFacturacionTerceroDetalleResultVM model = new();

            model = await _serviceEmbarque.ListarFacturacionTercerosDetalle(Id);

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> BandejaFacturaTercero(ListarSolicitudFacturacionTerceroModel model)
        {

            var resultSesion = HttpContext.Session.GetUserContent();

            if (model == null)
            {
                model = new ListarSolicitudFacturacionTerceroModel();
            }

            ListarFacturacionTerceroParameterVM parameter = new ListarFacturacionTerceroParameterVM();

            parameter.EmbarqueNroBL = model.EmbarqueNroBL;
            parameter.Cliente = model.Cliente;
            parameter.NroDocumento = model.NroDocumento;
            parameter.Estado = model.Estado;
            parameter.Entidad = model.Entidad;

            if (resultSesion.TipoPerfil.Equals(SeguridadConstante.TipPerfil.EXTERNO))
            {
                parameter.IdEntidad = resultSesion.IdEntidad;
            }

            var listEstado = await _servicMaestro.ObtenerParametroPorIdPadre(34);
            if (listEstado.CodigoResultado == 0)
            {
                ViewBag.ListEstado = new SelectList(listEstado.ListaParametros, "ValorCodigo", "NombreDescripcion");
            }

            model.model = await _serviceEmbarque.ListarFacturacionTerceros(parameter);


            model.TipoPerfil = resultSesion.TipoPerfil;

            if (resultSesion.TipoPerfil.Equals(SeguridadConstante.TipPerfil.EXTERNO))
            {

                if (resultSesion.TipoDocumento.Equals(EmbarqueConstante.TipoDocumento.DNI))
                {
                    model.Entidad = resultSesion.obtenerNombreCompleto();
                }
                else
                {
                    model.Entidad = resultSesion.RazonSocial;
                }


            }


            return View(model);
        }


    }

}
