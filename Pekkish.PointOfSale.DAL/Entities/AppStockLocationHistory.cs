using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppStockLocationHistory
{
    public int Id { get; set; }

    public Guid? TenantId { get; set; }

    public string? Reason { get; set; }

    public int? QuantityChange { get; set; }

    public int? OrderId { get; set; }

    public int? PurchaseId { get; set; }

    public int? PurchaseDetailId { get; set; }

    public int? StockId { get; set; }

    public int? LocationId { get; set; }

    public int? StockLocationHistoryReasonId { get; set; }

    public int CurrentLocationQuantity { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public Guid? LastModifierId { get; set; }

    public Guid? DeleterId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public bool IsDeleted { get; set; }
}
