using MediatR;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Products.Queries.GetProductById;

public class GetProductByIdHandler
    : IRequestHandler<GetProductByIdQuery, GetProductByIdResponse?>
{
    private readonly IProductRepository _repository;


    public GetProductByIdHandler(
        IProductRepository repository)
    {
        _repository = repository;
    }


    public async Task<GetProductByIdResponse?> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var product = await _repository.GetById(
            request.Id,
            cancellationToken
        );


        if (product == null)
        {
            return null;
        }


        return new GetProductByIdResponse(
            product.Id,
            product.Name,
            product.Price,
            product.QuantityInStock,
            product.IsArchived,
            product.SupplierId,
            product.Supplier != null 
                ? product.Supplier.Name 
                : product.SupplierName
        );
    }
}