using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppOrderPrintQueue
{
    public int Id { get; set; }

    public Guid? TenantId { get; set; }

    public int OrderId { get; set; }

    public int OrderRequestId { get; set; }

    public int? LocationId { get; set; }

    public int BrandId { get; set; }

    public string PrinterName { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public Guid CreatedUser { get; set; }

    public DateTime? PrintedDate { get; set; }
}
