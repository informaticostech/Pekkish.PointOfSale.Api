using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class PekkishOrderDetailOption
{
    public int Id { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public int ProductExtraOptionId { get; set; }

    public int ProductExtraOptionSubId { get; set; }

    public virtual PekkishOrderDetail IdNavigation { get; set; } = null!;
}
