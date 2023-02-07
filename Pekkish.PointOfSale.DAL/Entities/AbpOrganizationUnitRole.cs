using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AbpOrganizationUnitRole
{
    public Guid RoleId { get; set; }

    public Guid OrganizationUnitId { get; set; }

    public Guid? TenantId { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public virtual AbpOrganizationUnit OrganizationUnit { get; set; } = null!;

    public virtual AbpRole Role { get; set; } = null!;
}
