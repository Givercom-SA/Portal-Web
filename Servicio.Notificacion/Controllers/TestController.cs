using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Common.BaseServiceController;
using System;
using ViewModel.Common.Request;

namespace Servicio.Notificacion.Controllers
{
    [Route("api/[controller]")]
    public class TestController : BaseController
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("servicio-test")]
        public IActionResult ServicioTest()
        {
            var result = new DataRequestViewModelResponse { Message = "Servicio operativo", Resultado = DataRequestViewModelResponse.ResultadoServicio.Exito, StatusResponse = DateTime.Now.ToString() };
            return Ok(result);
        }

    }
}
