using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Web.Principal.Entities;

namespace Web.Principal.Hubs
{
    public interface INotificationHub 
    {
        Task RecibirNotificacion(Notificacion notificacion);

    }
}