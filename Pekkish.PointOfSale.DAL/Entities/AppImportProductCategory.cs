using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppImportProductCategory
{
    public int Id { get; set; }

    public Guid? TenantId { get; set; }

    public int ExternalId { get; set; }

    public int? ExternalParentId { get; set; }

    public string? Name { get; set; }

    public decimal? Price { get; set; }

    public string? Description { get; set; }

    public bool IsEnabled { get; set; }

    public int? SalesChannelId { get; set; }

    public int? Min { get; set; }

    public int? Max { get; set; }

    public bool IsProcessed { get; set; }

    public string ProductType { get; set; } = null!;

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public Guid? DeleterId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public Guid? LastModifierId { get; set; }
}
