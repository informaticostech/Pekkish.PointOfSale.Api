using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class PekkishExpense
{
    public int Id { get; set; }

    public int? VendorId { get; set; }

    public int? SupplierId { get; set; }

    public string? Description { get; set; }

    public DateTime EffectiveDate { get; set; }

    public string? InvoiceNumber { get; set; }

    public decimal SubTotal { get; set; }

    public decimal VatRate { get; set; }

    public decimal Vat { get; set; }

    public decimal Total { get; set; }

    public string? Status { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public Guid? DeleterId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public Guid? LastModifierId { get; set; }
}
