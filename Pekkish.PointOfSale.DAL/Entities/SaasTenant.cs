using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class SaasTenant
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid? EditionId { get; set; }

    public DateTime? EditionEndDateUtc { get; set; }

    public byte ActivationState { get; set; }

    public DateTime? ActivationEndDate { get; set; }

    public string? ExtraProperties { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public Guid? LastModifierId { get; set; }

    public bool? IsDeleted { get; set; }

    public Guid? DeleterId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public virtual ICollection<SaasTenantConnectionString> SaasTenantConnectionStrings { get; } = new List<SaasTenantConnectionString>();
}
