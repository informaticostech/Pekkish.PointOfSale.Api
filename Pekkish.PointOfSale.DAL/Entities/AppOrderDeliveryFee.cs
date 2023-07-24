using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppOrderDeliveryFee
{
    public int Id { get; set; }

    public Guid? TenantId { get; set; }

    public string PostCode { get; set; } = null!;

    public decimal DeliveryFee { get; set; }

    public decimal? FreeDeliveryMin { get; set; }
}
