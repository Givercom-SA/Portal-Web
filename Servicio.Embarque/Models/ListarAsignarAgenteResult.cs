using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Models
{
    public class ListarAsignarAgenteResult: BaseResult
    {
        public List<AsignacionAduanas> AsignacionAduanas { get; set; }
    }

    public class AsignacionAduanas
    {
        public int Id { get; set; }
        public string KEYBLD { get; set; }
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
        public string NombreUsuarioAsigna { get; set; }
        public int IdUsuarioAsignado { get; set; }
        public string NombreUsuarioAsignado { get; set; }
        public string Observacion { get; set; }
        public string Estado { get; set; }
        public string EstadoNombre { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
