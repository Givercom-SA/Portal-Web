using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.Documento;
using Web.Principal.Model;
using Web.Principal.ServiceConsumer;

namespace Web.Principal.Areas.GestionarAccesos.Controllers
{
    [Area("GestionarAccesos")]
    public class SolicitarAccesoController : Controller
    {


        private readonly ServicioMaestro _serviceMaestro;
        private readonly ServicioSolicitud _serviceSolicitud;

        private readonly IMapper _mapper;

        public SolicitarAccesoController(
            ServicioMaestro serviceMaestro,
            ServicioSolicitud serviceSolicitud,
            IMapper mapper)
        {
            _serviceMaestro = serviceMaestro;
            _serviceSolicitud = serviceSolicitud;
            _mapper = mapper;
        }





        // GET: GestionarAccesosController
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> listarDocumentosPorTipo([FromBody] ListDocumentoTipoEntidadParameterVM model)
        {
            var listaEstado = await _serviceMaestro.ObtenerDocumentoPorTipoEntidad(model);
            return Ok(listaEstado);
        }






        // GET: GestionarAccesosController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: GestionarAccesosController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GestionarAccesosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: GestionarAccesosController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: GestionarAccesosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: GestionarAccesosController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: GestionarAccesosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
