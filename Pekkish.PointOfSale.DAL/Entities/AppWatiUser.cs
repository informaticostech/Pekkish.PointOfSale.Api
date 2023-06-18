using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppWatiUser
{
    public int Id { get; set; }    

    public string WaId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? AddressStreet { get; set; }

    public string? AddressSuburb { get; set; }

    public string? AddressPostCode { get; set; }

    public string? EmailAddress { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public Guid? LastModifierId { get; set; }

    public Guid? DeleterId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public bool IsDeleted { get; set; }
}
