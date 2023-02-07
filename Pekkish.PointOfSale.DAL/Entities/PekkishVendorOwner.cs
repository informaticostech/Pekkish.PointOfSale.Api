using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class PekkishVendorOwner
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public bool IsEnabled { get; set; }

    public string? EmailAddress { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public string? ApiKey { get; set; }
}
