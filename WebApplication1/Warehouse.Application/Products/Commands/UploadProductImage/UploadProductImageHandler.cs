using MediatR;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Products.Commands.UploadProductImage;

public class UploadProductImageHandler
    : IRequestHandler<UploadProductImageCommand, UploadProductImageResponse>
{
    private readonly IProductRepository _repository;

    public UploadProductImageHandler(
        IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<UploadProductImageResponse> Handle(
        UploadProductImageCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _repository.GetById(request.ProductId, cancellationToken);


        if (product == null) return new UploadProductImageResponse(false);

        return new UploadProductImageResponse(true);
    }
}