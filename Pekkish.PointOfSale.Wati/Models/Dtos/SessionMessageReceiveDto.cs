using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pekkish.PointOfSale.Wati.Models.Dtos
{
    public class SessionMessageReceiveDto
    {
        public string Id { get; set; }
        public DateTime? Created { get; set; }
        public string WhatsappMessageId { get; set; }
        public string ConversationId { get; set; }
        public string TicketId { get; set; }
        public string? Text { get; set; }
        public string Type { get; set; }
        public string? Data { get; set; }
        public string Timestamp { get; set; }
        public bool Owner { get; set; }
        public string EventType { get; set; }
        public string StatusString { get; set; }
        public string? AvatarUrl { get; set; }
        public string? AssignedId { get; set; }
        public string? OperatorName { get; set; }
        public string? OperatorEmail { get; set; }
        public string WaId { get; set; }
        public string? MessageContact { get; set; }
        public string SenderName { get; set; }
        public SessionMessageReceiveListReplyDto? ListReply { get; set; }
        public string ReplyContextId { get; set; }

        public SessionMessageReceiveDto()
        {
            Id = "";
            Created = new DateTime();
            WhatsappMessageId = "";
            ConversationId = "";
            TicketId = "";
            Text = "";
            Type = "";
            Data = "";
            Timestamp = "";
            Owner = false;
            EventType = "";
            StatusString = "";
            AvatarUrl = "";
            AssignedId = "";
            OperatorName = "";
            OperatorEmail = "";
            WaId = "";
            MessageContact = "";
            SenderName = "";
            ListReply = new SessionMessageReceiveListReplyDto();
            ReplyContextId = "";
        }
    }

    public class SessionMessageReceiveListReplyDto
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Id { get; set; } = "";
    }
}
