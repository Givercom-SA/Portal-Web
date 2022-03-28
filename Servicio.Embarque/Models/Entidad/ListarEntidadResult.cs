using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Models.Entidad
{
    public class ListarEntidadResult : BaseResult
    {
        public IEnumerable<EntidadTipo> Entidades { get; set; }
    }


    public class EntidadTipo {

        public long EntidadId { get; set; }

        public string TipoDocumento { get; set; }

        public string NumeroDocumento { get; set; }
        public string RazonSocial { get; set; }

        public string PerfilEntidad { get; set; }
        public string CodigoTipoEntidad { get; set; }
        public int IdPerfil { get; set; }
        public int IdUsuarioEntidadAdmin { get; set; }

    }
}
