using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class PekkishProductExtra
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsEnabled { get; set; }

    public int? VendorId { get; set; }

    public virtual ICollection<PekkishProductExtraOption> PekkishProductExtraOptions { get; } = new List<PekkishProductExtraOption>();
}
