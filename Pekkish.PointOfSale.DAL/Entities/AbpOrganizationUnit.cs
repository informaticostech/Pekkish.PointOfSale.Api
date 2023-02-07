using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AbpOrganizationUnit
{
    public Guid Id { get; set; }

    public Guid? TenantId { get; set; }

    public Guid? ParentId { get; set; }

    public string Code { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public string? ExtraProperties { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public Guid? LastModifierId { get; set; }

    public bool? IsDeleted { get; set; }

    public Guid? DeleterId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public virtual ICollection<AbpOrganizationUnitRole> AbpOrganizationUnitRoles { get; } = new List<AbpOrganizationUnitRole>();

    public virtual ICollection<AbpUserOrganizationUnit> AbpUserOrganizationUnits { get; } = new List<AbpUserOrganizationUnit>();

    public virtual ICollection<AbpOrganizationUnit> InverseParent { get; } = new List<AbpOrganizationUnit>();

    public virtual AbpOrganizationUnit? Parent { get; set; }
}
