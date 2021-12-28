using Batch.Correo.Model;
using BusinessLogic.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MSMQ.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Batch.Correo.BusinessLogic
{
    public class EnvioCorreoLogic : IBusinessLogic
    {
        public EnvioCorreoResult EnviarCorreo(EnvioCorreoParameter parameter )
        {
            EnvioCorreoResult respuesta = new EnvioCorreoResult();
            respuesta.Estado = "0";

            try
            {
             
                MessageQueue messageQueue = new MSMQ.Messaging.MessageQueue(parameter.cola);
                messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(RequestMessage) });
                Message msg = messageQueue.PeekById(parameter.id);
                RequestMessage requestMessageResult = msg.Body as RequestMessage;


                TransMares.Core.CorreoManager correoManager = new TransMares.Core.CorreoManager();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                correoManager.Body = requestMessageResult.Contenido;
                correoManager.Para = requestMessageResult.Correo;
                correoManager.Asunto = requestMessageResult.Asunto;

                respuesta.Correo = requestMessageResult.Correo;
                respuesta.Asunto = requestMessageResult.Asunto;

                if (requestMessageResult.Archivos != null)
                    correoManager.ListaArchivos = requestMessageResult.Archivos.ToList();

                if (requestMessageResult.Cuentas != null)
                    correoManager.ListaPara = requestMessageResult.Cuentas.ToList();


                if (requestMessageResult.CopiaCuentas != null)
                    correoManager.CopiasCuenta = requestMessageResult.CopiaCuentas.ToList();

                if (!string.IsNullOrEmpty(parameter.CopiaOculta))
                {
                    string[] listCopiadOculta = parameter.CopiaOculta.Split(";");
                    if (listCopiadOculta.Count() > 0)
                    {
                        correoManager.ListaCopiaOculta = listCopiadOculta.ToList();
                    }


                }

     


                correoManager.EmailSender = parameter.EmailSender;
                correoManager.EmailSenderContrasenia = parameter.EmailSenderContrasenia;
                correoManager.EmailSenderHost = parameter.EmailSenderHost;
                correoManager.EmailSenderPuerto = parameter.EmailSenderPuerto;
                correoManager.EmailSenderSSL = parameter.EmailSenderSSL;

                correoManager.Enviar();

               

            }
            catch (Exception err)
            {
                respuesta.Estado = "1";
                respuesta.exception = err;
                respuesta.Mensaje = "Error inesperado lectura o envio de correo";
            }

            return respuesta;
        }

        public class EnvioCorreoResult
        {

            public string Mensaje { get; set; }
            public string Correo { get; set; }
            public string Estado { get; set; }
            public string Asunto { get; set; }
            public Exception exception { get; set; }

        }

        public class EnvioCorreoParameter
        {
          
          
            public string id { get; set; }
            public string cola { get; set; }
            public string EmailSender { get; set; }
            public string EmailSenderContrasenia { get; set; }
            public string EmailSenderHost { get; set; }
            public string CopiaOculta { get; set; }
            public int EmailSenderPuerto { get; set; }
            public bool EmailSenderSSL { get; set; }


        }
    }
}

