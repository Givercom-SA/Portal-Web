using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Security.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Principal.Entities;
using Web.Principal.Interface;

namespace Web.Principal.Hubs
{
    public class NotificationUserHub : Hub<INotificationHub>
    {
        private readonly IUserConnectionManager _userConnectionManager;
        private ILogger<NotificationUserHub> _logger;
        private IMemoryCache _memoryCache;

        public NotificationUserHub(IUserConnectionManager userConnectionManage,
            ILogger<NotificationUserHub> logger,
            IMemoryCache memoryCache): base()
        {
            _userConnectionManager = userConnectionManage;
            _logger = logger;
            _memoryCache = memoryCache;

        }


        public static List<Tuple<int, string>> Usuarios { get; set; } = new List<Tuple<int, string>>();
        public async Task<List<Notificacion>> Matricular(string codigoUsuarioEncriptado)
        {

            var codigoUsario = Encriptador.DesencriptarTexto(codigoUsuarioEncriptado);

            _userConnectionManager.KeepUserConnection(codigoUsario, Context.ConnectionId);

            var notificacionesEnviar = new List<Notificacion>();

            return notificacionesEnviar;
        }

        //Called when a connection with the hub is terminated.
        public async override Task OnDisconnectedAsync(Exception exception)
        {
            //get the connectionId
            var connectionId = Context.ConnectionId;
            _userConnectionManager.RemoveUserConnection(connectionId);
            var value = await Task.FromResult(0);//adding dump code to follow the template of Hub > OnDisconnectedAsync
        }
    }
}
