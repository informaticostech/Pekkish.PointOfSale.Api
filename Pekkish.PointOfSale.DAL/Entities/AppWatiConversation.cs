using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppWatiConversation
{
    public int Id { get; set; }

    public string ConversationId { get; set; } = null!;

    public string WaId { get; set; } = null!;

    public int? WatiOrderid { get; set; }

    public int? WatiConversationTypeId { get; set; }

    public int WatiConversationStatusId { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime ExpireDate { get; set; }

    public DateTime? CompletedDate { get; set; }

    public virtual ICollection<AppWatiOrder> AppWatiOrders { get; } = new List<AppWatiOrder>();

    public virtual AppWatiConversationStatus WatiConversationStatus { get; set; } = null!;

    public virtual AppWatiConversationType? WatiConversationType { get; set; }
}
