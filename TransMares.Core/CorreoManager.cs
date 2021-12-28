using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TransMares.Core
{
    public class CorreoManager
    {
        public string Para { get; set; }
        public List<string> CopiasCuenta { get; set; }
        public string Body { get; set; }
        public string Asunto { get; set; }
        public string UrlArchivoAdjunto { get; set; }

        public List<string> ListaPara { get; set; }

        public List<string> ListaCopiaOculta { get; set; }
        public List<string> ListaArchivos { get; set; }


        public string EmailSender { get; set; }
        public string EmailSenderContrasenia { get; set; }
        public string EmailSenderHost { get; set; }
        public int EmailSenderPuerto { get; set; }
        public bool EmailSenderSSL { get; set; }

        public int Enviar()
        {
            int estatus = 0;


            //string emailSender = "admin.sistemas@tpsac.com.pe";
            //string emailSenderPassword = "Tr4nsm4r3s";
            //string emailSenderHost = "smtp.outlook.office365.com";
            //int emailSenderPort = 587;
            //Boolean emailIsSSL = true;

       
            using (MailMessage _mailmsg = new MailMessage())
            {

                _mailmsg.IsBodyHtml = true;
                _mailmsg.From = new MailAddress(this.EmailSender);
                _mailmsg.Subject = this.Asunto;
                _mailmsg.Body = this.Body;


                if ( this.ListaPara!=null )
                {
                    _mailmsg.To.Clear();

                    if (!string.IsNullOrEmpty(this.Para)) 
                        _mailmsg.To.Add(this.Para);


                    foreach (var item in ListaPara)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            _mailmsg.To.Add(item);
                        }
                    }
                        
                }
                else
                {
                    _mailmsg.To.Add(this.Para);
                }

                if (this.CopiasCuenta!=null) {
                    foreach (var item in this.CopiasCuenta)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            _mailmsg.CC.Add(item);
                        }
                    }
                        
                }

                if (this.ListaCopiaOculta != null)
                {
                    foreach (var item in this.ListaCopiaOculta)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            _mailmsg.Bcc.Add(item);
                        }
                    }

                }


                if (!string.IsNullOrEmpty(UrlArchivoAdjunto))
                {
                    var att = new Attachment(UrlArchivoAdjunto);
                    _mailmsg.Attachments.Add(att);
                }

                if (this.ListaArchivos!=null)
                {
                    foreach (var itemFile in ListaArchivos)
                    {
                        if (!string.IsNullOrEmpty(itemFile)) {
                            var att = new Attachment(itemFile);
                            _mailmsg.Attachments.Add(att);
                        }
                    }
                }

                SmtpClient _smtp = new SmtpClient();
                _smtp.Host = this.EmailSenderHost;
                _smtp.Port = this.EmailSenderPuerto;
                _smtp.EnableSsl = this.EmailSenderSSL;
                NetworkCredential _network = new NetworkCredential(this.EmailSender, this.EmailSenderContrasenia);
                _smtp.Credentials = _network;

                 _smtp.Send(_mailmsg);


            }

            return estatus;

        }
    }
}

