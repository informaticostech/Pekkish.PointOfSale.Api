using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppOrderDetailOption
{
    public int Id { get; set; }

    public Guid? TenantId { get; set; }

    public int OrderDetailId { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public int? ProductExtraId { get; set; }

    public int? ProductExtraOptionId { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public Guid? LastModifierId { get; set; }

    public virtual AppOrderDetail OrderDetail { get; set; } = null!;

    public virtual AppProductExtra? ProductExtra { get; set; }

    public virtual AppProductExtraOption? ProductExtraOption { get; set; }
}
