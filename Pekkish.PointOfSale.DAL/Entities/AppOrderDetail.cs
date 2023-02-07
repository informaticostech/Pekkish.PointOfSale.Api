using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppOrderDetail
{
    public int Id { get; set; }

    public Guid? TenantId { get; set; }

    public int Orderid { get; set; }

    public int OrderRequestCrono { get; set; }

    public int? OrderRequestId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Amount { get; set; }

    public decimal AmountBase { get; set; }

    public int Quantity { get; set; }

    public string? Comment { get; set; }

    public int? ProductId { get; set; }

    public int? BrandId { get; set; }

    public decimal AmountNormal { get; set; }

    public decimal AmountBaseNormal { get; set; }

    public decimal RateIncrease { get; set; }

    public decimal? DiscountRate { get; set; }

    public decimal? DiscountValue { get; set; }

    public decimal? AmountNoDiscount { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public Guid? LastModifierId { get; set; }

    public Guid? DeleterId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public bool? IsDeleted { get; set; }

    public int? AppOrderId { get; set; }

    public virtual ICollection<AppOrderDetailOption> AppOrderDetailOptions { get; } = new List<AppOrderDetailOption>();

    public virtual AppOrder Order { get; set; } = null!;

    public virtual AppProduct? Product { get; set; }
}
