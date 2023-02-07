using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class PekkishOrderDetail
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public string? Name { get; set; }

    public decimal Amount { get; set; }

    public decimal AmountBase { get; set; }

    public int Quantity { get; set; }

    public string? Comment { get; set; }

    public int? ProductId { get; set; }

    public virtual PekkishOrder Order { get; set; } = null!;

    public virtual ICollection<PekkishOrderDetailOption> PekkishOrderDetailOptions { get; } = new List<PekkishOrderDetailOption>();
}
