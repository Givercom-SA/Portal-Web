using System;
using System.ComponentModel;

namespace ViewModel.Common.Attributes
{
    public class CustomFileAttributes : Attribute
    {
        private bool campoArchivo;

        private int idColumna;

        private string nombreColumna;

        private bool obligatorio;
        
        private TipoDatos tipoDato;

        private int longitud;

        private bool max;

        private string[] lista = new string[0];

        private int posicionInicial;

        private int posicionFInal;
        private string codigoError;

        public bool CampoArchivo { get => campoArchivo; set => campoArchivo = value; }

        public int IdColumna { get => idColumna; set => idColumna = value; }

        public string NombreColumna { get => nombreColumna; set => nombreColumna = value; }

        public bool Obligatorio { get => obligatorio; set => obligatorio = value; }

        public TipoDatos TipoDato { get => tipoDato; set => tipoDato = value; }
        
        public int Longitud { get => longitud; set => longitud = value; }

        public bool Max { get => max; set => max = value; }

        public String[] Formatos { get => lista; set => lista = value; }

        public int PosicionInicial { get => posicionInicial; set => posicionInicial = value; }

        public int PosicionFInal { get => posicionFInal; set => posicionFInal = value; }

        public string CodigoError { get => codigoError; set => codigoError = value; }

        public enum TipoDatos
        {
            Cadena,
            Numerico,
            NumericoCadena,
            NumericoGrande,
            Monto,  
            MontoCadena,
            IP,
            Lista,
            ListaNumerica,
            Fecha,
            Hora,
            Email,
            MontoDosDecimales,
            Entero,
            FechaCadena,
            CodigoErrorAsociado
        }
    }
}
