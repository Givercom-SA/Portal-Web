using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Models
{
    public class AsignarAgenteCrearParameter
    {
        public string KEYBLD { get; set; }
        public string LogoEmpresa { get; set; }
        
        public string NROOT { get; set; }
        public string NROBL { get; set; }
        public string NRORO { get; set; }
        public string EMPRESA { get; set; }
        public string ORIGEN { get; set; }
        public string CONDICION { get; set; }
        public string POL { get; set; }
        public string POD { get; set; }
        public string ETAPOD { get; set; }
        public string EQUIPAMIENTO { get; set; }
        public string MANIFIESTO { get; set; }
        public string COD_LINEA { get; set; }
        public string DES_LINEA { get; set; }
        public string CONSIGNATARIO { get; set; }
        public string COD_INSTRUCCION { get; set; }
        public string DES_INSTRUCCION { get; set; }
        public int IdUsuarioAsigna { get; set; }
        public int IdUsuarioAsignado { get; set; }
        public string CorreoUsuarioAsignado { get; set; }
        public string Observacion { get; set; }
        public string Estado { get; set; }
        public int IdUsuarioCrea { get; set; }
        public int IdUsuarioModifica { get; set; }

        public int IdEntidadAsigna { get; set; }
        public int IdEntidadAsignado { get; set; }

    


    }
    public class AsignarAgenteEstadoParameter
    {
        public int Id { get; set; }
        public string Estado { get; set; }
        public string Observacion { get; set; }
        public int IdUsuarioModifica { get; set; }
        public string LogoEmpresa { get; set; }
    }

    public class AsignarAgenteListarParameter
    {
        public int IdEntidadAsigna { get; set; }
        public int IdEntidadAsignado { get; set; }
        public int IdUsuarioAsigna { get; set; }
        public int IdUsuarioAsignado { get; set; }
        public string Estado { get; set; }
        public string KEYBLD { get; set; }
        public string NROOT { get; set; }
        public string NROBL { get; set; }

    }
}
