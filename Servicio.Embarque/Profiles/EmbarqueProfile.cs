using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Servicio.Embarque.Models;
using Servicio.Embarque.Models.SolicitudFacturacionTerceros;
using Servicio.Embarque.Models.SolicitudDireccionamiento;
using Servicio.Embarque.Models.GestionarMemo;
using ViewModel.Datos.Embarque.AsignarAgente;
using ViewModel.Datos.Embarque.CobroPendienteFacturar;
using ViewModel.Datos.Embarque.SolicitudFacturacionTercero;
using ViewModel.Datos.Embarque.SolicitudDireccionamiento;
using ViewModel.Datos.ListaNotificacionesArribo;
using ViewModel.Datos.Embarque.GestionarMemo;
using Servicio.Embarque.Models.Usuario;
using ViewModel.Datos.Embarque.Usuario;
using ViewModel.Datos.Embarque.CobrosPagar;
using Servicio.Embarque.Models.CobrosPagar;
using ViewModel.Datos.ListaExpressRelease;
using Servicio.Embarque.Models.SolicitudFacturacion;
using ViewModel.Datos.Embarque.SolicitudFacturacion;


namespace Servicio.Embarque.Profiles
{
    public class EmbarqueProfile : Profile
    {
        public EmbarqueProfile()
        {
            CreateMap<AsignacionAduanas, AsignacionAduanasVM>();

            CreateMap<ListarAsignarAgenteResult, ListarAsignarAgenteResultVM>()
            .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
            .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD))
            .ForMember(s => s.AsignacionAduanas, o => o.MapFrom(s => s.AsignacionAduanas));

            CreateMap<UsuarioEntidad, UsuarioEntidadVM>();

            CreateMap<ListarUsuarioEntidadResult, ListarUsuarioEntidadResultVM>()
            .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
            .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD))
            .ForMember(s => s.Usuarios, o => o.MapFrom(s => s.Usuarios));

            CreateMap<AsignarAgenteResult, AsignarAgenteResultVM>()
            .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
            .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD));

            CreateMap<AsignarAgenteCrearParameter, AsignarAgenteCrearParameterVM>();
            CreateMap<AsignarAgenteEstadoParameter, AsignarAgenteEstadoParameterVM>();
            CreateMap<AsignarAgenteListarParameter, AsignarAgenteListarParameterVM>();

            CreateMap<ListarSolicitudesMemoParameterVM, ListarSolicitudesMemoParameter>();

            CreateMap<ListarProvisionFacturacionTerceroParameterVM, ListarProvisionFacturacionTerceroParameter>()
                .ForMember(s => s.KeyBl, o => o.MapFrom(s => s.KeyBl))
            .ForMember(s => s.Provision, o => o.MapFrom(s => s.Provision)) ;
            CreateMap<ProvisionVM, Provision>();
            CreateMap<ListarProvisionFacturacionTerceroResult, ListarProvisionFacturacionTerceroResultVM>()
                .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
            .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD))
            .ForMember(s => s.PrivisionFacturacionTercero, o => o.MapFrom(s => s.PrivisionFacturacionTercero))
            ;
            CreateMap<ProvisionFacturacionTercero, ProvisionFacturacionTerceroVM>();
            CreateMap<EventosResult, EventosResultVM>();
            CreateMap<SolicitudResult, SolicitudResultVM>();
            CreateMap<ListarDocumentoResult, ListarDocumentoResultVM>();
            CreateMap<DocumentoResult, DocumentoResultVM>();
            CreateMap<Documento, DocumentoVM>();
            CreateMap<ListarEventosResult, ListarEventosResultVM>();
            CreateMap<ListarSolicitudesResult, ViewModel.Datos.Embarque.SolicitudDireccionamiento.ListarSolicitudesResultVM>();
            CreateMap<SolicitudDireccionamientoParameter, SolicitudDireccionamientoParameterVM>();
            CreateMap<AsignarAgenteHistorialResult, AsignarAgenteHistorialResultVM>();
            CreateMap<AgenteAduanasHistorial, AgenteAduanasHistorialVM>();


            CreateMap<EventosResult, EventosResultVM>();
            CreateMap<SolicitudDireccionamientoResult, SolicitudDireccionamientoResultVM>()
            .ForMember(s => s.IN_IDSOLICITUD, o => o.MapFrom(s => s.IN_IDSOLICITUD))
            .ForMember(s => s.VH_CODSOLICITUD, o => o.MapFrom(s => s.VH_CODSOLICITUD))
            .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
            .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD));

            CreateMap<NotificacionesMemo, NotificacionesMemoVM>();
            CreateMap<NotificacionesMemoResult, NotificacionesMemoResultVM>();
            CreateMap<NotificacionMemoParameter, NotificacionMemoParameterVM>().ReverseMap();

            CreateMap<DocumentoDireccionamientoResult, DocumentoDireccionamientoResultVM>()
                 .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
            .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD));

            
            CreateMap<EventosMemoResult, EventosMemoResultVM>();
            CreateMap<SolicitudMemoResult, SolicitudMemoResultVM>();
            CreateMap<ListarDocumentoMemoResult, ListarDocumentoMemoResultVM>();
            CreateMap<DocumentoMemoResult, DocumentoMemoResultVM>();
            CreateMap<DocumentoMemo, DocumentoMemoVM>().ReverseMap();
            CreateMap<ListarEventosMemoResult, ListarEventosMemoResultVM>();
            CreateMap<ListarSolicitudesMemoResult, ListarSolicitudesMemoResultVM>();
            CreateMap<SolicitudMemoParameter, SolicitudMemoParameterVM>().ReverseMap();
            CreateMap<SolicitudMemoEstadoresult, SolicitudMemoEstadoresultVM>().ReverseMap();
            

            CreateMap<ProcesarSolicitudMemoResult, ProcesarSolicitudMemoResultVM>()
                .ForMember(s => s.VH_CODSOLICITUD, o => o.MapFrom(s => s.VH_CODSOLICITUD))
                .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
                .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD));


            CreateMap<DocumentoEstadoMemoResult, SolicitudMemoDocumentoEstadoResultVM>()
           .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
           .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD));

            CreateMap<DocumentoEstadoMemoParameter, SolicitudMemoDocumentoEstadoParameterVM>().ReverseMap();
            CreateMap<DocumentoEstado, DocumentoMemoEstadoVM>().ReverseMap();
            CreateMap<ListarUsuarioResult, ListarUsuarioResultVM>();
            CreateMap<Usuario, UsuarioVM>();

            CreateMap<ListarFacturacionTerceroParameterVM, ListarFacturacionTerceroParameter>()
            .ForMember(s => s.EMFT_ENTIDAD, o => o.MapFrom(s => s.Entidad))
            .ForMember(s => s.EMFT_EMBARQUE_KEYBL, o => o.MapFrom(s => s.EmbarqueKeyBL))
            .ForMember(s => s.EMFT_EMBARQUE_NROBL, o => o.MapFrom(s => s.EmbarqueNroBL))
            .ForMember(s => s.EMFT_CLIENTE, o => o.MapFrom(s => s.Cliente))
            .ForMember(s => s.EMFT_CLIENTE_NRODOC, o => o.MapFrom(s => s.NroDocumento ))
            .ForMember(s => s.EMFT_ESTADO, o => o.MapFrom(s => s.Estado))
            .ForMember(s => s.IdEntidad, o => o.MapFrom(s => s.IdEntidad));

            CreateMap<ListarFacturacionTerceroDetalleResult, ListarFacturacionTerceroDetalleResultVM>().ReverseMap();
            CreateMap<ListarFacturacionTerceroDetallePorKeyblResult, ListarFacturacionTerceroDetallePorKeyblResultVM>().ReverseMap();
            CreateMap<SolicitudFacturacionTerceroDetalle, SolicitudFacturacionTerceroDetalleVM>().ReverseMap();
            CreateMap<FacturacionTerceroHistorial, FacturacionTerceroHistorialVM>().ReverseMap();

            CreateMap<SolicitarFacturacionParameterVM, SolicitarFacturacionParameter>()
            .ForMember(s => s.CobrosPendientesCliente, o => o.MapFrom(s => s.CobrosPendientesCliente));
            CreateMap<CobroClienteVM, CobroCliente>();


            CreateMap<SolicitarFacturacionResult, SolicitarFacturacionResultVM>()
                .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
                .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD))
                .ForMember(s => s.CodigoSolicitud, o => o.MapFrom(s => s.CodigoSolicitud))
                ;

            CreateMap<CobrosPagarPadreKeyBLResult, ListarCobrosPagarPadreBeyBlResultVM>()
                .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
                .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD))
                .ForMember(s => s.EmbarquePadreKeyBl, o => o.MapFrom(s => s.EmbarquePadreKeyBl))
                ;

            CreateMap<EmbarqueCobroPadre, EmbarqueCobroPadreVM>().ReverseMap();

            CreateMap<ListarSolicitudFacturacionBandejaResult, ListarSolicitudFacturacionBandejaResultVM>()
                .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
                .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD));

            CreateMap<LeerSolicitudFacturacionBandejaResult, LeerSolicitudFacturacionBandejaResultVM>()
                .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
                .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD));

            CreateMap<ListarSolicitudFacturacionBandejaParameter, ListarSolicitudFacturacionBandejaParameterVM>().ReverseMap();
            CreateMap<LeerSolicitudFacturacionBandejaParameter, LeerSolicitudFacturacionBandejaParameterVM>().ReverseMap();

            CreateMap<SolicitarFacturacionEstadoParameter, SolicitarFacturacionEstadoParameterVM>().ReverseMap();

            CreateMap<SolicitarFacturacionEstadoResult, SolicitarFacturacionEstadoResultVM>()
                .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
                .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD));


            CreateMap<LeerSolicitudFacturacionKeyBlResult, LeerSolicitudFacturacionKeyBlResultVM>()
            .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
            .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD))
            .ForMember(s => s.SolicitudFacturaciones, o => o.MapFrom(s => s.SolicitudFacturaciones))
            ;
            
            CreateMap<SolicitudFacturacion, SolicitudFacturacionVM>().ReverseMap();
            CreateMap<SolicitudFacturacionDetalle, SolicitudFacturacionDetalleVM>().ReverseMap();
            CreateMap<EventoSolicitudFacturacion, EventoSolicitudFacturacionVM>().ReverseMap();
            
            CreateMap<RegistrarFacturacionTerceroParameterVM, RegistrarFacturacionTerceroParameter>()
            .ForMember(s => s.EMFT_ID, o => o.MapFrom(s => s.Id))
            .ForMember(s => s.CobrosPendientesEmbarque, o => o.MapFrom(s => s.CobrosPendientesEmbarque))
            .ForMember(s => s.EMFT_ARCHIVO, o => o.MapFrom(s => s.Archivo))
            .ForMember(s => s.EMFT_CLIENTE_NOMBRE, o => o.MapFrom(s => s.ClienteNombres))
            .ForMember(s => s.EMFT_CLIENTE_NRODOC, o => o.MapFrom(s => s.ClienteNroDocumeto))
            .ForMember(s => s.EMFT_CODIGO_CLIENTE, o => o.MapFrom(s => s.CodigoCliente))
            .ForMember(s => s.EMFT_EMBARQUE_KEYBL, o => o.MapFrom(s => s.EmbarqueKeyBL))
            .ForMember(s => s.EMFT_EMBARQUE_NROBL, o => o.MapFrom(s => s.EmbarqueNroBL))
            .ForMember(s => s.EMFT_IDENTIDAD, o => o.MapFrom(s => s.IdEntidad))
            .ForMember(s => s.EMFT_IDUSUARIO, o => o.MapFrom(s => s.IdUsuario))
            .ForMember(s => s.EMFT_ESTADO, o => o.MapFrom(s => s.Estado))
            .ForMember(s => s.USU_CORREO, o => o.MapFrom(s => s.Correo))
            .ForMember(s => s.IdUsuarioEvalua, o => o.MapFrom(s => s.IdUsuarioEvalua))
            .ForMember(s => s.EMFT_IDUSUARIO_CREA, o => o.MapFrom(s => s.IdUsuarioCrea));


            CreateMap<CobrosPendienteEmbarqueVM, CobroPendietenEmbarque>()
            .ForMember(s => s.ConceptoCodigo, o => o.MapFrom(s => s.ConceptoCodigo))
            .ForMember(s => s.ConceptoCodigoDescripcion, o => o.MapFrom(s => s.ConceptoCodigoDescripcion))
            .ForMember(s => s.Descripcion, o => o.MapFrom(s => s.Descripcion))
            .ForMember(s => s.FlagAsignacion, o => o.MapFrom(s => s.FlagAsignacion))
            .ForMember(s => s.Igv, o => o.MapFrom(s => s.Igv))
            .ForMember(s => s.Importe, o => o.MapFrom(s => s.Importe))
            .ForMember(s => s.Moneda, o => o.MapFrom(s => s.Moneda))
            .ForMember(s => s.RubroCodigo, o => o.MapFrom(s => s.RubroCodigo))
            .ForMember(s => s.Total, o => o.MapFrom(s => s.Total))
            .ForMember(s => s.ID, o => o.MapFrom(s => s.ID));

            CreateMap<RegistrarFacturacionTerceroResult, RegistrarFacturacionTerceroResultVM>()
            .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
            .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD))
            .ForMember(s => s.RazonSocialNombres, o => o.MapFrom(s => s.RazonSocialNombres))
            ;

            CreateMap<ListarFacturacionTerceroResult, ListarFacturacionTerceroResultVM>()
            .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
            .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD))
            .ForMember(s => s.SolicituresFacturacionTereceros, o => o.MapFrom(s => s.SolicitudesFacturacionTerceros));


            CreateMap<Models.SolicitudFacturacionTerceros.SolicitudFacturacionTercero, SolicitudFacturacionTercerosVM>()
            .ForMember(s => s.Codigo, o => o.MapFrom(s => s.EMFT_CODIGO))
            .ForMember(s => s.IdFacturacionTercero, o => o.MapFrom(s => s.EMFT_ID))
            .ForMember(s => s.Archivo, o => o.MapFrom(s => s.EMFT_ARCHIVO))
            .ForMember(s => s.ClienteNombres, o => o.MapFrom(s => s.EMFT_CLIENTE_NOMBRE))
            .ForMember(s => s.ClienteNroDocumeto, o => o.MapFrom(s => s.EMFT_CLIENTE_NRODOC))
            .ForMember(s => s.CobrosPendientesEmbarque, o => o.MapFrom(s => s.CobrosPendientesEmbarque))
            .ForMember(s => s.CodigoCliente, o => o.MapFrom(s => s.EMFT_CODIGO_CLIENTE))
            .ForMember(s => s.EmbarqueKeyBL, o => o.MapFrom(s => s.EMFT_EMBARQUE_KEYBL))
            .ForMember(s => s.EmbarqueNroBL, o => o.MapFrom(s => s.EMFT_EMBARQUE_NROBL))
            .ForMember(s => s.EntidadDatos, o => o.MapFrom(s => s.ENTIDAD_DATOS))
            .ForMember(s => s.IdEntidad, o => o.MapFrom(s => s.EMFT_IDENTIDAD))
            .ForMember(s => s.IdUsuarioCrea, o => o.MapFrom(s => s.EMFT_IDUSUARIO_CREA))
            .ForMember(s => s.Estado, o => o.MapFrom(s => s.EMFT_ESTADO))
            .ForMember(s => s.EstadoNombre, o => o.MapFrom(s => s.EMFT_ESTADO_NOMBRE))
            .ForMember(s => s.Correo, o => o.MapFrom(s => s.USU_CORREO))
            .ForMember(s => s.Usuario, o => o.MapFrom(s => s.USU_NOMBRES))
            .ForMember(s => s.FechaRegistro, o => o.MapFrom(s => s.EMFT_FECHA_REGISTRO));


            CreateMap<ListarNotificacionesPendientesResult, ListarNotificacionesPendientesVW>()
            .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
            .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD));

            CreateMap<NotificacionesPendientesResult, NotificacionesPendientesVM>()
            .ForMember(s => s.KeyBld, o => o.MapFrom(s => s.NOTARR_KEYBLD))
            .ForMember(s => s.tipoDocumento, o => o.MapFrom(s => s.NOTARR_TIPO_DOCUMENTO));

            CreateMap<CobrosPagarParameter, CobrosPagarParameterVM>().ReverseMap();
            CreateMap<CobrosPagarDetalle, CobrosPagarDetalleVM>().ReverseMap();
            CreateMap<CobrosPagarResult, CobrosPagarResultVM>().ReverseMap();
            CreateMap<ListarCobrosPagarResult, ListarCobrosPagarResultVM>().ReverseMap();
            CreateMap<CobrosPagar, CobrosPagarVM>().ReverseMap();


            CreateMap<VerificarAsignacionAgenteAduanasParameterVM, VerificarAsignacionAgenteAduanasParameter>();



            CreateMap<VerificarAsignacionAgenteAduanasResult, VerificarAsignacionAgenteAduanasResultVM>()
            .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
            .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD));


         CreateMap<ListaExpressReleaseAceptadasResult, ListaExpressReleaseAceptadasVW>()
            .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
            .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD));

            CreateMap<ExpressReleaseAceptadaResult, ExpressReleaseAceptada>()
            .ForMember(s => s.KeyBl, o => o.MapFrom(s => s.AER_KEYBD))
            .ForMember(s => s.NroBl, o => o.MapFrom(s => s.AER_NROBL));
        }

    }
}
