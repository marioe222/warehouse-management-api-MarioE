using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Warehouse.Application.Products.Queries.SearchProducts;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interface;

namespace Warehouse.Api.UnitTests.Products.Queries;

public class SearchProductsHandlerTests
{
    private readonly Mock<IProductRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IDistributedCache> _cacheMock;
    private readonly SearchProductsHandler _handler;

    public SearchProductsHandlerTests()
    {
        _repositoryMock = new Mock<IProductRepository>();
        _mapperMock = new Mock<IMapper>();
        _cacheMock = new Mock<IDistributedCache>();

        _handler = new SearchProductsHandler(
            _repositoryMock.Object,
            _mapperMock.Object,
            _cacheMock.Object);
    }

    [Fact]
    public async Task SearchProducts_ByName_ShouldReturnMatches()
    {
        var products = new List<Product>
        {
            new Product(
                "Laptop",
                "LAP-001",
                "Gaming Laptop",
                1200m,
                10,
                "Dell",
                DateTime.UtcNow.AddDays(30))
        };

        var productViewModels = new List<ProductViewModel>
        {
            new()
            {
                Name = "Laptop",
                Price = 1200m,
                Quantity = 10,
                SupplierName = "Dell"
            }
        };

        _cacheMock
            .Setup(x => x.GetStringAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((string?)null);

        _repositoryMock
            .Setup(x => x.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        _mapperMock
            .Setup(x => x.Map<List<ProductViewModel>>(It.IsAny<List<Product>>()))
            .Returns(productViewModels);

        var query = new SearchProductsQuery("Laptop", null);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Products.Should().HaveCount(1);
        result.Products.First().Name.Should().Be("Laptop");
    }

    [Fact]
    public async Task SearchProducts_BySupplier_ShouldReturnMatches()
    {
        var products = new List<Product>
        {
            new Product(
                "Laptop",
                "LAP-001",
                "Gaming Laptop",
                1200m,
                10,
                "Dell",
                DateTime.UtcNow.AddDays(30))
        };

        var productViewModels = new List<ProductViewModel>
        {
            new()
            {
                Name = "Laptop",
                SupplierName = "Dell"
            }
        };

        _cacheMock
            .Setup(x => x.GetStringAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((string?)null);

        _repositoryMock
            .Setup(x => x.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        _mapperMock
            .Setup(x => x.Map<List<ProductViewModel>>(It.IsAny<List<Product>>()))
            .Returns(productViewModels);

        var query = new SearchProductsQuery(null, "Dell");

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Products.Should().ContainSingle();
        result.Products.First().SupplierName.Should().Be("Dell");
    }

    [Fact]
    public async Task SearchProducts_ByNameAndSupplier_ShouldReturnIntersection()
    {
        var products = new List<Product>
        {
            new Product(
                "Laptop",
                "LAP-001",
                "Gaming Laptop",
                1200m,
                10,
                "Dell",
                DateTime.UtcNow.AddDays(30))
        };

        var productViewModels = new List<ProductViewModel>
        {
            new()
            {
                Name = "Laptop",
                SupplierName = "Dell"
            }
        };

        _cacheMock
            .Setup(x => x.GetStringAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((string?)null);

        _repositoryMock
            .Setup(x => x.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        _mapperMock
            .Setup(x => x.Map<List<ProductViewModel>>(It.IsAny<List<Product>>()))
            .Returns(productViewModels);

        var query = new SearchProductsQuery("Laptop", "Dell");

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Products.Should().HaveCount(1);
        result.Products[0].Name.Should().Be("Laptop");
        result.Products[0].SupplierName.Should().Be("Dell");
    }

    [Fact]
    public async Task SearchProducts_ShouldStoreResultsInCache()
    {
        // Arrange
        _cacheMock
            .Setup(x => x.GetStringAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((string?)null);

        _repositoryMock
            .Setup(x => x.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Product>());

        _mapperMock
            .Setup(x => x.Map<List<ProductViewModel>>(It.IsAny<List<Product>>()))
            .Returns(new List<ProductViewModel>());

        await _handler.Handle(
            new SearchProductsQuery(null, null),
            CancellationToken.None);

        _cacheMock.Verify(
            x => x.SetStringAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<DistributedCacheEntryOptions>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}