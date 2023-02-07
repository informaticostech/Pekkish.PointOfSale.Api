using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppProductExtraOption
{
    public int Id { get; set; }

    public Guid? TenantId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public bool HasRecipe { get; set; }

    public bool IsEnabled { get; set; }

    public int? ProductExtraId { get; set; }

    public int? PekkishSuboptionId { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public Guid? LastModifierId { get; set; }

    public int? BrandId { get; set; }

    public Guid? DeleterId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<AppOrderDetailOption> AppOrderDetailOptions { get; } = new List<AppOrderDetailOption>();

    public virtual AppBrand? Brand { get; set; }

    public virtual AppProductExtra? ProductExtra { get; set; }
}
