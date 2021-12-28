using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ViewModel.Common.Attributes
{
    public class CustomFileDomain : Attribute
    {

        private TipoArchivo tipoArchivoCarga;
        private Cabecera cabecera;
        private string separador;
        private bool eliminarEspaciosSobrantes = false;
        private string encode;
        //QA-I035 CPU: Para validar tamaño de la fila en un archivo.
        private string tamanio;
        //
        public LeerExcelHasta FormaLecturaExcel { get; set; } = LeerExcelHasta.ElFinal;
        public LeerTextoHasta FormaLecturaTexto { get; set; } = LeerTextoHasta.EncontrarUnaLineaVacia;

        //QA-I035 CPU: Para validar tamaño de la fila en un archivo.
        public string Tamanio
        {
            get { return tamanio; }
            set { tamanio = value; }
        }
        //

        public string Encode
        {
            get { return encode; }
            set { encode = value; }
        }

        public Cabecera CabeceraArchivo
        {
            get { return cabecera; }
            set { cabecera = value; }
        }
        public string SeparadorCampo
        {
            get { return separador; }
            set { separador = value; }
        }
        
        public TipoArchivo TipoArchivoCarga
        {
            get { return tipoArchivoCarga; }
            set { tipoArchivoCarga = value; }
        }

        /// <summary>
        /// Indicador que autoiza eliminar los espacios sobrantes para cada campo que se obtiene de un archivo de texto.
        /// NOTA: A los campos obtenidos de un archivo Excel automatimente se les quita los espacios sobrantes.
        /// </summary>
        public bool EliminarEspaciosSobrantes
        {
            get { return eliminarEspaciosSobrantes; }
            set { eliminarEspaciosSobrantes = value; }
        }

        public enum TipoArchivo
        {
            Excel,
            Texto
        }

        public enum Cabecera
        {
            NoTieneCabecera = 1,
            TieneCabecera = 2
        }

        public enum LeerExcelHasta {

            /// <summary>
            /// Lee el archivo hasta el final del contenido
            /// </summary>
            ElFinal = 1,

            /// <summary>
            /// Lee Hasta Encontrar una Linea cuya primera celda sea vacia
            /// </summary>
            EncontrarPrimeraCeldaVacia = 2,

            /// <summary>
            /// NO IMPLEMENTADO
            /// </summary>
            EncontrarUnaLineaVacia = 3 //Falta Implementar
            
        }

        public enum LeerTextoHasta
        {
            /// <summary>
            /// Lee el archivo hasta el final del contenido
            /// </summary>
            ElFinal = 1,

            /// <summary>
            /// Lee hasta econtrar una Linea sin caracteres(vacia)
            /// </summary>
            EncontrarUnaLineaVacia = 2,

            /// <summary>
            /// Lee hasta encontrar una Linea solo con espacios en blanco
            /// </summary>
            EncontrarUnaLineaEnBlanco = 3
        }
    }
}
