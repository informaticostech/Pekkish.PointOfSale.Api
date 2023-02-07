using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class PayPaymentRequest
{
    public Guid Id { get; set; }

    public int State { get; set; }

    public string? Currency { get; set; }

    public string? Gateway { get; set; }

    public string? FailReason { get; set; }

    public DateTime? EmailSendDate { get; set; }

    public string? ExternalSubscriptionId { get; set; }

    public string? ExtraProperties { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public virtual ICollection<PayPaymentRequestProduct> PayPaymentRequestProducts { get; } = new List<PayPaymentRequestProduct>();
}
