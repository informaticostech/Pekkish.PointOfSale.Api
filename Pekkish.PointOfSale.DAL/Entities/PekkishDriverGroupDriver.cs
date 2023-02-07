using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class PekkishDriverGroupDriver
{
    public int Id { get; set; }

    public int? Driver { get; set; }

    public int? DriverGroupId { get; set; }

    public int? DriverId { get; set; }
}
