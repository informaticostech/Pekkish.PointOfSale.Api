﻿using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppUserBrand
{
    public int Id { get; set; }

    public Guid? TenantId { get; set; }

    public Guid UserId { get; set; }

    public int? BrandId { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public Guid? DeleterId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public Guid? LastModifierId { get; set; }

    public virtual AppBrand? Brand { get; set; }
}
