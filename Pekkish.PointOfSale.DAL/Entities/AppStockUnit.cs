using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppStockUnit
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<AppStock> AppStocks { get; } = new List<AppStock>();
}
