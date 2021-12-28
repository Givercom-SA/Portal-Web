using Servicio.Acceso.Models;
using Servicio.Acceso.Models.LoginUsuario;
using Servicio.Acceso.Models.SolicitarAcceso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Acceso.Repositorio
{
    public interface ISolicitarAccesoRepository
    {
        public SolicitarAccesoResult RegistrarSolicitudAcceso(SolicitarAccesoParameter parameter);

        public VerificarSolicitudAccesoResult VerificarSolicitudAcceso(VerificarSolicitudAccesoParameter parameter);

    }
}
      
