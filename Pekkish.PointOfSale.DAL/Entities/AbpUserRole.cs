using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AbpUserRole
{
    public Guid UserId { get; set; }

    public Guid RoleId { get; set; }

    public Guid? TenantId { get; set; }

    public virtual AbpRole Role { get; set; } = null!;

    public virtual AbpUser User { get; set; } = null!;
}
