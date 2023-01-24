using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Pekkish.PointOfSale.Api.Models.Wati;
using Pekkish.PointOfSale.DAL.Entities;
using Pekkish.PointOfSale.Wati;
using Pekkish.PointOfSale.Wati.Models.Dtos;
using System;
using System.Numerics;
using System.Reflection.PortableExecutable;
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
            else
                messageReply = message.Text;

            //Save Message
            var savedMessage = await MessageReceiveSave(message);

            //Only do automated when there is not a current operator assigned
            if (message.AssignedId == null)
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

                                //Create Food Order Record (Init)
                                await FoodOrderCreate(message, convo);

                                //Send Vendor List
                                await MessageFoodOrderVendorSelection(convo.WaId);
                                break;

                            case REPLY_CHAT_TO_HUMAN:
                                #region Chat To Human
                                //Convo Type Set Chat to Human
                                await ConversationTypeSet(convo.Id, WatiConversationTypeEnum.ChatToHuman);

                                //Send Vendor Website
                                await MessageChatToHumanRespone(message.WaId);

                                //Assign to Customer Service                                
                                await SessionAssignCustomerService(message.WaId);

                                //Convo Status Set Completed
                                await ConversationComplete(convo.Id);
                                #endregion
                                break;

                            case REPLY_BECOME_A_VENDOR:
                                #region Become a vendor
                                //Convo Type Set Become a Vendor
                                await ConversationTypeSet(convo.Id, WatiConversationTypeEnum.BecomeVendor);

                                //Send Vendor Website
                                await MessageWebsiteVendor(message.WaId);
                                
                                //Convo Assign to Customer Service
                                await SessionAssignCustomerService(message.WaId);

                                //Convo Status Set Completed
                                await ConversationComplete(convo.Id);
                                #endregion
                                break;
                        }
                    }

                    switch (convo.WatiConversationTypeId)
                    {
                        case (int)WatiConversationTypeEnum.FoodOrder:
                            //Run Order Through
                            await FoodOrderProcess(message, convo, messageReply);                                                                                    
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
                              where list.WaId == whatsAppNumber
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
        private async Task FoodOrderCreate(SessionMessageReceiveDto message, AppWatiConversation convo)
        {
            await Task.Run(() =>
            {
                //Create Order
                AppWatiOrder order = new AppWatiOrder();
                order.Name = message.SenderName;
                order.WatiConversationId = convo.Id;
                order.WatiOrderStatusId = (int)WatiFoodOrderStatusEnum.VendorSelection;
                order.SubTotal = 0;
                order.Total = 0;
                order.CreatedDate = DateTime.Now;

                _context.AppWatiOrders.Add(order);
                _context.SaveChanges();

                //Update Conversation with OrderId
                AppWatiConversation appWatiConversation = _context.AppWatiConversations.Single(x => x.Id == convo.Id);
                appWatiConversation.WatiOrderid = order.Id;
                _context.SaveChanges();

            });
        }
        private async Task FoodOrderStatusSet(int id, WatiFoodOrderStatusEnum status)
        {
            await Task.Run(() =>
            {
                AppWatiOrder order = _context.AppWatiOrders.Single(x => x.Id == id);
                order.WatiOrderStatusId = (int)status;

                _context.SaveChanges();
            });
        }
        private async Task FoodOrderProcess(SessionMessageReceiveDto message, AppWatiConversation convo, string messageReply)
        {
            if (convo.WatiOrderid != null)
            {
                var order = _context.AppWatiOrders.Single(x => x.Id == (int)convo.WatiOrderid);
                                
                switch (order.WatiOrderStatusId)
                {
                    case (int)WatiFoodOrderStatusEnum.VendorSelection:
                        #region Vendor Selection
                        var vendor = (await _pointOfSaleService.VendorList()).Single(x=>x.Name== messageReply);
                        var tenantId = (Guid)vendor.TenantId;
                        var locationList = await _pointOfSaleService.LocationList(tenantId);
                        var brandList = await _pointOfSaleService.BrandList(tenantId);

                        order.TenantId = vendor.TenantId;
                        order.TenantInfoId = vendor.Id;

                        if (locationList.Count == 1)
                        {
                            order.LocationId = locationList[0].Id;
                        }
                        else
                        { 
                            //TODO: Location Selection
                        }

                        if (brandList.Count == 1)
                        {
                            //Set Brand
                            order.CurrentBrand = brandList[0].Id;

                            //Set Status to Category Selection
                            await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.CategorySelection);

                            //Send Category List
                            await MessageFoodOrderProductCategorySelectionList(convo.WaId, brandList[0]);
                        }
                        else
                        {
                            //Set Status to Brand Selection
                            await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.BrandSelection);

                            //Send Brand List
                            await MessageFoodOrderBrandSelection(convo.WaId, tenantId, vendor);
                        }

                        _context.SaveChanges();

                        #endregion
                        break;

                    case (int)WatiFoodOrderStatusEnum.BrandSelection:
                        #region Brand Selection
                        var brand = (await _pointOfSaleService.BrandList((Guid)order.TenantId)).Single(x => x.Name == messageReply);

                        await ProductCategorySelctionFunction(convo, order, brand);
                        #endregion
                        break;

                    case (int)WatiFoodOrderStatusEnum.CategorySelection:
                        var category = (await _pointOfSaleService.ProductCategoryList((int)order.CurrentBrand)).Single(x => x.Name == messageReply);

                        await ProductSelctionFunction(convo, order, category);
                        break;

                    case (int)WatiFoodOrderStatusEnum.ProductSelection:
                        #region Product Selection

                        if (messageReply == REPLY_BACK_CATEGORY)
                        {
                            var brandFromProductSelection = await _pointOfSaleService.BrandItemGet((int)order.CurrentBrand);
                            await ProductCategorySelctionFunction(convo, order, brandFromProductSelection);
                        }
                        else
                        {
                            var product = (await _pointOfSaleService.ProductList((int)order.CurrentCategory)).Single(x => x.Name == messageReply);

                            order.CurrentProduct = product.Id;

                            //Set Status to Brand Selection
                            await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.ProductAddToCardConfirm);

                            //Send Product Message
                            await MessageFoodOrderProductConfirmation(convo.WaId, product);

                            _context.SaveChanges();
                        }
                        
                        #endregion
                        break;

                    case (int)WatiFoodOrderStatusEnum.ProductSelectionText:
                        #region Product Selection Text
                        int productSelection = Convert.ToInt32(messageReply);

                        if (productSelection == 0)
                        {
                            var brandFromProductSelection = await _pointOfSaleService.BrandItemGet((int)order.CurrentBrand);
                            await ProductCategorySelctionFunction(convo, order, brandFromProductSelection);
                        }
                        else
                        {
                            var productFromText = (await _pointOfSaleService.ProductList((int)order.CurrentCategory))[productSelection - 1];

                            //Set Status to Brand Selection
                            await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.ProductAddToCardConfirm);

                            //Send Product Message
                            await MessageFoodOrderProductConfirmation(convo.WaId, productFromText);

                            _context.SaveChanges();
                        }
                        
                        #endregion
                        break;

                    case (int)WatiFoodOrderStatusEnum.ProductAddToCardConfirm:
                        var productFromAddToCart = await _pointOfSaleService.ProductItemGet((int)order.CurrentProduct);
                        var categoryFromAddToCart = await _pointOfSaleService.ProductCategoryItemGet((int)order.CurrentCategory);
                            
                        switch (messageReply)
                        {
                            case REPLY_ADD_TO_CART:

                                break;

                            case REPLY_CANCEL:
                                //Send Product Selection
                                await ProductSelctionFunction(convo, order, categoryFromAddToCart);
                                break;
                        }

                        break;
                }

            }
            else
            {
                //TODO: Handle error
            }
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
        private async Task MessageFoodOrderVendorSelection(string whatsappNumber)
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
        private async Task MessageFoodOrderBrandSelection(string whatsappNumber, Guid tenantId, AppTenantInfo tenant)
        {
            var message = new InteractiveListMessageDto();
            var sectionList = new List<InteractiveListMessageSection>();
            var rowList = new List<InteractivelistMessageSectionRow>();
            var brandList = await _pointOfSaleService.BrandList(tenantId);

            message.Header = tenant.NameShort;
            message.Body = "Please select from the following brands:";
            message.Footer = "";
            message.ButtonText = "Choose Brand";

            foreach (var item in brandList)
            {
                rowList.Add(new InteractivelistMessageSectionRow
                {
                    Title = item.Name,
                    Description = ""
                });
            }

            sectionList.Add(new InteractiveListMessageSection
            {
                Title = $"Selection",
                Rows = rowList
            });

            message.Sections = sectionList;

            var result = await _wati.InteractiveListMessageSend(whatsappNumber, message);
        }
        private async Task MessageFoodOrderProductCategorySelectionList(string whatsappNumber, AppBrand brand)
        {
            var message = new InteractiveListMessageDto();
            var sectionList = new List<InteractiveListMessageSection>();
            var rowList = new List<InteractivelistMessageSectionRow>();
            var categoryList = await _pointOfSaleService.ProductCategoryList(brand.Id);

            message.Header = brand.NameShort;
            message.Body = "Please select from the following categories:";
            message.Footer = "";
            message.ButtonText = "Choose Category";

            foreach (var item in categoryList)
            {
                rowList.Add(new InteractivelistMessageSectionRow
                {
                    Title = item.Name,          // (item.ExternalAppName.IsNullOrEmpty()) ? item.Name : item.ExternalAppName,
                    Description = ""
                });
            }

            sectionList.Add(new InteractiveListMessageSection
            {
                Title = $"Selection",
                Rows = rowList
            });

            message.Sections = sectionList;

            var result = await _wati.InteractiveListMessageSend(whatsappNumber, message);
        }
        private async Task MessageFoodOrderProductCategorySelectionText(string whatsappNumber, AppBrand brand)
        {
            string message;
            var categoryList = await _pointOfSaleService.ProductCategoryList(brand.Id);

            message = brand.Name.Replace('&', '_').ToUpper();
            message += "\r\n";
            message += "\r\n";
            message += "Please select from the following categories:";
            message += "\r\n";

            for (int c = 0; c < categoryList.Count; c++)
            {
                message += $"{c + 1} {categoryList[c].Name}";
                message += "\r\n";
            }

            var result = await _wati.SessionMessageSend(whatsappNumber, message);
        }
        private async Task MessageFoodOrderProductSelectionList(string whatsappNumber, AppProductCategory category)
        {
            var message = new InteractiveListMessageDto();
            var sectionList = new List<InteractiveListMessageSection>();
            var rowCategoryList = new List<InteractivelistMessageSectionRow>();
            var rowProductList = new List<InteractivelistMessageSectionRow>();
            var productList = await _pointOfSaleService.ProductList(category.Id);

            message.Header = category.Name;
            message.Body = "Please select from the following Products:";
            message.Footer = "";
            message.ButtonText = "Choose Product";

            //Back to Categories
            rowCategoryList.Add( new InteractivelistMessageSectionRow 
            {
                Title = REPLY_BACK_CATEGORY,
                Description = ""
            });

            sectionList.Add(new InteractiveListMessageSection
            {
                Title = $"Categories",
                Rows = rowCategoryList
            });

            //Products
            foreach (var item in productList)
            {
                rowProductList.Add(new InteractivelistMessageSectionRow
                {
                    Title = item.Name, 
                    Description = $"R{item.Price}"
                });
            }

            //Complile Lists
            sectionList.Add(new InteractiveListMessageSection
            {
                Title = $"Products",
                Rows = rowProductList
            });

            message.Sections = sectionList;

            var result = await _wati.InteractiveListMessageSend(whatsappNumber, message);
        }
        private async Task MessageFoodOrderProductSelectionText(string whatsappNumber, AppProductCategory category)
        {
            string message = "";
            var productList = await _pointOfSaleService.ProductList(category.Id);

            message = category.Name.Replace('&', '_').ToUpper();
            message += "\r\n";
            message += "\r\n";
            message += "Categories:";
            message += "\r\n";
            message += $"0 {REPLY_BACK_CATEGORY}";
            message += "\r\n";
            message += "\r\n";
            message += "Products:";
            message += "\r\n";

            for (int c = 0; c < productList.Count; c++) 
            {
                message += $"{c + 1} {productList[c].Name} - R{productList[c].Price}";
                message += "\r\n";
            }

            var result = await _wati.SessionMessageSend(whatsappNumber, message);
        }
        private async Task MessageFoodOrderProductConfirmation(string whatsappNumber, AppProduct product)
        {
            var messageMedia = new InteractiveButtonsMessageMediaDto();
            var messageText = new InteractiveButtonsMessageTextDto();
            var headerMedia = new InteractiveButtonMessageHeaderMedia();
            var headerText = new InteractiveButtonMessageHeaderText();
            var media = new InteractiveButtonHeaderMedia();
            var buttonList = new List<InteractiveButtonMessageButton>();

            #region Buttons
            buttonList.Add(new InteractiveButtonMessageButton()
            {
                Text = REPLY_ADD_TO_CART
            });

            buttonList.Add(new InteractiveButtonMessageButton()
            {
                Text = REPLY_CANCEL
            });
            #endregion

            #region Determine if image is available
            var isImage = false;
            if(product.PekkishProductId != null)
            {
                var pekkishProduct = _context.PekkishProducts.SingleOrDefault(x => x.Id == product.PekkishProductId);

                if (pekkishProduct != null)
                {
                    if(pekkishProduct.ImageUrl!= null)
                    {
                        media.Url = pekkishProduct.ImageUrl;
                        isImage = true;
                    }                    
                }                
            }
            #endregion

            if (isImage)
            {
                headerMedia.Type = "Image";
                headerMedia.Media = media;
                messageMedia.Header = headerMedia;

                messageMedia.Body += $"*{product.Name}*";
                messageMedia.Body += "\r\n";

                if (!product.Description.IsNullOrEmpty())
                {
                    messageMedia.Body += "\r\n";
                    messageMedia.Body += product.Description;
                    messageMedia.Body += "\r\n";
                    messageMedia.Body += "\r\n";                    
                }

                messageMedia.Body += $"*R {product.Price}*";

                messageMedia.Footer = "";

                messageMedia.Buttons = buttonList;

                var result = await _wati.InteractiveButtonsMessageMediaSend(whatsappNumber, messageMedia);
            }
            else
            {
                headerText.Type = "Text";
                headerText.Text = product.Name;
                messageText.Header = headerText;                

                if (!product.Description.IsNullOrEmpty())
                {
                    messageText.Body += "\r\n";
                    messageText.Body += product.Description;
                    messageText.Body += "\r\n";
                    messageText.Body += "\r\n";
                }

                messageText.Body += $"*R {product.Price}*";

                messageText.Footer = "";

                messageText.Buttons = buttonList;

                var result = await _wati.InteractiveButtonsMessageTextSend(whatsappNumber, messageText);
            }            
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
        #endregion

        private async Task SessionAssignCustomerService(string whatsappNumber)
        {
            await _wati.SessionAssignOperator(whatsappNumber, _watiConfig.DefaultOperatorEmail);
        }

        private async Task ProductCategorySelctionFunction(AppWatiConversation convo, AppWatiOrder order, AppBrand brand)
        {            
            var categoryList = await _pointOfSaleService.ProductCategoryList(brand.Id);

            order.CurrentBrand = brand.Id;

            if (categoryList.Count > 10)
            {
                //Set Status to Brand Selection
                await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.CategorySelectionText);

                //Send Category List
                await MessageFoodOrderProductCategorySelectionText(convo.WaId, brand);
            }
            else
            {
                //Set Status to Brand Selection
                await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.CategorySelection);

                //Send Category List
                await MessageFoodOrderProductCategorySelectionList(convo.WaId, brand);
            }

            _context.SaveChanges();
        }
        private async Task ProductSelctionFunction(AppWatiConversation convo, AppWatiOrder order, AppProductCategory category)
        {            
            var productList = await _pointOfSaleService.ProductList(category.Id);
            var messageText = new InteractiveButtonsMessageTextDto();
            var headerText = new InteractiveButtonMessageHeaderText();
            var buttonList = new List<InteractiveButtonMessageButton>();

            order.CurrentCategory = category.Id;
            
            //Send Category List
            if (productList.Count > 9)
            {
                //Set Status to CAtegory Selection Text
                await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.ProductSelectionText);

                //Send Product Message Text
                await MessageFoodOrderProductSelectionText(convo.WaId, category);
            }
            else
            {
                //Set Status to Product Selection
                await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.ProductSelection);

                //Send Product Message
                await MessageFoodOrderProductSelectionList(convo.WaId, category);
            }

            _context.SaveChanges();
        }
    }
}
