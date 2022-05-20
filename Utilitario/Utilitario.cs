using System;

namespace Utilitario
{
    public class Utilitario
    {
        public  string MesXNumero(string mesDia)
        {
            if (mesDia == "1")
            {
                return "Enero";
            }
            else if (mesDia == "2")
            {
                return "Febrero";
            }
            else if (mesDia == "3")
            {
                return "Marzo";
            }
            else if (mesDia == "4")
            {
                return "Abril";
            }
            else if (mesDia == "5")
            {
                return "Mayo";
            }
            else if (mesDia == "6")
            {
                return "Junio";
            }
            else if (mesDia == "7")
            {
                return "Julio";
            }
            else if (mesDia == "8")
            {
                return "Agosto";
            }
            else if (mesDia == "9")
            {
                return "Septiembre";
            }
            else if (mesDia == "10")
            {
                return "Octubre";
            }
            else if (mesDia == "11")
            {
                return "Noviembre";
            }
            else if (mesDia == "12")
            {
                return "Dociembre";
            }

            return "";

        }
    }
}
