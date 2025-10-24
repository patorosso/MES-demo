using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace backend.Hubs
{
    public class RabbitMqConsumer : BackgroundService
    {
        private readonly IHubContext<LinesHub> _hub;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMqConsumer(IHubContext<LinesHub> hub)
        {
            _hub = hub;

            var factory = new ConnectionFactory()
            {
                HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "rabbitmq",
                UserName = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "admin",
                Password = Environment.GetEnvironmentVariable("RABBITMQ_PASS") ?? "admin"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: "lines",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                await _hub.Clients.All.SendAsync("ReceiveUpdate", message);
            };

            _channel.BasicConsume(queue: "lines", autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }
}
