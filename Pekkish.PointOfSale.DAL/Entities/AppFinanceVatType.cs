using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppFinanceVatType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal VatRate { get; set; }
}
