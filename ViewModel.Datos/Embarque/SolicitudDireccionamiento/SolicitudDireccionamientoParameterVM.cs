﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModel.Datos.Embarque.SolicitudDireccionamiento
{
    public class SolicitudDireccionamientoParameterVM
    {
        public int IdUsuario { get; set; }
        public int Identidad { get; set; }
        public string KeyBL { get; set; }
        public string NroBL { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string CodigoTaf { get; set; }
        public string RazonSocial { get; set; }
        public string CodAlmacen { get; set; }
        public string CodModalidad { get; set; }
        public string Correo { get; set; }
        public string VencimientoPlazo { get; set; }
        public string FlagDireccionamientoPermanente { get; set; }
        public string Almacen { get; set; }
        public string CantidadCtn { get; set; }
        public string NaveViaje { get; set; }
        public string Consignatario { get; set; }
        public List<DocumentoVM> Documentos { get; set; }
        public string NombreCompleto { get; set; }
        public string IdEntidadSeleccionado { get; set; }

        public string ImagenEmpresaLogo { get; set; }
    }

    public class DocumentoVM {

        public string CodigoDocumento { get; set; }
        public string NombreArchivo { get; set; }
        public string UrlArchivo { get; set; }


    }

}
