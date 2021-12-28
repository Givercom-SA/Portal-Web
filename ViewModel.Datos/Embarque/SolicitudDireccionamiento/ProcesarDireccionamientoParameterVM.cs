using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModel.Datos.Embarque.SolicitudDireccionamiento
{
    public class ProcesarDireccionamientoParameterVM
    {
       public string codSolicitud { get; set; }
        public string CodigoEstado { get; set; }
        public string CodigoMotivoRechazo { get; set; }
        public string nombreImagenEmpresa { get; set; }
        public string NombreCompletoUsuario { get; set; }
        public string CorreoUsuarioOperador { get; set; }
        
        public int idUsuario { get; set; }
        
    }


}
