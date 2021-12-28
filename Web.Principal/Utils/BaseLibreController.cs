using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Security.Common;
using Service.Common.Logging.Application;
using Service.Common.Logging.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Utilitario.Constante;
using ViewModel.Datos.UsuarioRegistro;

namespace Web.Principal.Utils
{
    public class BaseLibreController : Controller
    {
        private readonly IConfiguration configuration;
        
        private readonly IHttpContextAccessor httpContextAccessor;
        
        private static ILogger _logger = ApplicationLogging.CreateLogger("BaseLibreController");

        private string Uri { get; set; }



        public BaseLibreController(IConfiguration configuration) : base()
        {
            this.configuration = configuration;
        }


 
 

        public BaseLibreController() : base()
        {
        }



        public string GetUriHost() {

            return this.Uri;
           
        
        }

        internal UsuarioRegistroVM usuario { get; set; }
        public ActionResponse ActionResponse { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {

                System.Uri _uri = context.HttpContext.Request.GetUri();

                this.Uri = $"{_uri.Scheme}://{_uri.Authority}";

            }
            catch (Exception err) { 
            
            }
            base.OnActionExecuting(context);

        }




        private bool IsAjaxRequest(HttpContext context)
        {
            if (context.Request == null)
                throw new ArgumentNullException("request");

            if (context.Request.Headers != null)
                return context.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }





        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var data = ViewData["variable"] == null ? "" : ViewData["variable"].ToString();
            ViewData["variable"] = data + "/" + "1";
            if (ObtenerTempData<bool>("TimeSessionExpired"))
            {
                context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.RequestTimeout;
            }
        }

        public void GuardarTempData(string key, object data)
        {
            TempData[key] = JsonConvert.SerializeObject(data);
        }
        public T ObtenerTempData<T>(string key, bool keep = false)
        {
            var data = TempData[key];

            if (keep)
                TempData.Keep(key);

            if (data == null)
                return default(T);

            return JsonConvert.DeserializeObject<T>(data.ToString());
        }
      
    }

}
