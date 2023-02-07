using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class PekkishDriverRoster
{
    public int Id { get; set; }

    public DateTime ReferenceDate { get; set; }

    public int? DriverId { get; set; }

    public int? DeliveryManagerId { get; set; }
}
