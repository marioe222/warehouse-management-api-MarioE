namespace Warehouse.Application.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync<T>(
        T message,
        string routingKey);
}