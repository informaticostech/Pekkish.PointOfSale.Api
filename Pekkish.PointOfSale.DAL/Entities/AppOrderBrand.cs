using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppOrderBrand
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int BrandId { get; set; }

    public virtual AppOrder Order { get; set; } = null!;
}
