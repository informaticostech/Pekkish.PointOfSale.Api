using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppProductExtra
{
    public int Id { get; set; }

    public Guid? TenantId { get; set; }

    public string Name { get; set; } = null!;

    public int? BrandId { get; set; }

    public int Min { get; set; }

    public int Max { get; set; }

    public bool? IsEnabled { get; set; }

    public int? PekkishExtraId { get; set; }

    public int? PekkishExtraOptionId { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public Guid? LastModifierId { get; set; }

    public Guid? DeleterId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<AppOrderDetailOption> AppOrderDetailOptions { get; } = new List<AppOrderDetailOption>();

    public virtual ICollection<AppProductExtraLink> AppProductExtraLinks { get; } = new List<AppProductExtraLink>();

    public virtual ICollection<AppProductExtraOption> AppProductExtraOptions { get; } = new List<AppProductExtraOption>();

    public virtual AppBrand? Brand { get; set; }
}
