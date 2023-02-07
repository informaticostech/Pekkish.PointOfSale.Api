using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class IdentityServerClientGrantType
{
    public Guid ClientId { get; set; }

    public string GrantType { get; set; } = null!;

    public virtual IdentityServerClient Client { get; set; } = null!;
}
