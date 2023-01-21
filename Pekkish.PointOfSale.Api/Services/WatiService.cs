using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Pekkish.PointOfSale.Api.Models.Wati;
using Pekkish.PointOfSale.DAL.Entities;
using Pekkish.PointOfSale.Wati;
using Pekkish.PointOfSale.Wati.Models.Dtos;
using System;
using static Pekkish.PointOfSale.Api.Models.Wati.Constants;

namespace Pekkish.PointOfSale.Api.Services
{
    public interface IWatiService
    {
        Task MessageReceive(SessionMessageReceiveDto message);        
    }

    public class WatiService : IWatiService
    {                
        private readonly WatiWrapper _wati;
        private readonly WatiConfig _watiConfig;
        private readonly PointOfSaleContext _context;
        private readonly IPointOfSaleService _pointOfSaleService;
        public WatiService(PointOfSaleContext context, IPointOfSaleService pointOfSaleService, IOptions<WatiConfig> watiConfig)
        {            
            _watiConfig = watiConfig.Value;
            _context = context;
            _wati = new WatiWrapper(_watiConfig.BaseUri, _watiConfig.Token);
            
            _pointOfSaleService = pointOfSaleService;
        }
        
        public async Task MessageReceive(SessionMessageReceiveDto message)
        {
            string messageReply = "";

            if (message.ListReply != null)
                messageReply = message.ListReply.Title;

            //Save Message
            var savedMessage = await MessageReceiveSave(message);

            //Only do automated when there is not a current operator assigned
            if (message.AssignedId != "null" || message.AssignedId == null)
            {
                //Check Conversation Exists
                var conversationActiveList = await ConversationListAcive(message.WaId);

                if (conversationActiveList.Count == 0)
                {
                    //Create Conversation
                    var convo = await ConversationCreate(message);

                    //Set Conversation Status
                    await ConversationStatusSet(convo.Id, WatiConversationStatusEnum.Initialised);

                    //Send Welcome
                    await MessageWelcome(message.WaId);
                }
                else
                {
                    //For Now - Focus on Food. More Work Needed for multiple concurrent conversations (New Vendor, Speak to Human)
                    var convo = conversationActiveList[0];

                    //Set Conversation Type Set
                    if (convo.WatiConversationTypeId == null)
                    {
                        switch (messageReply)
                        {
                            case REPLY_FOOD_ORDER:
                                //Convo Type Set Food Order
                                await ConversationTypeSet(convo.Id, WatiConversationTypeEnum.FoodOrder);

                                //Create Food Order Record

                                //Food Status Set Receive Vendor Selection

                                break;

                            case REPLY_CHAT_TO_HUMAN:
                                //Convo Type Set Chat to Human
                                await ConversationTypeSet(convo.Id, WatiConversationTypeEnum.ChatToHuman);

                                //Send Vendor Website
                                await MessageChatToHumanRespone(message.WaId);

                                //Assign to Customer Service                                
                                await SessionAssignCustomerService(message.WaId);

                                //Convo Status Set Completed
                                await ConversationComplete(convo.Id);

                                break;

                            case REPLY_BECOME_A_VENDOR:
                                //Convo Type Set Become a Vendor
                                await ConversationTypeSet(convo.Id, WatiConversationTypeEnum.BecomeVendor);

                                //Send Vendor Website
                                await MessageWebsiteVendor(message.WaId);
                                
                                //Convo Assign to Customer Service
                                await SessionAssignCustomerService(message.WaId);

                                //Convo Status Set Completed
                                await ConversationComplete(convo.Id);

                                break;
                        }
                    }

                    switch (convo.WatiConversationTypeId)
                    {
                        case (int)WatiConversationTypeEnum.FoodOrder:

                            //Get Food Order Id (From Convo Record)


                            //Evaluate Food Order Status

                            //here

                            
                            switch (messageReply)
                            {
                                    
                            }                            
                            break;
                    }
                }
            }
        }

        #region Database Reference
        private async Task<AppWatiMessage> MessageReceiveSave(SessionMessageReceiveDto message)
        {
            return await Task.Run(() =>
            {
                AppWatiMessage watiMessage = new AppWatiMessage();
                watiMessage.WatiId = message.Id;
                watiMessage.Created = message.Created;
                watiMessage.WhatsappMessageId = message.WhatsappMessageId;
                watiMessage.ConversationId = message.ConversationId;
                watiMessage.TicketId = message.TicketId;
                watiMessage.Text = message.Text;
                watiMessage.Type = message.Type;
                watiMessage.Data = message.Data;
                watiMessage.Timestamp = message.Timestamp;
                watiMessage.Owner = message.Owner;
                watiMessage.EventType = message.EventType;
                watiMessage.StatusString = message.StatusString;
                watiMessage.AvatarUrl = message.AvatarUrl;
                watiMessage.AssignedId = message.AssignedId;
                watiMessage.OperatorName = message.OperatorName;
                watiMessage.OperatorEmail = message.OperatorEmail;
                watiMessage.WaId = message.WaId;
                watiMessage.SenderName = message.SenderName;

                if (message.ListReply != null)
                {
                    watiMessage.ListReplyTitle = message.ListReply.Title;
                    watiMessage.ListReplyDescription = message.ListReply.Description;
                    watiMessage.ListReplyId = message.ListReply.Id;
                }

                watiMessage.ReplyContextId = message.ReplyContextId;

                _context.AppWatiMessages.Add(watiMessage);
                _context.SaveChanges();
                
                return watiMessage;
            });
        }       
        private async Task<List<AppWatiConversation>> ConversationListAcive(string whatsAppNumber)
        {
            return await Task.Run(() => 
            {
                var result = (from list in _context.AppWatiConversations
                              where list.ExpireDate > DateTime.Now
                              where list.WatiConversationStatusId != (int)WatiConversationStatusEnum.Completed
                              select list).ToList();

                return result;                
            });
        }
        private async Task<AppWatiConversation> ConversationCreate(SessionMessageReceiveDto message)
        {
            return await Task.Run(() =>
            {
                AppWatiConversation convo = new AppWatiConversation();
                convo.WaId = message.WaId;
                convo.ConversationId = message.ConversationId;
                convo.WatiConversationStatusId = (int)WatiConversationStatusEnum.Initialised;
                convo.WatiConversationTypeId = null;
                convo.CreatedDate = DateTime.Now;
                convo.ExpireDate = DateTime.Now.AddHours(24);

                _context.AppWatiConversations.Add(convo);
                _context.SaveChanges();

                return convo;
            });        
        }
        private async Task ConversationTypeSet(int id, WatiConversationTypeEnum convoType)
        {
            await Task.Run(() =>
            {
                var convo = _context.AppWatiConversations.Single(x => x.Id == id);
                convo.WatiConversationTypeId = (int)convoType;

                _context.SaveChanges();
            });
        }
        private async Task ConversationStatusSet(int id, WatiConversationStatusEnum status)
        {
            await Task.Run(() =>
            {
                var convo = _context.AppWatiConversations.Single(x=>x.Id == id);
                convo.WatiConversationStatusId = (int)status;

                _context.SaveChanges();                
            });
        }
        private async Task ConversationComplete(int id)
        {
            await Task.Run(() =>
            {
                var convo = _context.AppWatiConversations.Single(x => x.Id == id);
                convo.WatiConversationStatusId = (int)WatiConversationStatusEnum.Completed;
                convo.CompletedDate = DateTime.Now;
                _context.SaveChanges();
            });            
        }

        #endregion

        #region WhatsApp Message Send
        private async Task MessageWelcome(string whatsappNumber)
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

            var result = await _wati.InteractiveListMessageSend(whatsappNumber, welcome);
        }
        private async Task MessageFoodOrder(string whatsappNumber)
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

            var result = await _wati.InteractiveListMessageSend(whatsappNumber, welcome);
        }
        private async Task MessageWebsiteVendor(string whatsappNumber)
        {
            await _wati.SessionMessageSend(whatsappNumber, "For more detailed information, please visit our website - https://business.pekkish.co.za/");

            await _wati.SessionMessageSend(whatsappNumber, "Please let us know if you have any further questions afterwards.");
        }
        private async Task MessageChatToHumanRespone(string whatsappNumber)
        {            
            await _wati.SessionMessageSend(whatsappNumber, "A member of our team has been assigned to this chat. How can we assit you?");

            //Todo: Email Customer Service.
        }
        private async Task MessageButtonMesageTest(string whatsappNumber)
        {
            InteractiveButtonsMessageDto message = new InteractiveButtonsMessageDto();

            var header = new InteractiveButtonMessageHeader();
            header.Type = "Text";
            header.Text = "Header Text";
            //header.Media = new InteractiveButtonHeaderMedia { };

            message.Header = header;
            message.Body = "Mesage Body";
            message.Footer = "Message Footer";

            var buttonList = new List<InteractiveButtonMessageButton>();
            buttonList.Add(new InteractiveButtonMessageButton
            {
                 Text = whatsappNumber
            });

            

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

            var result = await _wati.InteractiveListMessageSend(whatsappNumber, welcome);
        }
        #endregion

        private async Task SessionAssignCustomerService(string whatsappNumber)
        {
            await _wati.SessionAssignOperator(whatsappNumber, _watiConfig.DefaultOperatorEmail);
        }
    }
}
