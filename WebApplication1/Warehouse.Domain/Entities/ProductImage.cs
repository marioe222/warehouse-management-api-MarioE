using Warehouse.Domain.Exceptions;

namespace Warehouse.Domain.Entities;

public class ProductImage
{
    public Guid Id { get; private set; }

    public string ImageUrl { get; private set; }

    public Guid ProductId { get; private set; }

    public Product Product { get; private set; } = null!;


    private ProductImage()
    {
    }


    public ProductImage(string imageUrl, Guid productId)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new BusinessRuleException(
                "Image URL required"
            );


        Id = Guid.NewGuid();

        ImageUrl = imageUrl;

        ProductId = productId;
    }
}