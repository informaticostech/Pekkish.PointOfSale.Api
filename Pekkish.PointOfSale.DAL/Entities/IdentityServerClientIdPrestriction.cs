using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class IdentityServerClientIdPrestriction
{
    public Guid ClientId { get; set; }

    public string Provider { get; set; } = null!;

    public virtual IdentityServerClient Client { get; set; } = null!;
}
