using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppExpenseDetail
{
    public int Id { get; set; }

    public Guid? TenantId { get; set; }

    public string? Name { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal DiscountPerc { get; set; }

    public decimal Discount { get; set; }

    public decimal Exclusive { get; set; }

    public decimal VatPerc { get; set; }

    public decimal Vat { get; set; }

    public decimal Total { get; set; }

    public int ExpenseId { get; set; }

    public int? FinanceAccountId { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public Guid? LastModifierId { get; set; }

    public Guid? DeleterId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public bool IsDeleted { get; set; }

    public virtual AppFinanceAccount? FinanceAccount { get; set; }
}
