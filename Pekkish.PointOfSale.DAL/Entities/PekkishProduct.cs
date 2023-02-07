using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class PekkishProduct
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsInventory { get; set; }

    public int Quantity { get; set; }

    public bool IsFeatured { get; set; }

    public bool IsEnabled { get; set; }

    public int? VendorId { get; set; }

    public int? ProductCategoryId { get; set; }

    public virtual PekkishProductCategory? ProductCategory { get; set; }
}
