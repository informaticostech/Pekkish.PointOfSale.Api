using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class PekkishDriverGroupVendor
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? DriverGroupId { get; set; }

    public int? VendorId { get; set; }
}
