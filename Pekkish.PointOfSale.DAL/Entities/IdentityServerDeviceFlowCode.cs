using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class IdentityServerDeviceFlowCode
{
    public Guid Id { get; set; }

    public string DeviceCode { get; set; } = null!;

    public string UserCode { get; set; } = null!;

    public string? SubjectId { get; set; }

    public string? SessionId { get; set; }

    public string ClientId { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime Expiration { get; set; }

    public string Data { get; set; } = null!;

    public string? ExtraProperties { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }
}
