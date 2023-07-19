using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppTenantInfo
{
    public int Id { get; set; }

    public Guid? TenantId { get; set; }

    public string Name { get; set; } = null!;

    public string NameShort { get; set; } = null!;

    public bool IsMultiBrand { get; set; }

    public bool IsActiveStock { get; set; }

    public bool IsActiveExpense { get; set; }

    public bool IsActivePekkish { get; set; }

    public bool? IsActiveWhatsApp { get; set; }

    public bool? IsWhatsAppDelivery { get; set; }

    public string? WelcomeMessage { get; set; }

    public string? StoreNotice { get; set; }

    public string? LabelBrand { get; set; }

    public string? LabelCategory { get; set; }

    public virtual ICollection<AppWatiOrder> AppWatiOrders { get; } = new List<AppWatiOrder>();
}
