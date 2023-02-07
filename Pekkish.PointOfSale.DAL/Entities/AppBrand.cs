using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppBrand
{
    public int Id { get; set; }

    public Guid? TenantId { get; set; }

    public string Name { get; set; } = null!;

    public string? NameShort { get; set; }

    public string? EmailAddress { get; set; }

    public int? PreperationTimeMins { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public Guid? DeleterId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public Guid? LastModifierId { get; set; }

    public bool? IsEnabled { get; set; }

    public virtual ICollection<AppLocationBrand> AppLocationBrands { get; } = new List<AppLocationBrand>();

    public virtual ICollection<AppProductExtraLink> AppProductExtraLinks { get; } = new List<AppProductExtraLink>();

    public virtual ICollection<AppProductExtraOption> AppProductExtraOptions { get; } = new List<AppProductExtraOption>();

    public virtual ICollection<AppProductExtra> AppProductExtras { get; } = new List<AppProductExtra>();

    public virtual ICollection<AppProduct> AppProducts { get; } = new List<AppProduct>();

    public virtual ICollection<AppRateSheet> AppRateSheets { get; } = new List<AppRateSheet>();

    public virtual ICollection<AppUserBrand> AppUserBrands { get; } = new List<AppUserBrand>();
}
