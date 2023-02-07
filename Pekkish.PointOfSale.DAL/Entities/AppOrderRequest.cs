using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppOrderRequest
{
    public int Id { get; set; }

    public Guid? TenantId { get; set; }

    public int OrderId { get; set; }

    public int Iteration { get; set; }

    public int OrderStatusId { get; set; }

    public string? Brand { get; set; }

    public decimal Total { get; set; }

    public DateTime? PickupReadyDate { get; set; }

    public Guid? PickupReadyUser { get; set; }

    public DateTime? CompletedDate { get; set; }

    public Guid? CompletedUser { get; set; }

    public DateTime? RejectDate { get; set; }

    public Guid? RejectUser { get; set; }

    public string? RejectReason { get; set; }

    public bool IsMultiBrand { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public Guid? LastModifierId { get; set; }

    public Guid? DeleterId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<AppOrderRequestBrand> AppOrderRequestBrands { get; } = new List<AppOrderRequestBrand>();

    public virtual AppOrder Order { get; set; } = null!;

    public virtual AppOrderStatus OrderStatus { get; set; } = null!;
}
