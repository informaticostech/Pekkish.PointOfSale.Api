using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppStockLocation
{
    public int Id { get; set; }

    public Guid? TenantId { get; set; }

    public int Quantity { get; set; }

    public int StockId { get; set; }

    public int LocationId { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public Guid? LastModifierId { get; set; }

    public Guid? DeleterId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public bool IsDeleted { get; set; }

    public virtual AppLocation Location { get; set; } = null!;

    public virtual AppStock Stock { get; set; } = null!;
}
