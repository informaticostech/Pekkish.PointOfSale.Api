using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class IdentityServerIdentityResourceClaim
{
    public string Type { get; set; } = null!;

    public Guid IdentityResourceId { get; set; }

    public virtual IdentityServerIdentityResource IdentityResource { get; set; } = null!;
}
