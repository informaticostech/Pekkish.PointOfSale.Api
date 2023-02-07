using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppOrderKitchenScreen
{
    public int Id { get; set; }

    public Guid? TenantId { get; set; }

    public string? Name { get; set; }

    public int? BrandId { get; set; }
}
