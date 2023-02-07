using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppOrderStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? OrderQueuStatusId { get; set; }

    public virtual ICollection<AppOrderRequest> AppOrderRequests { get; } = new List<AppOrderRequest>();

    public virtual ICollection<AppOrder> AppOrders { get; } = new List<AppOrder>();
}
