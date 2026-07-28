using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Warehouse.Application.Suppliers.Commands.CreateSupplier;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interface;

namespace Warehouse.Api.UnitTests.Suppliers;

public class CreateSupplierHandlerTests
{
    private readonly Mock<ISupplierRepository> _repositoryMock;
    private readonly Mock<IDistributedCache> _cacheMock;
    private readonly CreateSupplierHandler _handler;

    public CreateSupplierHandlerTests()
    {
        _repositoryMock = new Mock<ISupplierRepository>();
        _cacheMock = new Mock<IDistributedCache>();

        _handler = new CreateSupplierHandler(
            _repositoryMock.Object,
            _cacheMock.Object);
    }

    [Fact]
    public async Task CreateSupplier_WithValidData_ShouldCreateSupplier()
    {
        var command = new CreateSupplierCommand(
            "Dell",
            "USA",
            "info@dell.com",
            "123456");

        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        result.SupplierId.Should().NotBeEmpty();
        result.Name.Should().Be("Dell");
        result.ContactEmail.Should().Be("info@dell.com");

        _repositoryMock.Verify(
            x => x.Add(
                It.IsAny<Supplier>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _cacheMock.Verify(
            x => x.RemoveAsync(
                "suppliers",
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}