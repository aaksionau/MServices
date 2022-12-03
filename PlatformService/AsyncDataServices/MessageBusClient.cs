using System.Text;
using System.Text.Json;
using PlatformService.Dtos;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices;

public class MessageBusClient : IMessageBusClient
{
    private readonly IConfiguration configuration;
    private readonly IConnection connection;
    private IModel channel;

    public MessageBusClient(IConfiguration configuration)
    {
        this.configuration = configuration;
        var factory = new ConnectionFactory()
        {
            HostName = this.configuration["RabbitMQHost"],
            Port = Int32.Parse(configuration["RabbitMQPort"])
        };

        try
        {
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "Platforms", type: ExchangeType.Fanout);

            connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not connect to message bus. Exception: {ex.Message}");
            throw;
        }
    }

    private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
    {
        Console.WriteLine("--> Connection to RabbitMQ was shutdown");
    }

    public void PublishNewPlatform(PlatformPublishDto platform)
    {
        if (!connection.IsOpen)
        {
            Console.WriteLine("--> Connection is not open");
            return;
        }

        var message = JsonSerializer.Serialize(platform);
        try
        {
            SendMessage(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> There was an error while sending message. Exception: {ex.Message}");
        }

    }

    private void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "Platforms",
                            routingKey: "",
                            basicProperties: channel.CreateBasicProperties(),
                            body);

        Console.WriteLine($"--> Message was sent: {message}");
    }
}