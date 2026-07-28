using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Warehouse.Application.Suppliers.Commands.DeactivateSupplier;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interface;

namespace Warehouse.Api.UnitTests.Suppliers;

public class DeactivateSupplierHandlerTests
{
    private readonly Mock<ISupplierRepository> _repositoryMock;
    private readonly Mock<IDistributedCache> _cacheMock;
    private readonly DeactivateSupplierHandler _handler;

    public DeactivateSupplierHandlerTests()
    {
        _repositoryMock = new Mock<ISupplierRepository>();
        _cacheMock = new Mock<IDistributedCache>();

        _handler = new DeactivateSupplierHandler(
            _repositoryMock.Object,
            _cacheMock.Object);
    }

    [Fact]
    public async Task DeactivateSupplier_ShouldDeactivateSupplier()
    {
        var supplier = new Supplier(
            "Dell",
            "info@dell.com");

        _repositoryMock
            .Setup(x => x.GetById(
                supplier.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(supplier);

        var command = new DeactivateSupplierCommand(supplier.Id);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Success.Should().BeTrue();
        supplier.IsActive.Should().BeFalse();

        _repositoryMock.Verify(
            x => x.Update(
                supplier,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task DeactivateSupplier_WithMissingSupplier_ShouldReturnFalse()
    {
        _repositoryMock
            .Setup(x => x.GetById(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Supplier?)null);

        var command = new DeactivateSupplierCommand(Guid.NewGuid());

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Success.Should().BeFalse();
    }
}