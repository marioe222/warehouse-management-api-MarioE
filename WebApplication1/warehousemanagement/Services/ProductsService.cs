using warehousemanagement.Data;
using warehousemanagement.modules;

namespace warehousemanagement.Services
{
    public class ProductService
    {
        public List<Products> GetAll(bool onlyAvailable)
        {
            var query = FakeWarehouseStore.Products
                .Where(p => !p.IsArchived);

            if (onlyAvailable)
                query = query.Where(p => p.QuantityInStock > 0);

            return query
                .OrderByDescending(p => p.CreatedAt)
                .ToList();
        }

        public Products? GetById(Guid id)
        {
            return FakeWarehouseStore.Products
                .FirstOrDefault(p => p.Id == id && !p.IsArchived);
        }

        public bool UpdatePrice(Guid id, decimal price)
        {
            var product = GetById(id);
            if (product == null) return false;

            product.Price = price;
            product.LastUpdatedAt = DateTime.UtcNow;
            return true;
        }

        public bool UpdateQuantity(Guid id, int quantity)
        {
            var product = GetById(id);
            if (product == null) return false;

            product.QuantityInStock = quantity;
            product.LastUpdatedAt = DateTime.UtcNow;
            return true;
        }
    }
}