using System;
using System.Collections.Generic;

namespace Warehouse.Infrastructure.Models;

public partial class Productimage
{
    public Guid Id { get; set; }

    public Guid Productid { get; set; }

    public string Imageurl { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
