using RabbitMQ.Client;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
        };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.BasicQos(0, 5, false);

        channel.QueueDeclare(queue: "test-queue",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
        while (true)
        {
            var message = Console.ReadLine();
            if (message == "exit")
                return;

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: string.Empty,
                routingKey: "test-queue",
                basicProperties: null,
                body: body);
            
        }
    }
}