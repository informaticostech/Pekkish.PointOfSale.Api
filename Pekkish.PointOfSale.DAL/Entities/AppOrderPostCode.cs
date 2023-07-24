using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppOrderPostCode
{
    public int Id { get; set; }

    public string PostCode { get; set; } = null!;

    public string? Name { get; set; }

    public string? Suburb { get; set; }

    public string? Area { get; set; }
}
