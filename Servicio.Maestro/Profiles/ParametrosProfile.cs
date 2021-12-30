using AutoMapper;
using Servicio.Maestro.Models;
using Servicio.Maestro.Models.LibroReclamo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.Documento;
using ViewModel.Datos.Entidad;
using ViewModel.Datos.ListaCorreos;
using ViewModel.Datos.Parametros;
using ViewModel.Reclamo;

namespace Servicio.Maestro.Profiles
{
    public class ParametrosProfile : Profile
    {
        public ParametrosProfile()
        {
            CreateMap<ListaParametroResult, ListaParametrosVM>()
                .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
                .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD))
                .ForMember(s => s.ListaParametros, o => o.MapFrom(s => s.ListaParametros));


            CreateMap<ParametroResult, ParametrosVM>()
                .ForMember(s => s.IdParametro, o => o.MapFrom(s => s.PRMT_ID))
                .ForMember(s => s.NombreDescripcion, o => o.MapFrom(s => s.PRMT_NOMBRE))
                .ForMember(s => s.ValorCodigo, o => o.MapFrom(s => s.PRMT_VALOR));


            CreateMap< ListaEmpresasParameterVM, ListaEmpresasParameter>();
            CreateMap<ListaUnidadNegocioXEmpresaParameterVM, ListaUnidadNegocioXEmpresaParameter>();


            CreateMap<ListaDocumentoTipoEntidadResult, ListarDocumentoTipoEntidadVM>()
                .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
                .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD))
                .ForMember(s => s.listarDocumentosTipoEntidad, o => o.MapFrom(s => s.ListaParametros));


            CreateMap<DocumentoTipoEntidadResult, DocumentoTipoEntidadVM>()
                .ForMember(s => s.NombreDocumento, o => o.MapFrom(s => s.PRMT_NOMBRE))
                .ForMember(s => s.CodigoDocumento, o => o.MapFrom(s => s.PRMT_VALOR));


            CreateMap<ListDocumentoTipoEntidadParameterVM, ListarDocumentoTipoEntidadParameter>()
                .ForMember(s => s.TiposEntidad, o => o.MapFrom(s => s.TiposEntidad))
                .ForMember(s => s.BrindaCargaFCL, o => o.MapFrom(s => s.BrindaCargaFCL))
                .ForMember(s => s.AcuerdoSeguridadCadenaSuministro, o => o.MapFrom(s => s.AcuerdoSeguridadCadenaSuministro))
                .ForMember(s => s.SeBrindaAgenciamientodeAduanas, o => o.MapFrom(s => s.SeBrindaAgenciamientodeAduanas));


            CreateMap<TipoEntidadVM, TipoEntidad>()
                .ForMember(s => s.CodigoEntidad, o => o.MapFrom(s => s.CodTipoEntidad));


            CreateMap<ListaCorreosResult, ListaCorreosVW>()
                .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
                .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD))
                .ForMember(s => s.ListaCorreos, o => o.MapFrom(s => s.ListaCorreos));

            CreateMap<ListaEmpresasResult, ListaEmpresasResultVM>()
       .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
       .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD))
       .ForMember(s => s.Empresas, o => o.MapFrom(s => s.Empresas));

            CreateMap<EmpresaReclamo, EmpresasReclamoVM>();

            
            CreateMap<ListaUnidadNegocioXEmpresasResult, ListaUnidadNegocioXEmpresasResultVM>()
       .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
       .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD))
       .ForMember(s => s.UnidadNegociosReclamo, o => o.MapFrom(s => s.UnidadNegociosReclamo))
       .ForMember(s => s.TiposDocumentos, o => o.MapFrom(s => s.TiposDocumentos));

            CreateMap<UnidadNegocioReclamo, UnidadNegocioReclamoVM>();
            CreateMap<TipoDocumentoReclamo, TipoDocumentoReclamoVM>();

        }
    }
}
