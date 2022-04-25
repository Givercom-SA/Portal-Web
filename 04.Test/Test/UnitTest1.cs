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
        public void TestContrasenia() {

           string loannis= new Utilitario.Seguridad.Encrypt().GetSHA256("ioannisb");
            string luis = new Utilitario.Seguridad.Encrypt().GetSHA256("luisa");
            string jose = new Utilitario.Seguridad.Encrypt().GetSHA256("Joser");
            string roger = new Utilitario.Seguridad.Encrypt().GetSHA256("rogerc");
            string patricia = new Utilitario.Seguridad.Encrypt().GetSHA256("patriciag");
            string raul = new Utilitario.Seguridad.Encrypt().GetSHA256("raulc");
            string JoseCOnr = new Utilitario.Seguridad.Encrypt().GetSHA256("josec");
            string Marco = new Utilitario.Seguridad.Encrypt().GetSHA256("marcoq");
            string Pilar = new Utilitario.Seguridad.Encrypt().GetSHA256("pilarr");

            string resultado = "";

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