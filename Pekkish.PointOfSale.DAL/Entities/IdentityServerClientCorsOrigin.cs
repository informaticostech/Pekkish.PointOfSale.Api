using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class IdentityServerClientCorsOrigin
{
    public Guid ClientId { get; set; }

    public string Origin { get; set; } = null!;

    public virtual IdentityServerClient Client { get; set; } = null!;
}
