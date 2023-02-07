using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class IdentityServerApiScopeClaim
{
    public string Type { get; set; } = null!;

    public Guid ApiScopeId { get; set; }

    public virtual IdentityServerApiScope ApiScope { get; set; } = null!;
}
