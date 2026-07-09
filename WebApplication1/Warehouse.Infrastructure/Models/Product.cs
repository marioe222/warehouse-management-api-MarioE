using System;
using System.Collections.Generic;

namespace Warehouse.Infrastructure.Models;

public partial class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int Quantityinstock { get; set; }

    public Guid? Supplierid { get; set; }

    public virtual ICollection<Productimage> Productimages { get; set; } = new List<Productimage>();

    public virtual Supplier? Supplier { get; set; }
}
