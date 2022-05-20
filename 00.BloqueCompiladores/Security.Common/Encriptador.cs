using HashidsNet;
using System;
using System.Linq;
using System.Net;

namespace Security.Common
{
    public  class Encriptador
    {
        private static string salt = "Pr0y3C70_@FpNEt";
        private static int minimunLength = 20;

        private static readonly Lazy<Encriptador> lazy =
        new Lazy<Encriptador>(() => new Encriptador());

        public static Encriptador Instance { get { return lazy.Value; } }

        //######################################################################################
        //Encriptar Entero
        //######################################################################################
        public  string Encriptar(int number)
        {
            var hashids = new Hashids(salt, minimunLength);
            return hashids.Encode(number);
        }

        public  int Desencriptar(string hash)
        {
            var hashids = new Hashids(salt, minimunLength);
            return hashids.Decode(hash).FirstOrDefault();
        }

        public  string Encriptar(long number)
        {
            var hashids = new Hashids(salt, minimunLength);
            return hashids.EncodeLong(number);
        }

        //######################################################################################
        //Encriptar Texto
        //######################################################################################
        public  string EncriptarTexto(string texto)
        {
            texto = WebUtility.HtmlEncode(texto);
            string hexadecimal = string.Concat(texto.Select(x => ((int)x).ToString("x")));
            var hashids = new Hashids(salt, minimunLength);
            var textoHexaDecimal = hashids.EncodeHex(hexadecimal);
            return textoHexaDecimal;
        }

        public  string DesencriptarTexto(string hash)
        {
            var hashids = new Hashids(salt, minimunLength);
            var textoHexaDecimal = hashids.DecodeHex(hash);

            byte[] bb = Enumerable.Range(0, textoHexaDecimal.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(textoHexaDecimal.Substring(x, 2), 16))
                             .ToArray();

            var texto = System.Text.Encoding.UTF8.GetString(bb);
            texto = WebUtility.HtmlDecode(texto);
            return texto;
        }
    }
}
