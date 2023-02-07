using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class PekkishProductCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Rank { get; set; }

    public bool IsEnabled { get; set; }

    public string? ImageUrl { get; set; }

    public int? VendorId { get; set; }

    public virtual ICollection<PekkishProduct> PekkishProducts { get; } = new List<PekkishProduct>();
}
