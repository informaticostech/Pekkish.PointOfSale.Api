using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppOrderFulfillment
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? HexCodeBack { get; set; }

    public string? HexCodeFront { get; set; }

    public virtual ICollection<AppOrder> AppOrders { get; } = new List<AppOrder>();
}
