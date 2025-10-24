using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

var factory = new ConnectionFactory()
{
    HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "rabbitmq",
    UserName = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "admin",
    Password = Environment.GetEnvironmentVariable("RABBITMQ_PASS") ?? "admin"
};

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(
    queue: "lines",
    durable: true,
    exclusive: false,
    autoDelete: false,
    arguments: null);

var random = new Random();

Console.WriteLine("Simulator running, sending messages to the queue 'lines'...");

const int SECONDS_DELAY = 2;

while (true)
{
    var message = new
    {
        LineId = random.Next(1, 10),
        Status = random.Next(0, 2) == 0 ? "Running" : "Stopped",
        ProductionCount = random.Next(0, 500),
        Timestamp = DateTime.UtcNow
    };

    var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

    channel.BasicPublish(
        exchange: "",
        routingKey: "lines",
        basicProperties: null,
        body: body);

    Console.WriteLine($"Sent -> {JsonSerializer.Serialize(message)}");

    await Task.Delay(SECONDS_DELAY * 1000);
}
