namespace Pekkish.PointOfSale.Api.Models.Wati
{
    public class WatiConfig
    {
        public string BaseUri { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;             
        public string DefaultOperatorEmail { get; set; } = string.Empty;
        public string DefaultWhatsappNumber { get; set; } = string.Empty;
        public string TemplateOrderAccepted { get; set; } = string.Empty;
        public string TemplateOrderRejected { get; set; } = string.Empty;
        public string TemplateOrderReadyPickup { get; set; } = string.Empty;
        public string TemplateOrderReadyDelivery { get; set; } = string.Empty;
    }
}
