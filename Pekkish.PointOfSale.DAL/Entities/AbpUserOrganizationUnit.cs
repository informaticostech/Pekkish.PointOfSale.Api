using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AbpUserOrganizationUnit
{
    public Guid UserId { get; set; }

    public Guid OrganizationUnitId { get; set; }

    public Guid? TenantId { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public virtual AbpOrganizationUnit OrganizationUnit { get; set; } = null!;

    public virtual AbpUser User { get; set; } = null!;
}
