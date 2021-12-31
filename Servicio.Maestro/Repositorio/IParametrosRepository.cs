using Servicio.Maestro.Models;
using Servicio.Maestro.Models.LibroReclamo;
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

        public Servicio.Maestro.Models.LibroReclamo.ListaEmpresasResult ListEmpresasReclamo(ListaEmpresasParameter parametro);

        public Servicio.Maestro.Models.LibroReclamo.ListaUnidadNegocioXEmpresasResult ListarUnidadNegocioXEmpresa(ListaUnidadNegocioXEmpresaParameter parametro);
        public Servicio.Maestro.Models.LibroReclamo.RegistraReclamoResult RegistrarReclamo(RegistrarReclamoParameter parametro);

    }
}
