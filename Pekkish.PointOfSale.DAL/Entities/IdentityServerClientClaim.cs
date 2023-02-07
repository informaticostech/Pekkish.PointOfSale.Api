using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class IdentityServerClientClaim
{
    public Guid ClientId { get; set; }

    public string Type { get; set; } = null!;

    public string Value { get; set; } = null!;

    public virtual IdentityServerClient Client { get; set; } = null!;
}
