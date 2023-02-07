using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AbpFeatureValue
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Value { get; set; } = null!;

    public string? ProviderName { get; set; }

    public string? ProviderKey { get; set; }
}
