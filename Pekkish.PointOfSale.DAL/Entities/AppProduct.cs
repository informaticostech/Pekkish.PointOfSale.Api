using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppProduct
{
    public int Id { get; set; }

    public Guid? TenantId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public decimal? PriceCost { get; set; }

    public string? Description { get; set; }

    public int? OrderKitchenScreenId { get; set; }

    public int? PreperationTimeMins { get; set; }

    public bool IsEnabled { get; set; }

    public int? ProductCategoryId { get; set; }

    public int? BrandId { get; set; }

    public int? PekkishProductId { get; set; }

    public bool HasRecipe { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public Guid? LastModifierId { get; set; }

    public Guid? DeleterId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<AppOrderDetail> AppOrderDetails { get; } = new List<AppOrderDetail>();

    public virtual ICollection<AppProductExtraLink> AppProductExtraLinks { get; } = new List<AppProductExtraLink>();

    public virtual ICollection<AppProductRecipe> AppProductRecipes { get; } = new List<AppProductRecipe>();

    public virtual AppBrand? Brand { get; set; }

    public virtual AppProductCategory? ProductCategory { get; set; }
}
