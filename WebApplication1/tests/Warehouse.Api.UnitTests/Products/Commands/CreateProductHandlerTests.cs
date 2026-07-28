using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Warehouse.Application.Products.Commands.CreateProduct;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interface;

namespace Warehouse.Api.UnitTests.Products.Commands;

public class CreateProductHandlerTests
{
    private readonly Mock<IProductRepository> _repositoryMock;
    private readonly Mock<IDistributedCache> _cacheMock;
    private readonly CreateProductHandler _handler;

    public CreateProductHandlerTests()
    {
        _repositoryMock = new Mock<IProductRepository>();
        _cacheMock = new Mock<IDistributedCache>();

        _handler = new CreateProductHandler(
            _repositoryMock.Object,
            _cacheMock.Object
        );
    }

    [Fact]
    public async Task CreateProduct_WithValidData_ShouldCreateProduct()
    {
        var command = new CreateProductCommand(
            "Laptop",
            "LAP-001",
            "Gaming Laptop",
            1200m,
            10,
            "Dell",
            DateTime.UtcNow.AddDays(30)
        );

        var result = await _handler.Handle(
            command,
            CancellationToken.None
        );

        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        result.Name.Should().Be("Laptop");
        result.Price.Should().Be(1200m);
        result.Quantity.Should().Be(10);

        _repositoryMock.Verify(
            x => x.Add(
                It.IsAny<Product>(),
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
    public async Task CreateProduct_WithValidData_ShouldAssignCreatedDate()
    {
        Product? createdProduct = null;

        _repositoryMock
            .Setup(x => x.Add(
                It.IsAny<Product>(),
                It.IsAny<CancellationToken>()))
            .Callback<Product, CancellationToken>((product, _) =>
            {
                createdProduct = product;
            })
            .Returns(Task.CompletedTask);

        var command = new CreateProductCommand(
            "Laptop",
            "LAP-001",
            "Gaming Laptop",
            1200m,
            10,
            "Dell",
            DateTime.UtcNow.AddDays(30)
        );

        
        await _handler.Handle(
            command,
            CancellationToken.None
        );

        createdProduct.Should().NotBeNull();
        createdProduct!.CreatedAt.Should()
            .BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }
}