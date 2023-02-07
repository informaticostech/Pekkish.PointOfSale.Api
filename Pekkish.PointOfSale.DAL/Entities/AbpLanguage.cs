using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AbpLanguage
{
    public Guid Id { get; set; }

    public string CultureName { get; set; } = null!;

    public string UiCultureName { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public string? FlagIcon { get; set; }

    public bool IsEnabled { get; set; }

    public string? ExtraProperties { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public Guid? LastModifierId { get; set; }

    public bool? IsDeleted { get; set; }

    public Guid? DeleterId { get; set; }

    public DateTime? DeletionTime { get; set; }
}
