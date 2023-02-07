using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class PayPaymentRequestProduct
{
    public Guid PaymentRequestId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public float UnitPrice { get; set; }

    public int Count { get; set; }

    public float TotalPrice { get; set; }

    public byte PaymentType { get; set; }

    public Guid? PlanId { get; set; }

    public string? ExtraProperties { get; set; }

    public virtual PayPaymentRequest PaymentRequest { get; set; } = null!;
}
