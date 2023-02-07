using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class PekkishProductExtraOptionSub
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? ImageUrl { get; set; }

    public decimal Price { get; set; }

    public int Rank { get; set; }

    public string? Description { get; set; }

    public int Max { get; set; }

    public decimal? HalfPrice { get; set; }

    public bool IsEnabled { get; set; }

    public int? ProductExtraOptionId { get; set; }

    public virtual PekkishProductExtraOption? ProductExtraOption { get; set; }
}
