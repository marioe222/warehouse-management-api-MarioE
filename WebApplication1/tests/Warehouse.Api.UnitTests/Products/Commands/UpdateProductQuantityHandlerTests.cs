using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Warehouse.Application.Products.Commands.UpdateProductQuantity;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Exceptions;
using Warehouse.Domain.Interface;

namespace Warehouse.Api.UnitTests.Products.Commands;

public class UpdateProductQuantityHandlerTests
{
    private readonly Mock<IProductRepository> _repositoryMock;
    private readonly Mock<IDistributedCache> _cacheMock;
    private readonly UpdateProductQuantityHandler _handler;

    public UpdateProductQuantityHandlerTests()
    {
        _repositoryMock = new Mock<IProductRepository>();
        _cacheMock = new Mock<IDistributedCache>();

        _handler = new UpdateProductQuantityHandler(
            _repositoryMock.Object,
            _cacheMock.Object);
    }

    [Fact]
    public async Task UpdateQuantity_WithValidQuantity_ShouldUpdateStock()
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

        var command = new UpdateProductQuantityCommand(
            product.Id,
            5);

        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        result.Success.Should().BeTrue();
        product.QuantityInStock.Should().Be(15);

        _repositoryMock.Verify(
            x => x.Update(product, It.IsAny<CancellationToken>()),
            Times.Once);

        _cacheMock.Verify(
            x => x.RemoveAsync(
                $"product:{product.Id}",
                It.IsAny<CancellationToken>()),
            Times.Once);

        _cacheMock.Verify(
            x => x.RemoveAsync(
                "products:True",
                It.IsAny<CancellationToken>()),
            Times.Once);

        _cacheMock.Verify(
            x => x.RemoveAsync(
                "products:False",
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateQuantity_WithNegativeQuantity_ShouldThrowException()
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

        var command = new UpdateProductQuantityCommand(
            product.Id,
            -20);

        Func<Task> act = async () =>
            await _handler.Handle(command, CancellationToken.None);

        await act.Should()
            .ThrowAsync<BusinessRuleException>();
    }

    [Fact]
    public async Task UpdateQuantity_WithMissingProduct_ShouldReturnFalse()
    {
        var command = new UpdateProductQuantityCommand(
            Guid.NewGuid(),
            5);

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