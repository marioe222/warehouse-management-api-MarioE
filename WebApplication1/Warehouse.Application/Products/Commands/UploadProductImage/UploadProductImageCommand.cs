using MediatR;
using Microsoft.AspNetCore.Http;

namespace Warehouse.Application.Products.Commands.UploadProductImage;

public record UploadProductImageCommand(
    Guid ProductId,
    IFormFile File
) : IRequest<UploadProductImageResponse>;