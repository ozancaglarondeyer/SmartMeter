namespace ReportApi.Services.RabbitMQService
{
    public interface IRabbitMQService
    {
        void SendMessage(string message, string queueName);
    }
}
