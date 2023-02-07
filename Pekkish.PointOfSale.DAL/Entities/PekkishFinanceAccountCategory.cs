using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class PekkishFinanceAccountCategory
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }
}
