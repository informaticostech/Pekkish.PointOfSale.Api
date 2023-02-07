using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppProductExtraLink
{
    public int Id { get; set; }

    public Guid? TenantId { get; set; }

    public string? Name { get; set; }

    public int? ProductId { get; set; }

    public int? ProductExtraId { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public Guid? LastModifierId { get; set; }

    public int? BrandId { get; set; }

    public virtual AppBrand? Brand { get; set; }

    public virtual AppProduct? Product { get; set; }

    public virtual AppProductExtra? ProductExtra { get; set; }
}
