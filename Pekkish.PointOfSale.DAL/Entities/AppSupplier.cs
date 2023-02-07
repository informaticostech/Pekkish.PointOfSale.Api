using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppSupplier
{
    public int Id { get; set; }

    public Guid? TenantId { get; set; }

    public string Name { get; set; } = null!;

    public string? StreetAddress { get; set; }

    public string? Suburb { get; set; }

    public string? City { get; set; }

    public string? PostCode { get; set; }

    public string? EmailAddress { get; set; }

    public int FinanceVatTypeId { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public Guid? LastModifierId { get; set; }

    public Guid? DeleterId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<AppExpense> AppExpenses { get; } = new List<AppExpense>();

    public virtual ICollection<AppStockPurchaseHeader> AppStockPurchaseHeaders { get; } = new List<AppStockPurchaseHeader>();
}
