using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AbpClaimType
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public bool Required { get; set; }

    public bool IsStatic { get; set; }

    public string? Regex { get; set; }

    public string? RegexDescription { get; set; }

    public string? Description { get; set; }

    public int ValueType { get; set; }

    public string? ExtraProperties { get; set; }

    public string? ConcurrencyStamp { get; set; }
}
