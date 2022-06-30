using AutoMapper;
using Servicio.Acceso.Models;
using Servicio.Acceso.Models.SolicitarAcceso;
using Servicio.Acceso.Models.Perfil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.SolictudAcceso;
using ViewModel.Datos.UsuarioRegistro;
using ViewModel.Datos.Perfil;
using ViewModel.Datos.Acceso;
using Servicio.Acceso.Models.LoginUsuario;
using Servicio.Acceso.Models.Menu;
using ViewModel.Datos.Menu;
using Servicio.Acceso.Models.Vista;
using ViewModel.Datos.Vista;

namespace Servicio.Acceso.Profiles
{
    public class AccesosProfile : Profile
    {
        public AccesosProfile()
        {
            CreateMap<UsuarioResult, UsuarioRegistroVM>()
                .ForMember(s => s.IdEntidad, o => o.MapFrom(s => s.ENTI_ID))
                .ForMember(s => s.TipoDocumento, o => o.MapFrom(s => s.ENTI_TIPO_DOCUMENTO))
                .ForMember(s => s.NumeroDocumento, o => o.MapFrom(s => s.ENTI_NUMERO_DOCUMENTO))
                .ForMember(s => s.RazonSocial, o => o.MapFrom(s => s.ENTI_RAZON_SOCIAL))
                .ForMember(s => s.idUsuario, o => o.MapFrom(s => s.USU_ID))
                .ForMember(s => s.NombresUsuario, o => o.MapFrom(s => s.USUA_NOMBRES))
                .ForMember(s => s.ApellidoPaternousuario, o => o.MapFrom(s => s.USUA_APELLIDO_PATERNO))
                .ForMember(s => s.ApellidoMaternoUsuario, o => o.MapFrom(s => s.USUA_APELLIDO_MATERNO))
                .ForMember(s => s.CorreoUsuario, o => o.MapFrom(s => s.USU_CORREO))
                .ForMember(s => s.IdPerfil, o => o.MapFrom(s => s.PEFL_ID))
                .ForMember(s => s.PerfilNombre, o => o.MapFrom(s => s.PEFL_NOMBRE))
                .ForMember(s => s.isCambioClave, o => o.MapFrom(s => s.USUA_CAMBIOCLAVE.Equals(1) ? true : false))
                .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
                .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD))
                .ForMember(s => s.TipoEntidad, o => o.MapFrom(s => s.PERFIL_TIPO_ENTIDAD))
                .ForMember(s => s.Dashboard, o => o.MapFrom(s => s.PEFL_DASHBOARD))
                .ForMember(s => s.TipoPerfil, o => o.MapFrom(s => s.PEFL_TIPO))
                .ForMember(s => s.Accesos, o => o.MapFrom(s => s.ListaAcceso))

                ;

            CreateMap<MenuLogin, MenuLoginVM>();
            CreateMap<PerfilLogin, PerfilLoginVM>();


            CreateMap<ItemAcceso, MenuElementoVM>()
                .ForMember(d => d.MenuId, o => o.MapFrom(s => s.MenuId))
                .ForMember(d => d.Name, o => o.MapFrom(s => string.IsNullOrEmpty(s.Name) ? string.Empty : s.Name.Trim()))
                .ForMember(d => d.HttpArea, o => o.MapFrom(s => string.IsNullOrEmpty(s.HttpArea) ? string.Empty : s.HttpArea.Trim().ToLower()))
                .ForMember(d => d.HttpController, o => o.MapFrom(s => string.IsNullOrEmpty(s.HttpController) ? string.Empty : s.HttpController.Trim().ToLower()))
                .ForMember(d => d.HttpAction, o => o.MapFrom(s => string.IsNullOrEmpty(s.HttpAction) ? string.Empty : s.HttpAction.Trim().ToLower()))
                .ForMember(d => d.HttpMethod, o => o.MapFrom(s => string.IsNullOrEmpty(s.HttpMethod) ? string.Empty : s.HttpMethod.Trim().ToLower()))
                .ForMember(d => d.IsMainForm, o => o.MapFrom(s => s.IsMainForm))
                .ForMember(d => d.ItemType, o => o.MapFrom(s => s.ItemType))
                .ForMember(d => d.NameControlHtml, o => o.MapFrom(s => s.NameControlHtml))
                .ForMember(d => d.ParentId, o => o.MapFrom(s => s.ParentId))
                .ForMember(d => d.Order, o => o.MapFrom(s => s.Order))
                .ForMember(d => d.IdPerfil, o => o.MapFrom(s => s.IdPerfil))
                .ForMember(d => d.IdVistaMenuPerfil, o => o.MapFrom(s => s.IdVistaMenuPerfil))
                .ForMember(d => d.IdVista, o => o.MapFrom(s => s.IdVista))
                .ForMember(d => d.IdVistaMenu, o => o.MapFrom(s => s.IdVistaMenu))
                .ForMember(d => d.Checked, o => o.MapFrom(s => s.Checked))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<CodigoGeneradoValidacionParameterVM, CodigoGeneradoValidacionParameter>()
           .ForMember(s => s.CODTIPO_DOCUMENTO, o => o.MapFrom(s => s.CodigoTipoDocumento))
           .ForMember(s => s.CORREO, o => o.MapFrom(s => s.Correo))
           .ForMember(s => s.NUMERO_DOCUMENTO, o => o.MapFrom(s => s.NumeroDocumento));

            CreateMap<CodigoGeneradoValidacionResult, CodigoGeneradoValidacionResultVM>()
            .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
            .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD));

            CreateMap<VerificarSolicitudAccesoParameterVM, VerificarSolicitudAccesoParameter>()
        .ForMember(s => s.CodigoTipoDocumento, o => o.MapFrom(s => s.CodigoTipoDocumento))
        .ForMember(s => s.Correo, o => o.MapFrom(s => s.Correo))
        .ForMember(s => s.NumeroDocumento, o => o.MapFrom(s => s.NumeroDocumento));

            CreateMap<VerificarSolicitudAccesoResult, VerificarSolicitudAccesoResultVM>()
            .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
            .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD));

            CreateMap<ListarMenuResult, ListarMenusResultVM>()
            .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
            .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD));


            CreateMap<LeerMenuResult, LeerMenusResultVM>()
            .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
            .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD));

            CreateMap<MantenimientoVistaResult, MantenimientoVistaResultVM>()
            .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
            .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD));

            CreateMap<ListarAreaControllerActionResult, ListarAreaControllerActionResultVM>()
            .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
            .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD));


            CreateMap<AreaControllerActionVM, AreaControllerAction>();
            CreateMap<AreaControllerAction, AreaControllerActionVM>();

            CreateMap<MenuVM, Menu>();
            CreateMap<Menu, MenuVM>();

            CreateMap<ListarVistaResult, ListarVistasResultVM>()
            .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
            .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD));

            CreateMap<MantenimientoMenuResult, MantenimientoMenuResultVM>()
            .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
            .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD));


            CreateMap<LeerVistaResult, LeerVistaResultVM>()
            .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
            .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD));


            CreateMap<ListarTodoVistaResult, ListarTodoVistaResultVM>()
            .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
            .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD));

            CreateMap<ListarTodoMenuResult, ListarTodoMenusResultVM>()
           .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
           .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD));


            CreateMap<VistaVM, Vista>();
            CreateMap<Vista, VistaVM>();

            CreateMap<VistaTodoVM, VistaTodo>();
            CreateMap<VistaTodo, VistaTodoVM>();



            CreateMap<VerificarCodigoValidacionParameterVM, VerificarCodigoValidacionParameter>()
              .ForMember(s => s.CODIGO_VERIFICACION, o => o.MapFrom(s => s.CodigoVerificacion))
              .ForMember(s => s.CODTIPO_DOCUMENTO, o => o.MapFrom(s => s.CodigoTipoDocumento))
              .ForMember(s => s.CORREO, o => o.MapFrom(s => s.Correo))
            .ForMember(s => s.NUMERO_DOCUMENTO, o => o.MapFrom(s => s.NumeroDocumento));

            CreateMap<VerificarCodigoValidacionResult, VerificarCodigoValidacionResultVM>()
          .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
          .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD));




            CreateMap<SolicitarAccesoResult, SolicitarAccesoResultVM>()
                .ForMember(s => s.CodigoSolicitudAcceso, o => o.MapFrom(s => s.VH_CODSOLICITUD_ACCESO))
                .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
                .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD))
                ;

            CreateMap<SolicitarAccesoParameterVM, SolicitarAccesoParameter>()
          .ForMember(s => s.SOLI_TIPODOCUMENTO, o => o.MapFrom(s => s.TipoDocumento))
          .ForMember(s => s.SOLI_NUMERO_DOCUMENTO, o => o.MapFrom(s => s.NumeroDocumento))
          .ForMember(s => s.SOLI_RAZON_SOCIAL, o => o.MapFrom(s => s.RazonSocial))
          .ForMember(s => s.SOLI_RELEGAL_NOMBRE, o => o.MapFrom(s => s.RepresentaLegalNombre))
          .ForMember(s => s.SOLI_RLEGAL_APELLIDO_PATERNO, o => o.MapFrom(s => s.RepresentaLegalApellidoPaterno))
          .ForMember(s => s.SOLI_RLEGAL_APELLIDO_MATERNO, o => o.MapFrom(s => s.RepresentaLegalMaterno))
          .ForMember(s => s.SOLI_CORREO, o => o.MapFrom(s => s.Correo))
          .ForMember(s => s.SOLI_ACUERDO_ENDOCE_ELECTRONICO, o => o.MapFrom(s => s.AcuerdoEndoceElectronico))
          .ForMember(s => s.SOLI_BRINDAOPE_CARGA_FCL, o => o.MapFrom(s => s.BrindaOpeCargaFCL))
          .ForMember(s => s.SOLI_ACUERDO_SEGUR_CADENA_SUMINI, o => o.MapFrom(s => s.AcuerdoSeguroCadenaSuministro))
          .ForMember(s => s.SOLI_DECLARA_JURADA_VERACIDAD_INFO, o => o.MapFrom(s => s.DeclaracionJuradaVeracidadInfo))
          .ForMember(s => s.TipoEntidad, o => o.MapFrom(s => s.TipoEntidad))
          .ForMember(s => s.ProcesoFacturacion, o => o.MapFrom(s => s.ProcesoFacturacion))
          .ForMember(s => s.TerminoCondicionGeneralContraTCGC, o => o.MapFrom(s => s.TerminoCondicionGeneralContraTCGC))
          .ForMember(s => s.CodigoSunat, o => o.MapFrom(s => s.CodigoSunat))
          .ForMember(s => s.Documento, o => o.MapFrom(s => s.Documento))
          .ForMember(s => s.SOLI_BRINDA_AGENCIAMIENTO_ADUANA, o => o.MapFrom(s => s.BrindaAgenciamientoAduanas))

          ;

            CreateMap<TipoEntidadVM, TipoEntidad>()
            .ForMember(s => s.CodigoTipoEntidad, o => o.MapFrom(s => s.CodigoTipoEntidad));

            CreateMap<DocumentoVM, Documento>()
           .ForMember(s => s.CodigoDocumento, o => o.MapFrom(s => s.CodigoDocumento))
           .ForMember(s => s.NombreArchivo, o => o.MapFrom(s => s.NombreArchivo))
           .ForMember(s => s.UrlArchivo, o => o.MapFrom(s => s.UrlArchivo))
            ;

            CreateMap<Perfil, PerfilVM>()
                .ForMember(s => s.IdPerfil, o => o.MapFrom(s => s.IdPerfil))
                .ForMember(s => s.Nombre, o => o.MapFrom(s => s.Nombre))
                .ForMember(s => s.Activo, o => o.MapFrom(s => s.Activo))
                .ForMember(s => s.IdSesion, o => o.MapFrom(s => s.IdSesion))
                .ForMember(s => s.IdUsuarioCrea, o => o.MapFrom(s => s.IdUsuarioCrea))
                .ForMember(s => s.IdUsuarioModifica, o => o.MapFrom(s => s.IdUsuarioModifica))
                .ForMember(s => s.FechaRegistro, o => o.MapFrom(s => s.FechaRegistro))
                .ForMember(s => s.FechaModifica, o => o.MapFrom(s => s.FechaModifica))
                .ForMember(s => s.Tipo, o => o.MapFrom(s => s.Tipo))
                .ForMember(s => s.TipoPerfil, o => o.MapFrom(s => s.TipoPerfil))
                .ForMember(s => s.UsuarioCrea, o => o.MapFrom(s => s.UsuarioCrea))
                .ForMember(s => s.UsuarioModifica, o => o.MapFrom(s => s.UsuarioModifica))
                .ForMember(s => s.Menus, o => o.MapFrom(s => s.Menus))
                .ForMember(s => s.VistaMenu, o => o.MapFrom(s => s.VistaMenu))
                .ForMember(s => s.Checked, o => o.MapFrom(s => s.Checked));


            CreateMap<PerfilParameterVM, PerfilParameter>();

            CreateMap<PerfilResult, PerfilResultVM>()
                    .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
                    .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD));

            CreateMap<MenuPerfil, MenuPerfilVM>();
            CreateMap<VistaMenu, VistaMenuVM>();
            CreateMap<VistaMenuVM, VistaMenu>();

            CreateMap<ListarMenusPerfilResult, ListarMenusPerfilResultVM>()
                .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
                .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD))
                .ForMember(s => s.Menus, o => o.MapFrom(s => s.Menus));

            CreateMap<ListarPerfilesResult, ListarPerfilesResultVM>()
                .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
                .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD))
                .ForMember(s => s.Perfiles, o => o.MapFrom(s => s.Perfiles));

            CreateMap<ObtenerPerfilResult, ObtenerPerfilResultVM>()
             .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
             .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD))
             .ForMember(s => s.Perfil, o => o.MapFrom(s => s.Perfil));

            CreateMap<TraerPerfilResult, TraerPerfilResultVM>()
                .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
                .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD))
                .ForMember(s => s.perfil, o => o.MapFrom(s => s.perfil));


            CreateMap<ListarTransGroupEmpresaResult, ListarTransGroupEmpresaVM>()
               .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
               .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD))
               .ForMember(s => s.Empresa, o => o.MapFrom(s => s.Empresa))
               ;

            CreateMap<ListarAreaControllerActionParameterVM, ListarAreaControllerActionParameter>();
            CreateMap<ListarAreaControllerActionParameter, ListarAreaControllerActionParameterVM>();

            CreateMap<ListarPerfilActivosParameterVM, ListarPerfilesActivosParameter>();
            CreateMap<ListarMenusParameterVM, ListarMenuParameter>();
            CreateMap<ListarMenuParameter, ListarMenusParameterVM>();

            CreateMap<ListarVistaParameterVM, ListarVistaParameter>();
            CreateMap<ListarVistaParameter, ListarVistaParameterVM>();

            CreateMap<MantenimientoVistaParameterVM, MantenimientoVistaParameter>();
            CreateMap<MantenimientoMenuParameterVM, MantenimientoMenuParameter>();

            CreateMap<ListarPerfilesActivosResult, ListarPerfilesActivosResultVM>()
                .ForMember(s => s.Perfiles, o => o.MapFrom(s => s.Perfiles));

            CreateMap<TransGroupEmpresa, TransGroupEmpresaVM>()
                .ForMember(s => s.Codigo, o => o.MapFrom(s => s.GTEM_CODIGO))
                .ForMember(s => s.Nombres, o => o.MapFrom(s => s.GTEM_NOMBRES))
                .ForMember(s => s.Ruc, o => o.MapFrom(s => s.GTEM_RUC))
                .ForMember(s => s.Imagen, o => o.MapFrom(s => s.GTEM_IMAGEN))

                ;

            CreateMap<CambiarContrasenaParameterVM, CambiarContrasenaParameter>();

            CreateMap<CambiarContrasenaResult, CambiarContrasenaResultVM>()
                .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
                .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD));



        }
    }
}
