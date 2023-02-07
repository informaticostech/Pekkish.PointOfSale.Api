using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppOrderRequestBrand
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int OrderRequestId { get; set; }

    public int BrandId { get; set; }

    public DateTime? PickupReadyDate { get; set; }

    public Guid? PickupReadyUser { get; set; }

    public virtual AppOrderRequest OrderRequest { get; set; } = null!;
}
