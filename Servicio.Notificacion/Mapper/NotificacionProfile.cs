using AutoMapper;
using Servicio.Notificacion.QueryHandler.ObtenerNotificacionesPorUsuario;
using ViewModel.Notificacion;

namespace Servicio.Notificacion.Mapper
{
    public class NotificacionProfile : Profile
    {

        public NotificacionProfile()
        {
            #region Result to VM

            CreateMap<NotificacionResult, NotificacionVM>()
                .ForMember(s => s.Proceso, o => o.MapFrom(d => d.Proceso))
                .ForMember(s => s.Mensaje, o => o.MapFrom(d => d.Mensaje))
                .ForMember(s => s.Fecha, o => o.MapFrom(d => d.Fecha));

            #endregion

        }

    }
}
