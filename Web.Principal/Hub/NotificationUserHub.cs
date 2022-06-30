using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Security.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Principal.Entities;
using Web.Principal.Interface;

using Utilitario.Constante;
using ViewModel.Notificacion;
using Web.Principal.ServiceConsumer;
using System.Linq;

namespace Web.Principal.Hub
{
    public class NotificationUserHub : Hub<INotificacionHub>
    {
        private readonly IUserConnectionManager _userConnectionManager;
        private ILogger<NotificationUserHub> _logger;
        private readonly ServicioNotificacion _notificacionService;
        private IMemoryCache _memoryCache;

        public NotificationUserHub(IUserConnectionManager userConnectionManage,
            ILogger<NotificationUserHub> logger,
               ServicioNotificacion notificacionService,
            IMemoryCache memoryCache): base()
        {
            _userConnectionManager = userConnectionManage;
            _logger = logger;
            _memoryCache = memoryCache;
            _notificacionService = notificacionService;
        }

        public static List<Tuple<int, string>> Usuario { get; set; } = new List<Tuple<int, string>>();

        public async Task<NotificacionManager> Matricular(string codigoUsuarioEncriptado)
        {
            var notificacionManager = new NotificacionManager();
            notificacionManager.Notificaciones= new List<Notificacion>();

            try
            {

                var codigoUsuario =int.Parse( Encriptador.Instance.DesencriptarTexto(codigoUsuarioEncriptado));

                _userConnectionManager.KeepUserConnection(codigoUsuario.ToString(), Context.ConnectionId);

                var t = Tuple.Create(Convert.ToInt32(codigoUsuario), Context.ConnectionId);
                Usuario.Add(t);

                var notificaciones = _memoryCache.Get<Dictionary<int, List<NotificacionVM>>>(SistemaConstante.Cache.Notificaciones);

                var notificacionesPorUsuario = new List<NotificacionVM>();

                notificaciones.TryGetValue(codigoUsuario, out notificacionesPorUsuario);

                //Por Seguridad no se envia el codigo de usuario, ademas no es necesario
                if (notificacionesPorUsuario != null)
                    notificacionManager.Notificaciones = notificacionesPorUsuario.Select(x => new Notificacion {    CodigoUsuario = 0, 
                                                                                                                    CreacionFecha = x.Fecha, 
                                                                                                                    Mensaje = x.Mensaje, 
                                                                                                                    Proceso = x.Proceso ,
                                                                                                                    ContadorVisible=x.ContadorVisible,
                                                                                                                    FechaFormato=x.FechaFormato,
                                                                                                                    Leido=x.Leido,
                                                                                                                    Link=x.Link,
                                                                                                                    Titulo  =x.Titulo
                                                                                                                    }).ToList();        

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Sucedio un error en Matricula");
            }

            notificacionManager.CantidadNotificacion = notificacionManager.Notificaciones.Count(X=>X.ContadorVisible==false);

            return notificacionManager;
        }

        public async Task LimpiarNotificaciones(string codigoUsuarioEncriptado)
        {
            try
            {
                var codigoUsuario = Int32.Parse(Encriptador.Instance.DesencriptarTexto(codigoUsuarioEncriptado));
                await _notificacionService.LimpiarNotificacionesPorUsuario(codigoUsuario);

                _memoryCache.Get<Dictionary<int, List<NotificacionVM>>>(SistemaConstante.Cache.Notificaciones)[codigoUsuario] = new List<NotificacionVM>();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Sucedio un error al LimpiarNotificaciones");
            }

        }

        public async Task LimpiarContadorNotificaciones(string codigoUsuarioEncriptado)
        {
            try
            {
                var codigoUsuario = Int32.Parse(Encriptador.Instance.DesencriptarTexto(codigoUsuarioEncriptado));
                await _notificacionService.LimpiarContadorNotificacionesPorUsuario(codigoUsuario);

                _memoryCache.Get<Dictionary<int, List<NotificacionVM>>>(SistemaConstante.Cache.Notificaciones)[codigoUsuario].ForEach(x=> {
                    x.ContadorVisible = true;
                });


            }
            catch (Exception e)
            {
                _logger.LogError(e, "Sucedio un error al limpiar el contador de notificación.");
            }

        }

        public async Task RecibirNotificacionDB(Notificacion notificacion)
        {
            _logger.LogInformation("Se recibio de DB el mensaje :" + notificacion);

            try
            {
                //Grabar en cache



                await recibirEnviarNotificaion(notificacion);

            }
            catch (KeyNotFoundException)
            {
                _memoryCache.Get<Dictionary<int, List<NotificacionVM>>>(SistemaConstante.Cache.Notificaciones)[notificacion.CodigoUsuario] = new List<NotificacionVM>();
                _logger.LogWarning("No se pudo obtener la informacion de Notificaciones desde Cache, se creará la información para el usuario : " + notificacion.CodigoUsuario);
                await recibirEnviarNotificaion(notificacion);

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Sucedio un error al RecibirNotificacionDB");
            }

        }


        private async Task  recibirEnviarNotificaion(Notificacion notificacion) {

            _memoryCache.Get<Dictionary<int, List<NotificacionVM>>>(SistemaConstante.Cache.Notificaciones)[notificacion.CodigoUsuario]
                       .Add(new NotificacionVM
                       {
                           Proceso = notificacion.Proceso,
                           Mensaje = notificacion.Mensaje,
                           Fecha = notificacion.CreacionFecha,
                           Titulo = notificacion.Titulo,
                           Leido = notificacion.Leido,
                           Link = notificacion.Link,
                           ContadorVisible = notificacion.ContadorVisible,
                           FechaFormato= notificacion.FechaFormato
                       });

            foreach (var t in Usuario)
            {
                if (t.Item1 == notificacion.CodigoUsuario)
                {
                    //Si es necesario realizar otra accion para un proceso diferente agregar un else if.
                    await Clients.Client(t.Item2).RecibirNotificacion(notificacion);
                }
            }
        }


        //Called when a connection with the hub is terminated.
        public async override Task OnDisconnectedAsync(Exception exception)
        {
           

            int conexionesEliminadas = 0;

            try
            {
                conexionesEliminadas = Usuario.RemoveAll(x => {
                    return x.Item2.Equals(Context.ConnectionId);
                });

                var connectionId = Context.ConnectionId;
                _userConnectionManager.RemoveUserConnection(connectionId);
                var value = await Task.FromResult(0);//adding dump code to follow the template of Hub > OnDisconnectedAsync

            }
            catch (Exception ex1)
            {
                _logger.LogError(ex1, "Sucedio un error en OnDisconnectedAsync");
            }

            await base.OnDisconnectedAsync(exception);

        }

    }
}
