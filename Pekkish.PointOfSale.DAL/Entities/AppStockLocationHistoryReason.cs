using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppStockLocationHistoryReason
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsManual { get; set; }
}
