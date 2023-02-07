using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class PekkishSupplier
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? StreetAddress { get; set; }

    public string? Suburb { get; set; }

    public string? City { get; set; }

    public string? PostCode { get; set; }

    public string? Email { get; set; }

    public int? VendorId { get; set; }

    public int? FinanceVatTypeId { get; set; }
}
