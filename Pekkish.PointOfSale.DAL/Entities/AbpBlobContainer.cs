using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AbpBlobContainer
{
    public Guid Id { get; set; }

    public Guid? TenantId { get; set; }

    public string Name { get; set; } = null!;

    public string? ExtraProperties { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public virtual ICollection<AbpBlob> AbpBlobs { get; } = new List<AbpBlob>();
}
