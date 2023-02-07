using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppPaymentMethod
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool? IsExternal { get; set; }

    public string? HexCodeBack { get; set; }

    public string? HexCodeFront { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public Guid? DeleterId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public Guid? LastModifierId { get; set; }

    public virtual ICollection<AppOrder> AppOrders { get; } = new List<AppOrder>();

    public virtual ICollection<AppWatiOrder> AppWatiOrders { get; } = new List<AppWatiOrder>();
}
