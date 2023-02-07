using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppStock
{
    public int Id { get; set; }

    public Guid? TenantId { get; set; }

    public string Name { get; set; } = null!;

    public int StockCategoryId { get; set; }

    public int StockUnitId { get; set; }

    public decimal? UnitPrice { get; set; }

    public int? QuantityMinimum { get; set; }

    public int Quantity { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public Guid? LastModifierId { get; set; }

    public Guid? DeleterId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<AppProductRecipe> AppProductRecipes { get; } = new List<AppProductRecipe>();

    public virtual ICollection<AppStockLocation> AppStockLocations { get; } = new List<AppStockLocation>();

    public virtual ICollection<AppStockPurchaseDetail> AppStockPurchaseDetails { get; } = new List<AppStockPurchaseDetail>();

    public virtual AppStockCategory StockCategory { get; set; } = null!;

    public virtual AppStockUnit StockUnit { get; set; } = null!;
}
