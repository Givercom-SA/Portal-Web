using AutoMapper;
using Servicio.Solicitud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.ListarEventos;
using ViewModel.Datos.ListarSolicitudes;
using ViewModel.Datos.ListarTipoEntidadSolicitud;
using ViewModel.Datos.Solicitud;
using ViewModel.Datos.SolictudAcceso;

namespace Servicio.Solicitud.Profiles
{
    public class SolicitudProfile : Profile
    {
        public SolicitudProfile()
        {
            CreateMap<ListarSolicitudesResult, ListarSolicitudesVW>()
                .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
                .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD));

            CreateMap<ObjetoSolicitudResult, SolicitudVM>()
                .ForMember(s => s.CodigoSolicitud, o => o.MapFrom(s => s.SOLI_CODIGO.Trim()))
                .ForMember(s => s.FechaRegistro, o => o.MapFrom(s => s.SOLI_FECHA_REGISTRO))
                .ForMember(s => s.RazonSocial, o => o.MapFrom(s => s.SOLI_RAZON_SOCIAL.Trim()))
                .ForMember(s => s.Correo, o => o.MapFrom(s => s.SOLI_CORREO.Trim()))
                .ForMember(s => s.CodigoEstado, o => o.MapFrom(s => s.SOLI_ESTADO_CODIGO.Trim()))
                .ForMember(s => s.NombreEstado, o => o.MapFrom(s => s.SOLI_ESTADO_NOMBRE.Trim()))
                .ForMember(s => s.TipoDocumento, o => o.MapFrom(s => s.SOLI_TIPODOCUMENTO.Trim()))
                .ForMember(s => s.NumeroDocumento, o => o.MapFrom(s => s.SOLI_NUMERO_DOCUMENTO.Trim()))
                .ForMember(s => s.NombreRepresentanteL, o => o.MapFrom(s => s.SOLI_RELEGAL_NOMBRE.Trim()))
                .ForMember(s => s.ApellidoPatRepresentanteL, o => o.MapFrom(s => s.SOLI_RLEGAL_APELLIDO_PATERNO.Trim()))
                .ForMember(s => s.ApellidoMatRepresentanteL, o => o.MapFrom(s => s.SOLI_RLEGAL_APELLIDO_MATERNO.Trim()))
                .ForMember(s => s.AcuerdoCadenaSuministro, o => o.MapFrom(s => (s.SOLI_ACUERDO_SEGUR_CADENA_SUMINI == 1) ? true : false));

            CreateMap<ObjetoDocumentoResult, DocumentosVW>()
                .ForMember(s => s.CodigoDocumento, o => o.MapFrom(s => s.SADO_CODDOCUMENTO.Trim()))
                .ForMember(s => s.NombreDocumento, o => o.MapFrom(s => s.SADO_NOMDOCUMENTO.Trim()))
                .ForMember(s => s.UrlDocumento, o => o.MapFrom(s => s.SADO_URLDOCUMENTO))
                .ForMember(s => s.CodigoEstado, o => o.MapFrom(s => s.SADO_ESTADO))
                .ForMember(s => s.CodigoRechazo, o => o.MapFrom(s => s.SADO_COD_MOTIVO_RECHAZO))
                .ForMember(s => s.NombreRechazo, o => o.MapFrom(s => s.SADO_NOMMOTIVORECHAZO));

            CreateMap<ListarEventosResult, ListarEventosVW>()
                .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
                .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD));

            CreateMap<ObjetoEventosResult, EventoVW>()
                .ForMember(s => s.EventId, o => o.MapFrom(s => s.SAEV_ID))
                .ForMember(s => s.NombreUsuario, o => o.MapFrom(s => s.SAEV_NOMUSUARIO))
                .ForMember(s => s.Descripcion, o => o.MapFrom(s => s.SAEV_DESCRIPCIN.Trim()))
                .ForMember(s => s.FechaEvento, o => o.MapFrom(s => s.SAEV_FECHA_REGISTRO));

            CreateMap<ListarTipoEntidadResult, ListarEntidadesVW>()
                .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
                .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD));

            CreateMap<ObjetoEntidadesResult, TipoEntidadVW>()
                .ForMember(s => s.CodigoEntidad, o => o.MapFrom(s => s.CODIGO_TIPOENTIDAD))
                .ForMember(s => s.NombreEntidad, o => o.MapFrom(s => s.NOMBRE_TIPOENTIDAD));

            CreateMap<ListarSolicitudesParameterVM, ListarSolicitudesParameter>();

            CreateMap<SolicitudAccesoAprobarParameterVM, AprobarSolicitudParameter>();

        }
    }
}
