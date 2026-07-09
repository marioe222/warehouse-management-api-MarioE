using System;
using System.Collections.Generic;

namespace Warehouse.Infrastructure.Models;

public partial class Supplier
{
    public int Supplierid { get; set; }

    public string Name { get; set; } = null!;

    public string? Country { get; set; }

    public string? Contactemail { get; set; }

    public string? Phonenumber { get; set; }

    public bool? Isactive { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
