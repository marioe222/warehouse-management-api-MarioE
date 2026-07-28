using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Warehouse.Application.Interfaces;
using Warehouse.Application.Products.Commands.UploadProductImage;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interface;
using Warehouse.IntegrationEvents.Events;

namespace Warehouse.Api.UnitTests.Products.Commands;

public class UploadProductImageHandlerTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IDistributedCache> _cacheMock;
    private readonly Mock<IStorageService> _storageServiceMock;
    private readonly Mock<IFileMetadataRepository> _fileRepositoryMock;
    private readonly Mock<IEventPublisher> _publisherMock;

    private readonly UploadProductImageHandler _handler;

    public UploadProductImageHandlerTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _cacheMock = new Mock<IDistributedCache>();
        _storageServiceMock = new Mock<IStorageService>();
        _fileRepositoryMock = new Mock<IFileMetadataRepository>();
        _publisherMock = new Mock<IEventPublisher>();

        _handler = new UploadProductImageHandler(
            _productRepositoryMock.Object,
            _cacheMock.Object,
            _storageServiceMock.Object,
            _fileRepositoryMock.Object,
            _publisherMock.Object);
    }

    [Fact]
    public async Task UploadJpg_ShouldSucceed()
    {
        var product = new Product(
            "Laptop",
            "LAP-001",
            "Gaming",
            1200m,
            10,
            null,
            DateTime.UtcNow.AddDays(30));

        _productRepositoryMock
            .Setup(x => x.GetById(product.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var file = new Mock<IFormFile>();

        file.Setup(x => x.Length).Returns(1024);
        file.Setup(x => x.ContentType).Returns("image/jpeg");
        file.Setup(x => x.FileName).Returns("image.jpg");
        file.Setup(x => x.OpenReadStream()).Returns(new MemoryStream());

        _storageServiceMock
            .Setup(x => x.UploadAsync(
                It.IsAny<Stream>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync("uploads/image.jpg");

        var command = new UploadProductImageCommand(product.Id, file.Object);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Success.Should().BeTrue();
    }

    [Fact]
    public async Task UploadPng_ShouldSucceed()
    {
        var product = new Product(
            "Laptop",
            "LAP-001",
            "Gaming",
            1200m,
            10,
            null,
            DateTime.UtcNow.AddDays(30));

        _productRepositoryMock
            .Setup(x => x.GetById(product.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var file = new Mock<IFormFile>();

        file.Setup(x => x.Length).Returns(1024);
        file.Setup(x => x.ContentType).Returns("image/png");
        file.Setup(x => x.FileName).Returns("image.png");
        file.Setup(x => x.OpenReadStream()).Returns(new MemoryStream());

        _storageServiceMock
            .Setup(x => x.UploadAsync(
                It.IsAny<Stream>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync("uploads/image.png");

        var command = new UploadProductImageCommand(product.Id, file.Object);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Success.Should().BeTrue();
    }

    [Fact]
    public async Task InvalidExtension_ShouldThrowException()
    {
        var product = new Product(
            "Laptop",
            "LAP-001",
            "Gaming",
            1200m,
            10,
            null,
            DateTime.UtcNow.AddDays(30));

        _productRepositoryMock
            .Setup(x => x.GetById(product.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var file = new Mock<IFormFile>();

        file.Setup(x => x.Length).Returns(1024);
        file.Setup(x => x.ContentType).Returns("text/plain");
        file.Setup(x => x.FileName).Returns("test.txt");

        var command = new UploadProductImageCommand(product.Id, file.Object);

        Func<Task> act = async () =>
            await _handler.Handle(command, CancellationToken.None);

        await act.Should()
            .ThrowAsync<Exception>()
            .WithMessage("*JPG*PNG*");
    }

    [Fact]
    public async Task OversizedFile_ShouldThrowException()
    {
        var product = new Product(
            "Laptop",
            "LAP-001",
            "Gaming",
            1200m,
            10,
            null,
            DateTime.UtcNow.AddDays(30));

        _productRepositoryMock
            .Setup(x => x.GetById(product.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var file = new Mock<IFormFile>();

        file.Setup(x => x.Length).Returns(6 * 1024 * 1024);
        file.Setup(x => x.ContentType).Returns("image/jpeg");
        file.Setup(x => x.FileName).Returns("large.jpg");

        var command = new UploadProductImageCommand(product.Id, file.Object);

        Func<Task> act = async () =>
            await _handler.Handle(command, CancellationToken.None);

        await act.Should()
            .ThrowAsync<Exception>()
            .WithMessage("*5MB*");
    }

    [Fact]
    public async Task Upload_ShouldGenerateObjectKey()
    {
        var product = new Product(
            "Laptop",
            "LAP-001",
            "Gaming",
            1200m,
            10,
            null,
            DateTime.UtcNow.AddDays(30));

        _productRepositoryMock
            .Setup(x => x.GetById(product.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var file = new Mock<IFormFile>();

        file.Setup(x => x.Length).Returns(1024);
        file.Setup(x => x.ContentType).Returns("image/jpeg");
        file.Setup(x => x.FileName).Returns("image.jpg");
        file.Setup(x => x.OpenReadStream()).Returns(new MemoryStream());

        _storageServiceMock
            .Setup(x => x.UploadAsync(
                It.IsAny<Stream>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync("products/images/image.jpg");

        var command = new UploadProductImageCommand(product.Id, file.Object);

        await _handler.Handle(command, CancellationToken.None);

        _storageServiceMock.Verify(x =>
            x.UploadAsync(
                It.IsAny<Stream>(),
                "image.jpg",
                "image/jpeg",
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}