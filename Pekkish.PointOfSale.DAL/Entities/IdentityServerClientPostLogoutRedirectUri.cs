using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class IdentityServerClientPostLogoutRedirectUri
{
    public Guid ClientId { get; set; }

    public string PostLogoutRedirectUri { get; set; } = null!;

    public virtual IdentityServerClient Client { get; set; } = null!;
}
