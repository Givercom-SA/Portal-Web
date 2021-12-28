using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Email.Common
{
    public class EmailSMTP : IEmail
    {
        private readonly string SMTP_SERVER;
        private readonly int SMTP_PORT;
        private readonly bool SMTP_SSL;
        private readonly string USER;
        private readonly string PASSWORD;
        private readonly string SENDER_MAIL;

        public EmailSMTP(string smtpServer, int port, bool ssl, string user, string password, string sender)
        {
            this.SMTP_SERVER = smtpServer;
            this.SMTP_PORT = port;
            this.SMTP_SSL = ssl;
            this.USER = user;
            this.PASSWORD = password;
            this.SENDER_MAIL = sender;
        }

        public void Send(string subject, string body, string to, string attachmentFilename = null)
        {
            SmtpClient client = new SmtpClient(this.SMTP_SERVER);
            client.Port = SMTP_PORT;
            client.EnableSsl = SMTP_SSL;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(this.USER, this.PASSWORD);

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(this.SENDER_MAIL);
            mailMessage.To.Add(to);
            mailMessage.Body = body;
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;

            if (attachmentFilename != null)
            {
                Attachment attachment = new Attachment(attachmentFilename, MediaTypeNames.Application.Octet);
                ContentDisposition disposition = attachment.ContentDisposition;
                disposition.CreationDate = File.GetCreationTime(attachmentFilename);
                disposition.ModificationDate = File.GetLastWriteTime(attachmentFilename);
                disposition.ReadDate = File.GetLastAccessTime(attachmentFilename);
                disposition.FileName = Path.GetFileName(attachmentFilename);
                disposition.Size = new FileInfo(attachmentFilename).Length;
                disposition.DispositionType = DispositionTypeNames.Attachment;
                mailMessage.Attachments.Add(attachment);
            }

            client.SendAsync(mailMessage, Guid.NewGuid());
        }

        public void Send(string subject, string body, params string[] to)
        {
            throw new NotImplementedException();
        }
    }
}
