using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class PekkishDeliveryManager
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? EmailAddress { get; set; }

    public bool IsEnabled { get; set; }

    public DateTime? LastWeeklyReport { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? ApiKey { get; set; }
}
