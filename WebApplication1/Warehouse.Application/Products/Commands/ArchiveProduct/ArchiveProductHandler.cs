using MediatR;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Products.Commands.ArchiveProduct;

public class ArchiveProductHandler
    : IRequestHandler<ArchiveProductCommand, ArchiveProductResponse>
{
    private readonly IProductRepository _repository;

    public ArchiveProductHandler(
        IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<ArchiveProductResponse> Handle(
        ArchiveProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _repository.GetById(
            request.ProductId, cancellationToken
        );


        if (product == null)
            return new ArchiveProductResponse(false);

        product.Archive();


        await _repository.Update(product, cancellationToken);


        return new ArchiveProductResponse(true);
    }
}