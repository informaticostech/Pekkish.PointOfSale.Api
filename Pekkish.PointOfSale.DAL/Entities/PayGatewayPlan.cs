using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class PayGatewayPlan
{
    public Guid PlanId { get; set; }

    public string Gateway { get; set; } = null!;

    public string ExternalId { get; set; } = null!;

    public string? ExtraProperties { get; set; }

    public virtual PayPlan Plan { get; set; } = null!;
}
