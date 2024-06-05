
using MeterApi.DTOs;
using MeterApi.Services.MeterService;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Text;

namespace MeterApi.BackgroundServices
{
    public class TcpServer : BackgroundService
    {
        private TcpListener _listener;

        private readonly IServiceProvider _serviceProvider;
        public TcpServer(IServiceProvider serviceProvider)
        {
            
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _listener = new TcpListener(IPAddress.Any, 5000);
            _listener.Start();


            while (!stoppingToken.IsCancellationRequested)
            {
                var client = await _listener.AcceptTcpClientAsync();
                _ = Task.Run(() => HandleClientAsync(client, stoppingToken));
            }

            _listener.Stop();
        }

        private async Task HandleClientAsync(TcpClient client, CancellationToken stoppingToken)
        {
            var buffer = new byte[1024];
            var stream = client.GetStream();

            while (!stoppingToken.IsCancellationRequested)
            {
                var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, stoppingToken);
                if (bytesRead == 0) break;

                var message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                var meterReadingDto = JsonSerializer.Deserialize<MeterReadingDTO>(message);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var meterService = scope.ServiceProvider.GetRequiredService<IMeterService>();
                    meterService.AddMeterReading(meterReadingDto);
                }

            }

            client.Close();
        }
    }
}
