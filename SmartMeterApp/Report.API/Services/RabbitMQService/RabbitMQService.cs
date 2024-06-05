using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;

namespace ReportApi.Services.RabbitMQService
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly RabbitMQConfiguration _rabbitMQConfig;

        public RabbitMQService(IOptions<RabbitMQConfiguration> rabbitMQConfig)
        {
            _rabbitMQConfig = rabbitMQConfig.Value;
        }

        public void SendMessage(string message, string queueName)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMQConfig.HostName,
                UserName = _rabbitMQConfig.UserName,
                Password = _rabbitMQConfig.Password
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: queueName,
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}
