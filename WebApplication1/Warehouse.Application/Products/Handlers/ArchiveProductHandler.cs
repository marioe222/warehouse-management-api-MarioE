using MediatR;
using Warehouse.Application.Products.Commands;
using Warehouse.Domain.Interface;

namespace Warehouse.Application.Products.Handlers;


public class ArchiveProductHandler
    : IRequestHandler<ArchiveProductCommand, bool>
{

    private readonly IProductRepository _repository;



    public ArchiveProductHandler(
        IProductRepository repository)
    {
        _repository = repository;
    }




    public async Task<bool> Handle(
        ArchiveProductCommand request,
        CancellationToken cancellationToken)
    {

        var product = await _repository.GetById(
            request.ProductId
        );


        if(product == null)
            return false;



        product.Archive();


        await _repository.Update(product);


        return true;
    }
}