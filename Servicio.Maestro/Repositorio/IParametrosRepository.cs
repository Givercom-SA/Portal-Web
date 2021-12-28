using Servicio.Maestro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Maestro.Repositorio
{
    public interface IParametrosRepository
    {
        public ListaParametroResult ObtenerParametroPorIdPadre(int idParam);

        public ListaDocumentoTipoEntidadResult ObtenerDocumentoPorTipoEntidad(ListarDocumentoTipoEntidadParameter listTiposEntidad);

        public ListaCorreosResult ObtenerCorreosPorPerfil(int _perfil);
    }
}
