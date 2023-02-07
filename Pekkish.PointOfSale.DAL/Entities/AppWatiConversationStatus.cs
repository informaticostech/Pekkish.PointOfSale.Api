using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppWatiConversationStatus
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<AppWatiConversation> AppWatiConversations { get; } = new List<AppWatiConversation>();
}
