using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppWatiOrderDetailOption
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int WatiOrderDetailId { get; set; }

    public int Quantity { get; set; }
        
    public decimal Price { get; set; }

    public int ProductExtraId { get; set; }

    public int ProductExtraOptionId { get; set; }
}
