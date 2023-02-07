using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AbpRoleClaim
{
    public Guid Id { get; set; }

    public Guid RoleId { get; set; }

    public Guid? TenantId { get; set; }

    public string ClaimType { get; set; } = null!;

    public string? ClaimValue { get; set; }

    public virtual AbpRole Role { get; set; } = null!;
}
