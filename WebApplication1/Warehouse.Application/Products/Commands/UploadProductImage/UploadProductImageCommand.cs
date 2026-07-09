using MediatR;
using Microsoft.AspNetCore.Http;

namespace Warehouse.Application.Products.Commands.UploadProductImage;

public record UploadProductImageCommand(
    int ProductId,
    IFormFile File
) : IRequest<UploadProductImageResponse>;