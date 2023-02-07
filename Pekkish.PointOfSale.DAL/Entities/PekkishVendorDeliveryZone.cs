using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class PekkishVendorDeliveryZone
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int Type { get; set; }

    public decimal Price { get; set; }

    public decimal Minimum { get; set; }

    public bool IsEnabled { get; set; }

    public int? VendorId { get; set; }
}
