using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class PekkishProductExtraOption
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? ImageUrl { get; set; }

    public int Min { get; set; }

    public int Max { get; set; }

    public int Rank { get; set; }

    public bool IsHalfOption { get; set; }

    public bool IsAllowSuboptionQuantity { get; set; }

    public bool IsLimitSuboptionByMax { get; set; }

    public bool IsEnabled { get; set; }

    public int? ProductExtraId { get; set; }

    public virtual ICollection<PekkishProductExtraOptionSub> PekkishProductExtraOptionSubs { get; } = new List<PekkishProductExtraOptionSub>();

    public virtual PekkishProductExtra? ProductExtra { get; set; }
}
