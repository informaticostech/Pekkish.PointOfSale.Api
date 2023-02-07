using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AbpUser
{
    public Guid Id { get; set; }

    public Guid? TenantId { get; set; }

    public string UserName { get; set; } = null!;

    public string NormalizedUserName { get; set; } = null!;

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string Email { get; set; } = null!;

    public string NormalizedEmail { get; set; } = null!;

    public bool? EmailConfirmed { get; set; }

    public string? PasswordHash { get; set; }

    public string SecurityStamp { get; set; } = null!;

    public bool? IsExternal { get; set; }

    public string? PhoneNumber { get; set; }

    public bool? PhoneNumberConfirmed { get; set; }

    public bool? TwoFactorEnabled { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }

    public bool? LockoutEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    public string? ExtraProperties { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public Guid? LastModifierId { get; set; }

    public bool? IsDeleted { get; set; }

    public Guid? DeleterId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public virtual ICollection<AbpUserClaim> AbpUserClaims { get; } = new List<AbpUserClaim>();

    public virtual ICollection<AbpUserLogin> AbpUserLogins { get; } = new List<AbpUserLogin>();

    public virtual ICollection<AbpUserOrganizationUnit> AbpUserOrganizationUnits { get; } = new List<AbpUserOrganizationUnit>();

    public virtual ICollection<AbpUserRole> AbpUserRoles { get; } = new List<AbpUserRole>();

    public virtual ICollection<AbpUserToken> AbpUserTokens { get; } = new List<AbpUserToken>();
}
