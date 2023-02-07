using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class PekkishDeliveryGroup
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string EmailAddress { get; set; } = null!;

    public DateTime? LastWeeklyReport { get; set; }

    public string? ApiKey { get; set; }

    public int? DeliveryManagerId { get; set; }

    public bool IsDeliveryCommission { get; set; }
}
