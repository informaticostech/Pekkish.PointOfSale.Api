using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AbpPermissionGrant
{
    public Guid Id { get; set; }

    public Guid? TenantId { get; set; }

    public string Name { get; set; } = null!;

    public string ProviderName { get; set; } = null!;

    public string ProviderKey { get; set; } = null!;
}
