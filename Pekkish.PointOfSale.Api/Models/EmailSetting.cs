using Microsoft.Identity.Client;

namespace Pekkish.PointOfSale.Api
{
    public class EmailSetting
    {
        public string MailServer { get; set; } = "";
        public string FromEmail { get; set; } = "";
        public string FromName { get; set; } = "";
        public string ToEmail { get; set; } = "";
        public int MailPort { get; set; }
        public string SpecifiedPickupDirectory { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
