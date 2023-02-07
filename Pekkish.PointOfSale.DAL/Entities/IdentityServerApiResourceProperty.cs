using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class IdentityServerApiResourceProperty
{
    public Guid ApiResourceId { get; set; }

    public string Key { get; set; } = null!;

    public string Value { get; set; } = null!;

    public virtual IdentityServerApiResource ApiResource { get; set; } = null!;
}
