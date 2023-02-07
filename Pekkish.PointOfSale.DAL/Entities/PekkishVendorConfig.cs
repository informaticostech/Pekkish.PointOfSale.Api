using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class PekkishVendorConfig
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;

    public bool IsWeeklyReport { get; set; }

    public DateTime? LastUpdated { get; set; }

    public DateTime? LastOrdersUpdated { get; set; }

    public DateTime? LastWeeklyReport { get; set; }

    public DateTime? LastMonthlyReport { get; set; }

    public int? VendorTypeId { get; set; }

    public string? NameShort { get; set; }

    public string? ApiKey { get; set; }

    public int? VendorTermId { get; set; }

    public decimal? DeliveryCommission { get; set; }
}
