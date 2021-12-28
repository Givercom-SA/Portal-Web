using Servicio.Acceso.Models;
using Servicio.Acceso.Models.LoginUsuario;
using Servicio.Acceso.Models.SolicitarAcceso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Acceso.Repositorio
{
    public interface ICodigoGeneradoValidacionRepository
    {
        public CodigoGeneradoValidacionResult GenerarCodigoValidacion(CodigoGeneradoValidacionParameter parameter);

        public VerificarCodigoValidacionResult VerificarCodigoValidacion(VerificarCodigoValidacionParameter parameter);
        public CodigoGeneradoValidacionResult GenerarCodigoValidacionCorreo(CodigoGeneradoValidacionParameter parameter);
        public VerificarCodigoValidacionResult VerificarCodigoValidacionCorreo(VerificarCodigoValidacionParameter parameter);

    }
}
