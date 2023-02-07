using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AbpUserClaim
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid? TenantId { get; set; }

    public string ClaimType { get; set; } = null!;

    public string? ClaimValue { get; set; }

    public virtual AbpUser User { get; set; } = null!;
}
