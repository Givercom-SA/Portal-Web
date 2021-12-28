using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Servicio.Embarque.Repositorio;
using Servicio.Embarque.ServiceConsumer;
using Servicio.Embarque.ServiceExterno;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TransMares.Core;
using ViewModel.Datos.UsuarioRegistro;
using Servicio.Embarque.Models.GestionarMemo;

namespace Servicio.Embarque.Jobs
{
    public class BackgroundMemo : IHostedService, IDisposable
    {
        private int number = 0;
        private Timer timer;

        private readonly IMemoRepository _repository;
        private readonly ServicioUsuario _serviceUsuario;
        private readonly ServicioEmbarques _serviceEmbarques;
        private IConfiguration _configuration;

        public BackgroundMemo(IMemoRepository repository,
            ServicioUsuario serviceUsuario,
            ServicioEmbarques serviceEmbarques,
            IConfiguration configuration)
        {
            _repository = repository;
            _serviceUsuario = serviceUsuario;
            _serviceEmbarques = serviceEmbarques;
            _configuration = configuration;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(o =>
            {
                Interlocked.Increment(ref number);
                Console.WriteLine($"Memo worker: {number}");

                try
                {
                    string pattern = "BM_*";
                    string rutaNotificacionesPendiente = _configuration["MemosCarpeta:PendientesPath"];
                    string rutaNotificacionesProcesadas = _configuration["MemosCarpeta:ProcesadasPath"];

                    var archivos = new DirectoryInfo(rutaNotificacionesPendiente).GetFiles(pattern);
                    string KeyBL = string.Empty;
                    foreach(var archivo in archivos)
                    {
                        string KeyBLFull = Path.GetFileName(archivo.Name).ToUpper().Replace("BM_","");
                        string[] arrayKeyBl = KeyBLFull.Split('_');
                        KeyBL = arrayKeyBl[0];
                        var parameter = new NotificacionMemoParameter
                        {
                            KeyBLD = KeyBL,
                            IdUsuario = -1,
                            NombreArchivo= KeyBLFull
                        };
                        var result = _repository.CrearNotificacionMemo(parameter);
                        archivo.MoveTo(string.Format("{0}/{1}", rutaNotificacionesProcesadas, archivo.Name));
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error from Memo worker: {ex.Message}");
                }


            },
                null,
                TimeSpan.Zero,
                TimeSpan.FromMinutes(5));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
