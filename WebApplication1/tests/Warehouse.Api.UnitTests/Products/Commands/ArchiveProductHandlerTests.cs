using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Warehouse.Application.Products.Commands.ArchiveProduct;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interface;

namespace Warehouse.Api.UnitTests.Products.Commands;

public class ArchiveProductHandlerTests
{
    private readonly Mock<IProductRepository> _repositoryMock;
    private readonly Mock<IDistributedCache> _cacheMock;
    private readonly ArchiveProductHandler _handler;

    public ArchiveProductHandlerTests()
    {
        _repositoryMock = new Mock<IProductRepository>();
        _cacheMock = new Mock<IDistributedCache>();

        _handler = new ArchiveProductHandler(
            _repositoryMock.Object,
            _cacheMock.Object);
    }

    [Fact]
    public async Task ArchiveProduct_WithExistingProduct_ShouldArchiveProduct()
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

        var command = new ArchiveProductCommand(product.Id);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Success.Should().BeTrue();
        product.IsArchived.Should().BeTrue();

        _repositoryMock.Verify(
            x => x.Update(product, It.IsAny<CancellationToken>()),
            Times.Once);

        _cacheMock.Verify(
            x => x.RemoveAsync($"product:{product.Id}", It.IsAny<CancellationToken>()),
            Times.Once);

        _cacheMock.Verify(
            x => x.RemoveAsync("products:True", It.IsAny<CancellationToken>()),
            Times.Once);

        _cacheMock.Verify(
            x => x.RemoveAsync("products:False", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task ArchiveProduct_WithMissingProduct_ShouldReturnFalse()
    {
        var command = new ArchiveProductCommand(Guid.NewGuid());

        _repositoryMock
            .Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Success.Should().BeFalse();

        _repositoryMock.Verify(
            x => x.Update(
                It.IsAny<Product>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}