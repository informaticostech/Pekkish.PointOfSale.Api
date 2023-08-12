using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppLocation
{
    public int Id { get; set; }

    public Guid? TenantId { get; set; }

    public string Name { get; set; } = null!;

    public string AddressLine1 { get; set; } = null!;

    public string? AddressLine2 { get; set; }

    public string? City { get; set; }

    public string? PostCode { get; set; }

    public string? EmailAddress { get; set; }

    public string? WhatsApp { get; set; }

    public string? PrintPassPhrase { get; set; }

    public string? PrintServerUrl { get; set; }

    public string PrinterName { get; set; } = null!;

    public string? ApiKeyPekkish { get; set; }

    public int? PekkishVendorId { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public Guid? LastModifierId { get; set; }

    public Guid? DeleterId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<AppLocationBrand> AppLocationBrands { get; } = new List<AppLocationBrand>();

    public virtual ICollection<AppOrder> AppOrders { get; } = new List<AppOrder>();

    public virtual ICollection<AppStockLocation> AppStockLocations { get; } = new List<AppStockLocation>();

    public virtual ICollection<AppStockPurchaseHeader> AppStockPurchaseHeaders { get; } = new List<AppStockPurchaseHeader>();

    public virtual ICollection<AppUserLocation> AppUserLocations { get; } = new List<AppUserLocation>();

    public virtual ICollection<AppWatiOrder> AppWatiOrders { get; } = new List<AppWatiOrder>();
}
