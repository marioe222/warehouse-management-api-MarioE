using MediatR;
using Microsoft.AspNetCore.Http;

namespace Warehouse.Application.Products.Commands;

public record UploadProductImageCommand(
    Guid ProductId,
    IFormFile File
) : IRequest<bool>;