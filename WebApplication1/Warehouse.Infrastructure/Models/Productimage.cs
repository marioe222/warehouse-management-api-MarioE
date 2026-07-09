using System;
using System.Collections.Generic;

namespace Warehouse.Infrastructure.Models;

public partial class Productimage
{
    public int Productimageid { get; set; }

    public string? Imageurl { get; set; }

    public int? Productid { get; set; }

    public virtual Product? Product { get; set; }
}
