using System;
using System.Collections.Generic;
using System.Text;

namespace Email.Common
{
    public interface IEmail
    {
        void Send(string subject, string body, string to , string attachmentFilename = null);

        void Send(string subject, string body, params string[] to);
    }
}
