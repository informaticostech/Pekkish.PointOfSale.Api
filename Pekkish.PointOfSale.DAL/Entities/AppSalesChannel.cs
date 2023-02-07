using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppSalesChannel
{
    public int Id { get; set; }

    public Guid? TenantId { get; set; }

    public string Name { get; set; } = null!;

    public string? HexColourCode { get; set; }

    public string? HexColourCodeText { get; set; }

    public bool? IsEnabled { get; set; }

    public bool? IsShowNewOrder { get; set; }

    public bool? IsMultiBrand { get; set; }

    public bool IsExternal { get; set; }

    public bool? IsExternalPayment { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public Guid? DeleterId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public Guid? LastModifierId { get; set; }

    public virtual ICollection<AppOrder> AppOrders { get; } = new List<AppOrder>();

    public virtual ICollection<AppRateSheet> AppRateSheets { get; } = new List<AppRateSheet>();
}
