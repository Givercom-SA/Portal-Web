using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Web.Principal.Entities;

namespace Web.Principal.Hub
{
    public interface INotificacionHub
    {
        Task RecibirNotificacion(Notificacion notificacion);

    }
}