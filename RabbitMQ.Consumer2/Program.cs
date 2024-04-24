using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQ.Consumer2;

internal class Program
{
    static void Main(string[] args)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        
         

        channel.QueueDeclare(queue: "test-queue",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        Console.WriteLine(" [*] Waiting for messages.");

        var consumer = new EventingBasicConsumer(channel);
        
        consumer.Received += (model, ea) =>
        {
            if(ea.DeliveryTag > 5)
            {
                channel.BasicReject(ea.DeliveryTag, false);
            }
            else
            {
                channel.BasicNack(ea.DeliveryTag, false, true);
            }


        };
        channel.BasicConsume(queue: "test-queue",
                             autoAck: false,

                             consumer: consumer);

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }
}
