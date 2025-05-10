namespace Wejo.Identity.Infrastructure.MessageQueue;

public interface IMessageQueue
{
    Task PublishAsync<T>(string queueName, T message);
}
