using System;
using System.Collections.Generic;

namespace Warehouse.Infrastructure.Models;

public partial class Supplier
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public bool Isactive { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
