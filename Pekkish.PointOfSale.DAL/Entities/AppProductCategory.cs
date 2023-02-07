using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppProductCategory
{
    public int Id { get; set; }

    public Guid? TenantId { get; set; }

    public string Name { get; set; } = null!;

    public string ExternalAppName { get; set; } = null!;

    public decimal? CostPercentageDefault { get; set; }

    public int? OrderKitchenScreenId { get; set; }

    public int? PreperationTimeMins { get; set; }

    public bool? IsEnabled { get; set; }

    public int? BrandId { get; set; }

    public int? PekkishCategoryId { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public Guid? LastModifierId { get; set; }

    public Guid? DeleterId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<AppProduct> AppProducts { get; } = new List<AppProduct>();
}
