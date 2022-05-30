﻿using AutoMapper;
using Servicio.Usuario.Models.Cliente;
using Servicio.Usuario.Models.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.Perfil;
using ViewModel.Datos.UsuarioRegistro;

namespace Servicio.Usuario.Profiles
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {

            CreateMap<ListarUsuariosResult, ListarUsuariosResultVM>()
                .ForMember(s => s.TotalRegistros, o => o.MapFrom(s => s.TotalRegistros))
                .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
            .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD))
            .ForMember(s => s.Usuarios, o => o.MapFrom(s => s.Usuarios));

            CreateMap<LeerUsuariosResult, LeerUsuarioResultVM>()
             .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
             .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD))
             .ForMember(s => s.Usuario, o => o.MapFrom(s => s.Usuario));


            CreateMap<Models.Usuario.Usuario, UsuarioVM>()
                .ForMember(s => s.IdEntidad, o => o.MapFrom(s => s.IdEntidad))
                .ForMember(s => s.Activo, o => o.MapFrom(s => s.Activo))
                .ForMember(s => s.ApellidoMaterno, o => o.MapFrom(s => s.ApellidoMaterno))
            .ForMember(s => s.ApellidoPaterno, o => o.MapFrom(s => s.ApellidoPaterno))
            .ForMember(s => s.Correo, o => o.MapFrom(s => s.Correo))
            .ForMember(s => s.EsAdmin, o => o.MapFrom(s => s.EsAdmin))
            .ForMember(s => s.FechaModificacion, o => o.MapFrom(s => s.FechaModificacion))
            .ForMember(s => s.FechaRegistro, o => o.MapFrom(s => s.FechaRegistro))
            .ForMember(s => s.IdPerfil, o => o.MapFrom(s => s.IdPerfil))
            .ForMember(s => s.IdUsuario, o => o.MapFrom(s => s.IdUsuario))
            .ForMember(s => s.Nombres, o => o.MapFrom(s => s.Nombres))
            .ForMember(s => s.PerfilNombre, o => o.MapFrom(s => s.PerfilNombre))
            .ForMember(s => s.EntidadNroDocumneto, o => o.MapFrom(s => s.EntidadNroDocumneto))
            .ForMember(s => s.EntidadTipoDocumento, o => o.MapFrom(s => s.EntidadTipoDocumento))
               .ForMember(s => s.EntidadRazonSocial, o => o.MapFrom(s => s.EntidadRazonSocial))
            .ForMember(s => s.EntidadRepresentanteNombre, o => o.MapFrom(s => s.EntidadRepresentanteNombre))
            .ForMember(s => s.UsuarioCrea, o => o.MapFrom(s => s.UsuarioCrea))
            .ForMember(s => s.UsuarioModifica, o => o.MapFrom(s => s.UsuarioModifica))
            .ForMember(s => s.CorreoConfirmado, o => o.MapFrom(s => s.CorreoConfirmado))
            .ForMember(s => s.CambioContrasenia, o => o.MapFrom(s => s.CambioContrasenia))
            ;

            CreateMap<ListarUsuarioParameterVM, ListarUsuariosParameter>()
              .ForMember(s => s.ApellidoPaterno, o => o.MapFrom(s => s.ApellidoPaterno))
              .ForMember(s => s.ApellidoMaterno, o => o.MapFrom(s => s.ApellidoMaterno))
              .ForMember(s => s.Nombres, o => o.MapFrom(s => s.Nombres))
              .ForMember(s => s.Correo, o => o.MapFrom(s => s.Correo))
              .ForMember(s => s.IdPerdil, o => o.MapFrom(s => s.IdPerdil))
              .ForMember(s => s.IdEntidad, o => o.MapFrom(s => s.IdEntidad))
              .ForMember(s => s.IsAdmin, o => o.MapFrom(s => s.IsAdmin))
              .ForMember(s => s.isActivo, o => o.MapFrom(s => s.isActivo))
              .ForMember(s => s.RegistroInicio, o => o.MapFrom(s => s.RegistroInicio))
              .ForMember(s => s.RegistroFin, o => o.MapFrom(s => s.RegistroFin));

            CreateMap<UsuarioSecundarioResult, UsuarioSecundarioResultVM>()
            .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
            .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD))
            .ForMember(s => s.usuario, o => o.MapFrom(s => s.usuario));

            CreateMap<UsuarioMenu, UsuarioMenuVM>();

            CreateMap<VistaMenu, VistaMenuVM>();
            CreateMap<VistaMenuVM, VistaMenu>();

            CreateMap<ListarUsuarioMenuResult, ListarUsuarioMenuResultVM>()
            .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
            .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD))
            .ForMember(s => s.Menus, o => o.MapFrom(s => s.Menus));

            CreateMap<CambiarPerfilDefectoParameterVM, CambiarPerfilDefectoParameter>()
              .ForMember(s => s.IdPerdil, o => o.MapFrom(s => s.IdPerfil))
              .ForMember(s => s.IdUsuario, o => o.MapFrom(s => s.IdUsuario));

            CreateMap<CambiarPerfilDefectoResult, CambiarPerfilDefectoResultVM>()
           .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
           .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD));

            CreateMap<ListarClienteParameterVM, ListarClienteParameter>();
            CreateMap<DashboardClienteParameterVM, DashboardClienteParameter>();
            CreateMap<DashboardAdminParameterVM, DashboardAdminParameter>();

            CreateMap<DashboardClienteResult, DashboardClienteResultVM>()
          .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
          .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD));


            CreateMap<DashboardAdminResult, DashboardAdminResultVM>()
          .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
          .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD));

            CreateMap<DashboardEstado, DashboardEstadoVM>();
            CreateMap<DashboardFecha, DashboardFechaVM>();
            CreateMap<DashboardUsuarioPerfil, DashboardUsuarioPerfilVM>();

            CreateMap<ListarClienteResult, ListarClientesResultVM>()
           .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
           .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD));

            CreateMap<LeerClienteResult, LeerClienteResultVM>()
        .ForMember(s => s.CodigoResultado, o => o.MapFrom(s => s.IN_CODIGO_RESULTADO))
        .ForMember(s => s.MensajeResultado, o => o.MapFrom(s => s.STR_MENSAJE_BD));

            CreateMap<Cliente, ClienteVM>();

        }
    }
}