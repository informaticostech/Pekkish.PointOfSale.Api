using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class SaasTenantConnectionString
{
    public Guid TenantId { get; set; }

    public string Name { get; set; } = null!;

    public string Value { get; set; } = null!;

    public virtual SaasTenant Tenant { get; set; } = null!;
}
