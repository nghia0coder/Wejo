using Microsoft.AspNetCore.SignalR;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace Wejo.Realtime.API.MessageQueue;

using Common.Core.Constants;
using Common.Domain.Dtos;
using Hubs;

public class PlaypalChatConsumer : BackgroundService
{
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly string _queueName;
    private readonly IHubContext<PlaypalChatHub> _hc;

    public PlaypalChatConsumer(IHubContext<PlaypalChatHub> hc, string queueName = QueueName.PlaypalChatMessage)
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

                if (message == null || string.IsNullOrEmpty(message.ReveiverId) || message.Message == null)
                {
                    await _channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
                    return;
                }

                string ReveiverId = message.ReveiverId;
                var messageDto = message.Message;

                await _hc.Clients.Group(ReveiverId).SendAsync(RealTimeTopic.ReceivePlaypalMessage, messageDto);
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
        public string ReveiverId { get; set; } = null!;
        public UserChatMessageDto Message { get; set; } = null!;
    }
}