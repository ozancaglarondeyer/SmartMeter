
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ReportApi.Models.Entities;
using ReportApi.Services.ReportService;
using System.Text;

namespace ReportApi.BackgroundServices
{
    public class RabbitMQBackgroundService : BackgroundService
    {
        private readonly RabbitMQConfiguration _rabbitMQConfig;
        private readonly IServiceProvider _serviceProvider;
        private IConnection _connection;
        private IModel _channel;
        public RabbitMQBackgroundService(IOptions<RabbitMQConfiguration> rabbitMQConfig, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _rabbitMQConfig = rabbitMQConfig.Value;

            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMQConfig.HostName,
                UserName = _rabbitMQConfig.UserName,
                Password = _rabbitMQConfig.Password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }



        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _channel.QueueDeclare(queue: RabbitMQSettings.ReportQueue,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Guid reportId = Guid.Parse(message);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var reportGenerator = scope.ServiceProvider.GetRequiredService<IReportService>();
                    reportGenerator.GenerateReport(reportId);
                }
                
            };

            _channel.BasicConsume(queue: RabbitMQSettings.ReportQueue,
                                 autoAck: true,
                                 consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }

    }
}
