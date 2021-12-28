using Autofac;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Security.Common
{
    public class Encrypter : IEncrypter
    {
        private readonly ILifetimeScope container;
        private readonly ILogger<Encrypter> logger;
        private string password;

        public Encrypter(ILifetimeScope container, ILogger<Encrypter> logger, string password)
        {
            this.container = container;
            this.logger = logger;
            this.password = password;
        }

        public string Encrypt(string textToEncrypt)
        {
            using (var scopre = container.BeginLifetimeScope())
            {
                try
                {
                    var encrypt1 = DecryptUsernamePassword(textToEncrypt);
                    var encrypt2 = DecryptUsernamePassword(encrypt1);

                    return DecryptUsernamePassword(encrypt2);
                }
                catch (Exception e)
                {
                    e = Unwrap(e);
                    logger.LogError(e, e.Message);
                    throw new Exception("Se ha producido un error");
                }
            }
        }

        public string Decrypt(string encryptedText)
        {
            var text = DecryptUsernamePassword(encryptedText);
            return text;
        }
        
        private string DecryptUsernamePassword(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
            {
                return cipherText;
            }

            byte[] salt = new byte[]
            {
                (byte)0xc7,
                (byte)0x73,
                (byte)0x21,
                (byte)0x8c,
                (byte)0x7e,
                (byte)0xc8,
                (byte)0xee,
                (byte)0x99
            };

            PKCSKeyGenerator kp = new PKCSKeyGenerator(this.password, salt, 20, 1);

            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, kp.Encryptor, CryptoStreamMode.Write);

            byte[] plainBytes = Encoding.UTF8.GetBytes(cipherText);

            // Encrypt the input textToEncrypt string
            cryptoStream.Write(plainBytes, 0, plainBytes.Length);

            // Complete the encryption process
            cryptoStream.FlushFinalBlock();

            // Convert the encrypted data from a MemoryStream to a byte array
            byte[] cipherBytes = memoryStream.ToArray();

            memoryStream.Close();
            cryptoStream.Close();

            // Convert the encrypted byte array to a base64 encoded string
            return Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);
        }

        private Exception Unwrap(Exception ex)
        {
            while (null != ex.InnerException)
            {
                ex = ex.InnerException;
            }

            return ex;
        }
    }
}
