using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class PekkishProductExtraLink
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? ProductId { get; set; }

    public int? ProductExtraId { get; set; }

    public int? VendorId { get; set; }
}
