using Warehouse.Domain.Entities;

namespace Warehouse.Domain.Interface;

public interface IProductService
{
    List<Product> GetAll(bool onlyAvailable);

    Product? GetById(Guid id);

    bool UpdatePrice(Guid id, decimal price);

    bool UpdateQuantity(Guid id, int quantity);
}