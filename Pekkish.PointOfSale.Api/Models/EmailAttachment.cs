namespace Pekkish.PointOfSale.Api.Models
{
    public class EmailAttachment
    {
        public string AttachmentName { get; set; } = "";
        public string AttachmentType { get; set; } = "";
        public byte[] Attachment { get; set; } = new byte[0];
    }
}
