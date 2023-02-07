using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class IdentityServerPersistedGrant
{
    public string Key { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? SubjectId { get; set; }

    public string? SessionId { get; set; }

    public string ClientId { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreationTime { get; set; }

    public DateTime? Expiration { get; set; }

    public DateTime? ConsumedTime { get; set; }

    public string Data { get; set; } = null!;

    public Guid Id { get; set; }

    public string? ExtraProperties { get; set; }

    public string? ConcurrencyStamp { get; set; }
}
