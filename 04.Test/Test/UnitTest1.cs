using NUnit.Framework;

namespace Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            //Assert.Pass();



            string strContrasenia = new Utilitario.Seguridad.SeguridadCodigo().GenerarCadenaLongitud(6);
            strContrasenia = strContrasenia.ToUpper();
             string datpos=  new Utilitario.Seguridad.Encrypt().GetSHA256("956VGO");


        }

        [Test]
        public void TestSplitArchivo()
        {
            //Assert.Pass();



            string nombreArchivo = "jfloersninaco.pdf";
            string[] listNombreArchivo = nombreArchivo.Split(".");

            if (listNombreArchivo.Length > 0) {
                string soloNombreSinExtension=listNombreArchivo[0];
            }


        }
    }
}