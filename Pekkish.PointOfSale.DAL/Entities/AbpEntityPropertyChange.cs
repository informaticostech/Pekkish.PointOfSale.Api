using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AbpEntityPropertyChange
{
    public Guid Id { get; set; }

    public Guid? TenantId { get; set; }

    public Guid EntityChangeId { get; set; }

    public string? NewValue { get; set; }

    public string? OriginalValue { get; set; }

    public string PropertyName { get; set; } = null!;

    public string PropertyTypeFullName { get; set; } = null!;

    public virtual AbpEntityChange EntityChange { get; set; } = null!;
}
