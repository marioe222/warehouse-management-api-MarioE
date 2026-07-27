using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Warehouse.Application.Products.Commands.UpdateProductPrice;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Exceptions;
using Warehouse.Domain.Interface;

namespace Warehouse.Api.UnitTests.Products.Commands;

public class UpdateProductPriceHandlerTests
{
    private readonly Mock<IProductRepository> _repositoryMock;
    private readonly Mock<IDistributedCache> _cacheMock;
    private readonly UpdateProductPriceHandler _handler;

    public UpdateProductPriceHandlerTests()
    {
        _repositoryMock = new Mock<IProductRepository>();
        _cacheMock = new Mock<IDistributedCache>();

        _handler = new UpdateProductPriceHandler(
            _repositoryMock.Object,
            _cacheMock.Object);
    }

    [Fact]
    public async Task UpdatePrice_WithValidPrice_ShouldUpdatePrice()
    {
        var product = new Product(
            "Laptop",
            "LAP-001",
            "Gaming Laptop",
            1200m,
            10,
            "Dell",
            DateTime.UtcNow.AddDays(30));

        _repositoryMock
            .Setup(x => x.GetById(product.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var command = new UpdateProductPriceCommand(
            product.Id,
            1500m);

        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        result.Success.Should().BeTrue();
        product.Price.Should().Be(1500m);

        _repositoryMock.Verify(
            x => x.Update(product, It.IsAny<CancellationToken>()),
            Times.Once);

        _cacheMock.Verify(
            x => x.RemoveAsync(
                $"product:{product.Id}",
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdatePrice_WithInvalidPrice_ShouldThrowException()
    {
        var product = new Product(
            "Laptop",
            "LAP-001",
            "Gaming Laptop",
            1200m,
            10,
            "Dell",
            DateTime.UtcNow.AddDays(30));

        _repositoryMock
            .Setup(x => x.GetById(product.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var command = new UpdateProductPriceCommand(
            product.Id,
            -100m);

        Func<Task> act = async () =>
            await _handler.Handle(command, CancellationToken.None);

        await act.Should()
            .ThrowAsync<BusinessRuleException>();
    }

    [Fact]
    public async Task UpdatePrice_WithMissingProduct_ShouldReturnFalse()
    {
        var command = new UpdateProductPriceCommand(
            Guid.NewGuid(),
            1500m);

        _repositoryMock
            .Setup(x => x.GetById(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        result.Success.Should().BeFalse();

        _repositoryMock.Verify(
            x => x.Update(
                It.IsAny<Product>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}