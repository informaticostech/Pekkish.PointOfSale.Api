using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppWatiOrderDetail
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int WatiOrderId { get; set; }

    public int BrandId { get; set; }

    public int ProductId { get; set; }

    public decimal Amount { get; set; }

    public int Quantity { get; set; }

    public string? Comment { get; set; }
}
