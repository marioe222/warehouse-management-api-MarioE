using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Warehouse.Application.Products.Commands.AssignSupplier;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Exceptions;
using Warehouse.Domain.Interface;

namespace Warehouse.Api.UnitTests.Suppliers;

public class AssignSupplierHandlerTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<ISupplierRepository> _supplierRepositoryMock;
    private readonly Mock<IDistributedCache> _cacheMock;
    private readonly AssignSupplierHandler _handler;

    public AssignSupplierHandlerTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _supplierRepositoryMock = new Mock<ISupplierRepository>();
        _cacheMock = new Mock<IDistributedCache>();

        _handler = new AssignSupplierHandler(
            _productRepositoryMock.Object,
            _supplierRepositoryMock.Object,
            _cacheMock.Object);
    }

    [Fact]
    public async Task AssignSupplier_WithValidData_ShouldAssignSupplier()
    {
        var product = new Product(
            "Laptop",
            "LAP-001",
            "Gaming Laptop",
            1200m,
            10,
            null,
            DateTime.UtcNow.AddDays(30));

        var supplier = new Supplier(
            "Dell",
            "info@dell.com");

        _productRepositoryMock
            .Setup(x => x.GetById(
                product.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        _supplierRepositoryMock
            .Setup(x => x.GetById(
                supplier.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(supplier);

        var command = new AssignSupplierCommand(
            product.Id,
            supplier.Id);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Success.Should().BeTrue();
        product.SupplierId.Should().Be(supplier.Id);
        product.SupplierName.Should().Be("Dell");
    }

    [Fact]
    public async Task AssignSupplier_WithArchivedProduct_ShouldThrowException()
    {
        var product = new Product(
            "Laptop",
            "LAP-001",
            "Gaming Laptop",
            1200m,
            10,
            null,
            DateTime.UtcNow.AddDays(30));

        product.Archive();

        var supplier = new Supplier(
            "Dell",
            "info@dell.com");

        _productRepositoryMock
            .Setup(x => x.GetById(
                product.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        _supplierRepositoryMock
            .Setup(x => x.GetById(
                supplier.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(supplier);

        var command = new AssignSupplierCommand(
            product.Id,
            supplier.Id);

        Func<Task> act = async () =>
            await _handler.Handle(command, CancellationToken.None);

        await act.Should()
            .ThrowAsync<BusinessRuleException>();
    }

    [Fact]
    public async Task AssignSupplier_WithMissingSupplier_ShouldReturnFalse()
    {
        var product = new Product(
            "Laptop",
            "LAP-001",
            "Gaming Laptop",
            1200m,
            10,
            null,
            DateTime.UtcNow.AddDays(30));

        _productRepositoryMock
            .Setup(x => x.GetById(
                product.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        _supplierRepositoryMock
            .Setup(x => x.GetById(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Supplier?)null);

        var command = new AssignSupplierCommand(
            product.Id,
            Guid.NewGuid());

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Success.Should().BeFalse();
    }
}