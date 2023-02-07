using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class PekkishReview
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int Quality { get; set; }

    public int Delivery { get; set; }

    public int Service { get; set; }

    public int Package { get; set; }

    public string Comment { get; set; } = null!;

    public bool IsEnabled { get; set; }

    public int? VendorId { get; set; }

    public int? PekkishUserId { get; set; }
}
