using System;
using System.Collections.Generic;

namespace Warehouse.Infrastructure.Models;

public partial class Product
{
    public int Productid { get; set; }

    public string Name { get; set; } = null!;

    public decimal? Price { get; set; }

    public int? Quantity { get; set; }

    public DateOnly? Expirydate { get; set; }

    public DateTime? Createdat { get; set; }

    public int? Supplierid { get; set; }

    public virtual ICollection<Productimage> Productimages { get; set; } = new List<Productimage>();

    public virtual Supplier? Supplier { get; set; }
}
