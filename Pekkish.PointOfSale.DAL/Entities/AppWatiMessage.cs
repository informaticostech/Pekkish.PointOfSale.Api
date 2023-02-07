using System;
using System.Collections.Generic;

namespace Pekkish.PointOfSale.DAL.Entities;

public partial class AppWatiMessage
{
    public int Id { get; set; }

    public string? WatiId { get; set; }

    public string? WaId { get; set; }

    public DateTime Created { get; set; }

    public string? SenderName { get; set; }

    public string? Type { get; set; }

    public string? Text { get; set; }

    public string? ListReplyTitle { get; set; }

    public string? ListReplyDescription { get; set; }

    public string? ListReplyId { get; set; }

    public string? WhatsappMessageId { get; set; }

    public string? ConversationId { get; set; }

    public string? TicketId { get; set; }

    public string? Data { get; set; }

    public string? Timestamp { get; set; }

    public bool Owner { get; set; }

    public string? EventType { get; set; }

    public string? StatusString { get; set; }

    public string? AvatarUrl { get; set; }

    public string? AssignedId { get; set; }

    public string? OperatorName { get; set; }

    public string? OperatorEmail { get; set; }

    public string? MessageContact { get; set; }

    public string? ReplyContextId { get; set; }
}
