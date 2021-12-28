using System;
using System.Collections.Generic;
using System.Text;

namespace Security.Common
{
    public interface IEncrypter
    {
        string Encrypt(string textToEncrypt);
        string Decrypt(string encryptedText);
    }
}
