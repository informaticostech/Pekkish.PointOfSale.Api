using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Pekkish.PointOfSale.Api.Models.Wati;
using Pekkish.PointOfSale.Wati;
using Pekkish.PointOfSale.Wati.Models.Dtos;

namespace Pekkish.PointOfSale.Api.Services
{
    public interface IWatiService
    {
        Task MessageReceive(SessionMessageReceiveDto message);        
    }

    public class WatiService : IWatiService
    {
        public const string MESSAGE_TYPE_INTERACTIVE = "interactive";
        public const string MESSAGE_TYPE_TEXT = "text";
        public const string REPLY_FOOD_ORDER = "Order Food";

        private readonly WatiWrapper _wati;
        private readonly WatiConfig _watiConfig;        
        private readonly IPointOfSaleService _pointOfSaleService;
        public WatiService(IPointOfSaleService pointOfSaleService, IOptions<WatiConfig> watiConfig)
        {            
            _watiConfig = watiConfig.Value;            
            _wati = new WatiWrapper(_watiConfig.BaseUri, _watiConfig.Token);
            
            _pointOfSaleService = pointOfSaleService;
        }
        
        public async Task MessageReceive(SessionMessageReceiveDto message)
        {
            //Save Message
            await MessageReceiveSave(message);

            //Send Welcome
            switch (message.Type)
            {
                case MESSAGE_TYPE_TEXT:
                    await MessageWelcome(message.WaId);
                    break;

                case MESSAGE_TYPE_INTERACTIVE:                    
                    switch (message.ListReply.Title)
                    {
                        case REPLY_FOOD_ORDER:
                            //Create Food Order Record

                            //Message Init Food Order
                            await MessageFoodOrder(message.WaId);
                            break;
                    }
                    
                    break;
            }

            

        }

        private async Task MessageReceiveSave(SessionMessageReceiveDto message)
        {
            await Task.Run(() =>
            {
                // Save to Database

            });
        }
        private async Task MessageWelcome(string whatsAppNumber)
        {
            InteractiveListMessageDto welcome = new InteractiveListMessageDto();
            List<InteractiveListMessageSection> sectionList = new List<InteractiveListMessageSection>();
            List<InteractivelistMessageSectionRow> rowList = new List<InteractivelistMessageSectionRow>();

            welcome.Header = "Welcome to Pekkish";
            welcome.Body = "How can we help you today?";
            welcome.Footer = "Pekkish BOT (Test)";
            welcome.ButtonText = "Choose Option";

            rowList.Add(new InteractivelistMessageSectionRow
            {
                Title = "Order Food",
                Description = ""
            });

            rowList.Add(new InteractivelistMessageSectionRow
            {
                Title = "Chat to a human",
                Description = "Chat to a member our team"
            });

            rowList.Add(new InteractivelistMessageSectionRow
            {
                Title = "Become a food vendor",
                Description = "Start your own Online Store / online Point of Sale"
            });

            sectionList.Add(new InteractiveListMessageSection
            {
                Title = "Selection",
                Rows = rowList
            });

            welcome.Sections = sectionList;

            var result = await _wati.InteractiveListMessageSend(whatsAppNumber, welcome);
        }
        private async Task MessageFoodOrder(string whatsAppNumber)
        {
            var welcome = new InteractiveListMessageDto();
            var sectionList = new List<InteractiveListMessageSection>();
            var rowList = new List<InteractivelistMessageSectionRow>();
            var vendorList = await _pointOfSaleService.VendorList();

            welcome.Header = "Order Food";
            welcome.Body = "Please select from the following vendors:";
            welcome.Footer = "";
            welcome.ButtonText = "Choose Vendor";

            foreach(var vendor in vendorList)
            {
                rowList.Add(new InteractivelistMessageSectionRow
                {
                    Title = vendor.Name,
                    Description = ""
                });
            }
                        
            sectionList.Add(new InteractiveListMessageSection
            {
                Title = "Participating Vendors",
                Rows = rowList
            });

            welcome.Sections = sectionList;

            var result = await _wati.InteractiveListMessageSend(whatsAppNumber, welcome);
        }

    }
}
