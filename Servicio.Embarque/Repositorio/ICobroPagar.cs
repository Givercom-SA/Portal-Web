using AccesoDatos.Utils;
using Servicio.Embarque.Models.CobrosPagar;
using Servicio.Embarque.Models.SolicitudFacturacion;
using Servicio.Embarque.Models.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Servicio.Embarque.Repositorio
{
    public interface ICobroPagarRepository
    {
        public CobrosPagarResult CrearCobrosPagar(CobrosPagarParameter parameter);
        public CobrosPagarResult ActualizarCobrosPagar(CobrosPagarParameter parameter);
        public ListarCobrosPagarResult ObtenerCobrosPagar(string KeyBLD, string BL, string BLNieto, string ConceptoCodigo);
        public CobrosPagarPadreKeyBLResult ObtenerEmbarquePadrePorKeyBl(string KeyBLD);
      

            public ListarUsuarioResult ObtenerUsuariosPorPerfil(int IdPerfil);
        public ListarProvisionFacturacionTerceroResult ObtenerProvisionEmbarqueFacturacionTercerto(ListarProvisionFacturacionTerceroParameter parameter);

   

    }
}
