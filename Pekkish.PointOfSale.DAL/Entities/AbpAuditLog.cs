using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AbpAuditLog
{
    public Guid Id { get; set; }

    public string? ApplicationName { get; set; }

    public Guid? UserId { get; set; }

    public string? UserName { get; set; }

    public Guid? TenantId { get; set; }

    public string? TenantName { get; set; }

    public Guid? ImpersonatorUserId { get; set; }

    public Guid? ImpersonatorTenantId { get; set; }

    public DateTime ExecutionTime { get; set; }

    public int ExecutionDuration { get; set; }

    public string? ClientIpAddress { get; set; }

    public string? ClientName { get; set; }

    public string? ClientId { get; set; }

    public string? CorrelationId { get; set; }

    public string? BrowserInfo { get; set; }

    public string? HttpMethod { get; set; }

    public string? Url { get; set; }

    public string? Exceptions { get; set; }

    public string? Comments { get; set; }

    public int? HttpStatusCode { get; set; }

    public string? ExtraProperties { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public virtual ICollection<AbpAuditLogAction> AbpAuditLogActions { get; } = new List<AbpAuditLogAction>();

    public virtual ICollection<AbpEntityChange> AbpEntityChanges { get; } = new List<AbpEntityChange>();
}
