using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppSeatingArea
{
    public int Id { get; set; }

    public Guid? TenantId { get; set; }

    public string Name { get; set; } = null!;

    public int? LocationId { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid CreatorId { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletionTime { get; set; }

    public Guid? DeleterId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public Guid? LastModifierId { get; set; }
}
