using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AbpUserToken
{
    public Guid UserId { get; set; }

    public string LoginProvider { get; set; } = null!;

    public string Name { get; set; } = null!;

    public Guid? TenantId { get; set; }

    public string? Value { get; set; }

    public virtual AbpUser User { get; set; } = null!;
}
