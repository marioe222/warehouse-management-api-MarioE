using Warehouse.Application.Interfaces;

namespace Warehouse.Api.IntegrationTests.Fakes;

public class FakeEventPublisher : IEventPublisher
{
    public Task PublishAsync<T>(
        T message,
        string routingKey)
    {
        return Task.CompletedTask;
    }
}