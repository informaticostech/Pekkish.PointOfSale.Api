using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class PekkishOrderHistory
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int RowId { get; set; }

    public int HistoryTypeId { get; set; }

    public string? Attribute { get; set; }

    public int? ValueOld { get; set; }

    public int? ValueNew { get; set; }

    public string? Comment { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public virtual PekkishOrder Order { get; set; } = null!;
}
