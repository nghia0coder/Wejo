using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Wejo.Game.Infrastructure.MessageQueue;

using Common.Core.Constants;

public class RabbitMQProducer : IMessageQueue, IAsyncDisposable
{
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly string _queueName = QueueName.GameChatMessage;

    public RabbitMQProducer(ILogger<RabbitMQProducer> logger, string queueName = QueueName.GameChatMessage)
    {
        _queueName = queueName;
        InitAsync().GetAwaiter().GetResult();
    }

    public async Task InitAsync()
    {
        var factory = new ConnectionFactory
        {
            HostName = "rabbitmq", // Change to localhost if running locally
            Port = 5672,
            UserName = "wejo",
            Password = "wejo"
        };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.QueueDeclareAsync(
            queue: _queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
    }

    public async Task PublishAsync<T>(string queueName, T message)
    {
        if (_channel == null)
            throw new InvalidOperationException("RabbitMQ channel chưa được khởi tạo. Gọi InitAsync() trước.");

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        await _channel.BasicPublishAsync(
            exchange: "",
            routingKey: queueName,
            body: body
        );
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel != null)
        {
            await _channel.CloseAsync();
            await _channel.DisposeAsync();
        }

        if (_connection != null)
        {
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
        }
    }
}
