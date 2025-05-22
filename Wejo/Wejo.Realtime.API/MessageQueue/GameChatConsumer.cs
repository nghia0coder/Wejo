using Microsoft.AspNetCore.SignalR;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using Wejo.Common.Core.Constants;
using Wejo.Common.Domain.Dtos;
using Wejo.Realtime.API.Hubs;

namespace Wejo.Realtime.API.MessageQueue;

public class GameChatConsumer : BackgroundService
{
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly string _queueName;
    private readonly IHubContext<GameChatHub> _hc;

    public GameChatConsumer(IHubContext<GameChatHub> hc, string queueName = QueueName.GameChatMessage)
    {
        _queueName = queueName;
        _hc = hc;
        InitAsync().GetAwaiter().GetResult();
    }

    public async Task InitAsync()
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost", // Change to localhost if running locally
            Port = 5672,
            UserName = "wejo",
            Password = "wejo"
        };

        _connection = await Policy
            .Handle<RabbitMQ.Client.Exceptions.BrokerUnreachableException>()
            .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (exception, timeSpan, retryCount, context) =>
            {
                Console.WriteLine($"Retry {retryCount} for RabbitMQ connection: {exception.Message}. Waiting {timeSpan.TotalSeconds} seconds.");
            })
            .ExecuteAsync(async () => await factory.CreateConnectionAsync());
        _channel = await _connection.CreateChannelAsync();

        await _channel.QueueDeclareAsync(
            queue: _queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_channel == null)
        {
            throw new InvalidOperationException("Channel is not initialized.");
        }

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (sender, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = JsonSerializer.Deserialize<IncomingMessage>(body);

                if (message == null || string.IsNullOrEmpty(message.GameId) || message.Message == null)
                {
                    await _channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
                    return;
                }

                string gameId = message.GameId;
                var messageDto = message.Message;

                await _hc.Clients.Group(gameId).SendAsync(RealTimeTopic.ReceiveGameMessage, messageDto);
                await _channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                await _channel.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
                throw new Exception("Error processing message", ex);
            }
        };

        await _channel.BasicConsumeAsync(queue: _queueName, autoAck: false, consumer: consumer);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await DisposeAsync();
        await base.StopAsync(cancellationToken);
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

    private class IncomingMessage
    {
        public string GameId { get; set; } = null!;
        public GameChatMessageDto Message { get; set; } = null!;
    }
}