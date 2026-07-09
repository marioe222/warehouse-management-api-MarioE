using Warehouse.Domain.Entities;

namespace Warehouse.Domain.Interface;

public interface IProductService    
{
    Task<IEnumerable<Product>> GetProductsBySupplier(
        string supplierName,
        string sort);


    Task<object> GroupProductsByExpiryYear();


    Task<object> GroupProductsByExpiryYearAndCountry();


    Task<int> GetTotalProducts();


    Task<IEnumerable<Product>> GetProductsPagination(
        int pageNumber,
        int pageSize);
}