using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class IdentityServerApiResourceSecret
{
    public string Type { get; set; } = null!;

    public string Value { get; set; } = null!;

    public Guid ApiResourceId { get; set; }

    public string? Description { get; set; }

    public DateTime? Expiration { get; set; }

    public virtual IdentityServerApiResource ApiResource { get; set; } = null!;
}
