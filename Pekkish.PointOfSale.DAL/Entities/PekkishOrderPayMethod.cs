using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class PekkishOrderPayMethod
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
}
