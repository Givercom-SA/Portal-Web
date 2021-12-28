using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Servicio.Embarque.Models;
using Servicio.Embarque.Models.SolicitudFacturacionTerceros;

namespace Servicio.Embarque.Repositorio
{
    public interface IAsignarAgenteRepository
    {
        public ListarUsuarioEntidadResult ObtenerUsuariosEntidad(int IdPerfil, int IdUsuarioExcluir);

        public ListarAsignarAgenteResult ObtenerListaAsignacion(AsignarAgenteListarParameter parameter);
        public ListarAsignarAgenteResult ObtenerListaAsignados(AsignarAgenteListarParameter parameter);
        public AsignarAgenteResult AsignarAgenteCrear(AsignarAgenteCrearParameter parameter);
        public AsignarAgenteResult AsignarAgenteCambiarEstado(AsignarAgenteEstadoParameter parameter);
        public AsignarAgenteDetalle AsignarAgenteDetalle(int Id);

        public AsignarAgenteHistorialResult AsignarAgenteHistorial(int IdAsignacionAduana);
        public RegistrarFacturacionTerceroResult RegistrarFacturacionTercero(RegistrarFacturacionTerceroParameter parameter);
        public ListarFacturacionTerceroResult ObtenerFacutracionTerceros(ListarFacturacionTerceroParameter parameter);
        public RegistrarFacturacionTerceroResult ActualizarFacturacionTercero(RegistrarFacturacionTerceroParameter parameter);
        public ListarFacturacionTerceroDetalleResult ObtenerFacutracionTerceroDetalle(int IdFacturacionTercero);

        public ListarFacturacionTerceroDetallePorKeyblResult ObtenerFacutracionTerceroDetallePorKeybl(string Keybl);

        
        public VerificarAsignacionAgenteAduanasResult VerificarAsignarAgenteAduanas(VerificarAsignacionAgenteAduanasParameter parameter);
    }
}
