using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class IdentityServerClientScope
{
    public Guid ClientId { get; set; }

    public string Scope { get; set; } = null!;

    public virtual IdentityServerClient Client { get; set; } = null!;
}
