using MediatR;
using Warehouse.Application.Products.Queries;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Products.Handlers;


public class GetProductByIdHandler 
    : IRequestHandler<GetProductByIdQuery, object?>
{

    private readonly IProductRepository _repository;


    public GetProductByIdHandler(
        IProductRepository repository)
    {
        _repository = repository;
    }



    public async Task<object?> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {

        var product =
            await _repository.GetById(request.Id);


        return product;
    }
}