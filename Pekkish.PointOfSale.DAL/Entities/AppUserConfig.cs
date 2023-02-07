using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppUserConfig
{
    public int Id { get; set; }

    public Guid? TenantId { get; set; }

    public Guid UserId { get; set; }

    public string HomeView { get; set; } = null!;
}
