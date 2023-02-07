using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class IdentityServerApiResourceClaim
{
    public string Type { get; set; } = null!;

    public Guid ApiResourceId { get; set; }

    public virtual IdentityServerApiResource ApiResource { get; set; } = null!;
}
