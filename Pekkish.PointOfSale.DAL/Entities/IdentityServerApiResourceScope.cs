using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class IdentityServerApiResourceScope
{
    public Guid ApiResourceId { get; set; }

    public string Scope { get; set; } = null!;

    public virtual IdentityServerApiResource ApiResource { get; set; } = null!;
}
