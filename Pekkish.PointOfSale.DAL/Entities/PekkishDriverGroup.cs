using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class PekkishDriverGroup
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? DeliveryManagerId { get; set; }

    public int Type { get; set; }

    public bool IsEnabled { get; set; }
}
