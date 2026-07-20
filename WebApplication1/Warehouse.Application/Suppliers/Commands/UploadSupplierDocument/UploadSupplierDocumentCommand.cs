using MediatR;
using Microsoft.AspNetCore.Http;

namespace Warehouse.Application.Suppliers.Commands.UploadSupplierDocument;

public record UploadSupplierDocumentCommand(
    Guid SupplierId,
    IFormFile File
) : IRequest<UploadSupplierDocumentResponse>;