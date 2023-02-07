using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppProductRecipe
{
    public int Id { get; set; }

    public Guid? TenantId { get; set; }

    public int? ProductId { get; set; }

    public int? ProductExtraOptionId { get; set; }

    public int StockId { get; set; }

    public string Unit { get; set; } = null!;

    public int Quantity { get; set; }

    public string? AdditionalInfo { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public Guid? LastModifierId { get; set; }

    public Guid? DeleterId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public bool IsDeleted { get; set; }

    public virtual AppProduct? Product { get; set; }

    public virtual AppStock Stock { get; set; } = null!;
}
