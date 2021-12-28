using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.SolictudAcceso;
using Web.Principal.ServiceConsumer;
using Web.Principal.Utils;

namespace Web.Principal.Controllers
{


    [AllowAnonymous]

    public class SolicitarAccesoController : BaseLibreController
    {

        private readonly ServicioAcceso _serviceAcceso;
        private readonly ServicioMaestro _serviceMaestro;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        

        public SolicitarAccesoController(
            ServicioAcceso servicioAcceso,
            ServicioMaestro servicioMaestro,
            IConfiguration configuration,

            IMapper mapper)
        {
            _serviceAcceso = servicioAcceso;
            _serviceMaestro = servicioMaestro;
            _mapper = mapper;
            _configuration = configuration;
        }



        [AllowAnonymous]

        // GET: SolicitarAccesoController
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost("ValidarCorreo", Name = "solicitaracceso_validarcorreo")]
        [AllowAnonymous]
        public async Task<ActionResult> ValidarCodigo(VerificarCodigoValidacionParameterVM model)
        {

            ActionResponse actionResponse = new ActionResponse();
            actionResponse.ListActionListResponse = new List<ActionErrorResponse>();
            actionResponse.Codigo = -3;




            if (ModelState.IsValid)
            {

                SolicitarAccesoParameterVM solicitarAccesoVM = new SolicitarAccesoParameterVM();
                string strSesion = HttpContext.Session.GetString("SesionSolicitarAcceso");

                solicitarAccesoVM = JsonConvert.DeserializeObject<SolicitarAccesoParameterVM>(strSesion);
                solicitarAccesoVM.ImagenGrupoTrans =$"{this.GetUriHost()}/{_configuration[Utilitario.Constante.ConfiguracionConstante.Imagen.ImagenGrupo]}";


                model.CodigoTipoDocumento = solicitarAccesoVM.TipoDocumento;
                model.NumeroDocumento = solicitarAccesoVM.NumeroDocumento;
                model.CodigoVerificacion = model.CodigoVerificacion;

                var verificarExistenciadeToekn = await _serviceAcceso.VerificarCodigoValidacion(model);

                if (verificarExistenciadeToekn.CodigoResultado == 0)
                {

                    var listTipoDocumnentoResult = await _serviceAcceso.SolicitarAcceso(solicitarAccesoVM);


                    if (listTipoDocumnentoResult.CodigoResultado == 0)
                    {

                        actionResponse.Mensaje = "Ok";
                        actionResponse.Codigo = 0;
                    }
                    else
                    {
                        actionResponse.Mensaje = "Ocurrio un error interno, intentar más tarde por favor.";
                        actionResponse.Codigo = -1;
                    }

                }
                else
                {
                    actionResponse.Mensaje = verificarExistenciadeToekn.MensajeResultado;
                    actionResponse.Codigo = verificarExistenciadeToekn.CodigoResultado;
                }



            }
            else {
                var erroneousFields = ModelState.Where(ms => ms.Value.Errors.Any())
                                       .Select(x => new { x.Key, x.Value.Errors });

                foreach (var erroneousField in erroneousFields)
                {
                    var fieldKey = erroneousField.Key;
                    var fieldErrors = string.Join(" | ", erroneousField.Errors.Select(e => e.ErrorMessage));

                    actionResponse.ListActionListResponse.Add(new ActionErrorResponse()
                    {
                        Mensaje = fieldErrors,
                        NombreCampo = fieldKey
                    }); ;
                }



                actionResponse.Codigo = -1;
                actionResponse.Mensaje = "Por favor ingresar los campos requeridos.";
            }
                




            return new JsonResult(actionResponse);



        }

        [HttpGet("ReenviarCodigo", Name = "solicitaracceso_reenviarcodigo")]
        [AllowAnonymous]
        public async Task<ActionResult> ReenviarCodigo()
        {
            ActionResponse actionResponse = new ActionResponse();
            actionResponse.ListActionListResponse = new List<ActionErrorResponse>();
            actionResponse.Codigo = -3;

            SolicitarAccesoParameterVM solicitarAccesoVM = new SolicitarAccesoParameterVM();
            string strSesion = HttpContext.Session.GetString("SesionSolicitarAcceso");

            solicitarAccesoVM = JsonConvert.DeserializeObject<SolicitarAccesoParameterVM>(strSesion);

            CodigoGeneradoValidacionParameterVM codigoGeneradoValidacionParameterVM = new CodigoGeneradoValidacionParameterVM();
            codigoGeneradoValidacionParameterVM.CodigoTipoDocumento = solicitarAccesoVM.TipoDocumento;
            codigoGeneradoValidacionParameterVM.NumeroDocumento = solicitarAccesoVM.NumeroDocumento;
            codigoGeneradoValidacionParameterVM.Correo = solicitarAccesoVM.Correo;
            codigoGeneradoValidacionParameterVM.Nombres = solicitarAccesoVM.RepresentaLegalNombre;
            var codigoGenerado = await _serviceAcceso.GenerarCodigoVerificacion(codigoGeneradoValidacionParameterVM);

            if (codigoGenerado.CodigoResultado == 0)
            {

                actionResponse.Mensaje = "Reenviado con éxito";
                actionResponse.Codigo = 0;

            }
            else
            {
                actionResponse.Mensaje = codigoGenerado.MensajeResultado;
                actionResponse.Codigo = codigoGenerado.CodigoResultado;
            }

   

            return new JsonResult(actionResponse);
        }
    }
}
