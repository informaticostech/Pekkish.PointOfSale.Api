using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class IdentityServerClientRedirectUri
{
    public Guid ClientId { get; set; }

    public string RedirectUri { get; set; } = null!;

    public virtual IdentityServerClient Client { get; set; } = null!;
}
