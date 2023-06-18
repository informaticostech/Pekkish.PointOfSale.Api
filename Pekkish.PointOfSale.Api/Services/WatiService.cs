using MailKit.Search;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Nancy.Routing.Trie.Nodes;
using Pekkish.PointOfSale.Api.Models.Wati;
using Pekkish.PointOfSale.DAL.Context;
using Pekkish.PointOfSale.DAL.Entities;
using Pekkish.PointOfSale.Wati;
using Pekkish.PointOfSale.Wati.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using static Pekkish.PointOfSale.Api.Models.Wati.Constants;
using static System.Net.Mime.MediaTypeNames;

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
            var conversationActiveList = new List<AppWatiConversation>();
            var convo = new AppWatiConversation();
            var order = new AppWatiOrder();
            var tenant = new AppTenantInfo();
            Guid tenantId = new Guid();

            if (message.ListReply != null)
                messageReply = message.ListReply.Title;
            else
                messageReply = message.Text.ToString();

            //await MessageTemplateMessage(message.WaId);

            //Wati Duplicate Check
            var duplicateCheck = _context.AppWatiMessages.Where(x => x.WatiId == message.Id).ToList();

            if (duplicateCheck.Count == 0)
            {
                //Save Message
                var savedMessage = await MessageReceiveSave(message);

                //Restrict Access                
                var isOrderFood = false;
                var watiUser = _context.AppWatiUsers.SingleOrDefault(x => x.WaId == message.WaId);

                if (watiUser != null)
                    isOrderFood = true;

                //Only do automated when there is not a current operator assigned
                if (message.AssignedId == null)
                {
                    //Check for food order direct to store
                    if ((messageReply.Length > 6 && messageReply.Substring(0, 5).ToUpper() == "ORDER") && isOrderFood)
                    {
                        #region cancel existing convos
                        conversationActiveList = await ConversationListAcive(message.WaId);

                        foreach (var convoItem in conversationActiveList)
                        {
                            //Cancell any existing conversations
                            await ConversationCancel(convoItem.Id);
                        }
                        #endregion

                        var tenantSelectedString = messageReply.Substring(6, messageReply.Length - 6);
                        tenant = _context.AppTenantInfos.SingleOrDefault(x => x.NameShort == tenantSelectedString);

                        //Create Conversation
                        convo = await ConversationCreate(message);

                        //Set Conversation Status
                        await ConversationStatusSet(convo.Id, WatiConversationStatusEnum.Initialised);

                        //Convo Type Set Food Order
                        await ConversationTypeSet(convo.Id, WatiConversationTypeEnum.FoodOrder);

                        //Create Food Order Record (Init)
                        order = await FoodOrderCreate(message, convo);

                        if (tenant == null)
                        {
                            //Send Vendor List
                            await MessageFoodOrderVendorSelection(convo.WaId);
                        }
                        else
                        {
                            if (tenant.TenantId != null)
                                tenantId = (Guid)tenant.TenantId;

                            order.TenantId = tenant.TenantId;
                            order.TenantInfoId = tenant.Id;

                            var locationList = await _pointOfSaleService.LocationList(tenantId);

                            if (locationList.Count == 1)
                                order.LocationId = locationList[0].Id;
                            else
                            {
                                //TODO: Location Selection
                            }
                            _context.SaveChanges();


                            //Set Status Vendor Landing
                            await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.VendorLanding);

                            //Message welcome message
                            await MessageFoodOrderVendorWelcome(convo.WaId, tenantId, tenant);
                        }

                        return;
                    }

                    //Check Conversation Exists
                    conversationActiveList = await ConversationListAcive(message.WaId);

                    if (conversationActiveList.Count == 0)
                    {
                        //Create Conversation
                        convo = await ConversationCreate(message);

                        //Set Conversation Status
                        await ConversationStatusSet(convo.Id, WatiConversationStatusEnum.Initialised);

                        //Send Welcome
                        await MessageWelcome(message.WaId, isOrderFood);
                    }
                    else
                    {
                        //For Now - Focus on Food. More Work Needed for multiple concurrent conversations (New Vendor, Speak to Human)
                        convo = conversationActiveList[0];

                        //Restart
                        if (messageReply == REPLY_RESTART)
                        {
                            await ConversationCancel(convo.Id);

                            //Create Conversation
                            convo = await ConversationCreate(message);

                            //Set Conversation Status
                            await ConversationStatusSet(convo.Id, WatiConversationStatusEnum.Initialised);

                            //Send Welcome
                            await MessageWelcome(message.WaId, isOrderFood);

                            return;
                        }

                        //Help
                        if (messageReply == REPLY_HELP)
                        {
                            //Cancel conversation
                            await ConversationCancel(convo.Id);

                            //Send Chat to Human Response
                            await MessageChatToHumanRespone(message.WaId);

                            //Assign to Customer Service                                
                            await SessionAssignCustomerService(message.WaId);
                        }

                        //Set Conversation Type Set
                        if (convo.WatiConversationTypeId == null)
                        {
                            switch (messageReply)
                            {
                                case REPLY_FOOD_ORDER:
                                    #region Food Order
                                    //Convo Type Set Food Order
                                    await ConversationTypeSet(convo.Id, WatiConversationTypeEnum.FoodOrder);

                                    //Create Food Order Record (Init)
                                    await FoodOrderCreate(message, convo);

                                    //Send Vendor List
                                    await MessageFoodOrderVendorSelection(convo.WaId);
                                    #endregion
                                    return;

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
                                    return;

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
                                    return;

                                default:
                                    #region Default
                                    //Catch error in loop. Clear existing convo.
                                    await ConversationCancel(convo.Id);

                                    //Send training message
                                    await _wati.SessionMessageSend(convo.WaId, "Unfortunately, I am unable to answer your text message. If you would like to speak to a human please select 'Chat to a human' from the list of responses below:");

                                    //Send Welcome
                                    await MessageWelcome(message.WaId, isOrderFood);
                                    #endregion
                                    return;
                            }
                        }

                        switch (convo.WatiConversationTypeId)
                        {
                            case (int)WatiConversationTypeEnum.FoodOrder:
                                await FoodOrderProcess(message, convo, messageReply);
                                break;
                        }
                    }
                }

            }
            else
            {
                var test = message;
            }
        }

        #region Database Reference
        private async Task<AppWatiMessage> MessageReceiveSave(SessionMessageReceiveDto message)
        {
            return await Task.Run(() =>
            {                
                AppWatiMessage watiMessage = new AppWatiMessage();
                watiMessage.WatiId = message.Id;
                watiMessage.Created = DateTime.Now;
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
                              where list.WatiConversationStatusId != (int)WatiConversationStatusEnum.Cancelled
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
        private async Task ConversationCancel(int id)
        {
            await Task.Run(() =>
            {
                var convo = _context.AppWatiConversations.Single(x => x.Id == id);
                convo.WatiConversationStatusId = (int)WatiConversationStatusEnum.Cancelled;
                convo.CompletedDate = DateTime.Now;                
                _context.SaveChanges();
            });
        }
        private async Task<AppWatiOrder> FoodOrderCreate(SessionMessageReceiveDto message, AppWatiConversation convo)
        {
            return await Task.Run(() =>
            {
                //Create Order
                AppWatiOrder order = new AppWatiOrder();
                order.Name = message.SenderName;
                order.WatiConversationId = convo.Id;
                order.WatiOrderStatusId = (int)WatiFoodOrderStatusEnum.VendorSelection;
                order.WaId = convo.WaId;
                order.SubTotal = 0;
                order.Total = 0;
                order.CreatedDate = DateTime.Now;
                order.OrderFulfillmentId = (int)PosFulfillmentTypeEnum.Pickup;  //TODO manage Delivery

                _context.AppWatiOrders.Add(order);
                _context.SaveChanges();

                //Update Conversation with OrderId
                AppWatiConversation appWatiConversation = _context.AppWatiConversations.Single(x => x.Id == convo.Id);
                appWatiConversation.WatiOrderid = order.Id;
                _context.SaveChanges();

                return order;
            });
        }
        private async Task<AppWatiOrder> FoodOrderDetailCreate(int orderId, AppProduct product, int quantity)
        {
            var order = _context.AppWatiOrders.Single(x => x.Id == orderId);
            var orderDetail = new AppWatiOrderDetail();

            orderDetail.WatiOrderId = orderId;
            orderDetail.BrandId = (int)order.CurrentBrand;
            orderDetail.ProductId = product.Id;
            orderDetail.Amount = product.Price;
            orderDetail.Name = product.Name;
            orderDetail.Quantity = quantity;
            orderDetail.Comment = "";
            
            _context.AppWatiOrderDetails.Add(orderDetail);
            _context.SaveChanges();

            order.CurrentOrderDetail = orderDetail.Id;
            order.SubTotal = await FoodOrderTotalsGet(orderId);
            _context.SaveChanges();

            return order;
        }
        private async Task<AppWatiOrderDetail> FoodOrderDetailOptionCreate(int orderDetailId, AppProductExtraOption productExtraOption)
        {
            return await Task.Run(() =>
            {                
                var orderDetail = _context.AppWatiOrderDetails.Single(x => x.Id == orderDetailId);                
                var orderDetailOption = new AppWatiOrderDetailOption();

                orderDetailOption.WatiOrderDetailId = orderDetailId;
                orderDetailOption.Name = productExtraOption.Name;
                orderDetailOption.WatiOrderDetailId = orderDetailId;
                orderDetailOption.Quantity = orderDetail.Quantity;
                orderDetailOption.Price = productExtraOption.Price;
                orderDetailOption.ProductExtraId = (int)productExtraOption.ProductExtraId;
                orderDetailOption.ProductExtraOptionId = (int)productExtraOption.Id;                
                
                _context.AppWatiOrderDetailOptions.Add(orderDetailOption);
                _context.SaveChanges();

                if (productExtraOption.Price != 0)
                {                    
                    orderDetail.Amount += (productExtraOption.Price);
                    _context.SaveChanges();                    
                }

                return orderDetail;
            });
        }
        private async Task<List<AppWatiOrderDetail>> FoodOrderCartGet(int orderId)
        {
            return await Task.Run(() =>
            {
                var result = (from Detail in _context.AppWatiOrderDetails
                              where Detail.WatiOrderId == orderId
                              select Detail).ToList();
                return result;
            });
        }
        private async Task<decimal> FoodOrderTotalsGet(int orderId)
        {            
            return await Task.Run(() =>
            {
                decimal result = 0;

                var productList = (from list in _context.AppWatiOrderDetails
                         where list.WatiOrderId == orderId
                         select list).ToList();

                Parallel.ForEach(productList, product =>
                {
                    result += product.Amount * product.Quantity;
                });

                return result;
            });
        }
        private async Task BrandOrCategorySelectionFunction(AppWatiConversation convo, AppWatiOrder order, Guid tenantId, AppTenantInfo tenant)
        {            
            var brandList = await _pointOfSaleService.BrandList(tenantId);

            order.TenantId = tenantId;
            order.TenantInfoId = tenant.Id;
            
            if (brandList.Count == 1)
            {
                //Set Brand
                order.CurrentBrand = brandList[0].Id;

                //Set Status to Category Selection
                await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.CategorySelection);

                //Send Category List
                await MessageFoodOrderProductCategorySelectionList(convo.WaId, brandList[0], tenantId);
            }
            else
            {
                //Set Status to Brand Selection
                await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.BrandSelection);

                //Send Brand List
                await MessageFoodOrderBrandSelection(convo.WaId, tenantId, tenant);
            }

            _context.SaveChanges();
        }
        private async Task ProductCategorySelectionFunction(AppWatiConversation convo, AppWatiOrder order, AppBrand brand, Guid tenantId)
        {
            var categoryList = await _pointOfSaleService.ProductCategoryList(brand.Id);

            order.CurrentBrand = brand.Id;
            _context.SaveChanges();

            if (categoryList.Count >= 10)
            {
                //Set Status to Brand Selection
                await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.CategorySelectionText);

                //Send Category List
                await MessageFoodOrderProductCategorySelectionText(convo.WaId, brand, tenantId);
            }
            else
            {
                //Set Status to Brand Selection
                await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.CategorySelection);

                //Send Category List
                await MessageFoodOrderProductCategorySelectionList(convo.WaId, brand, tenantId);
            }
        }
        private async Task ProductSelectionFunction(AppWatiConversation convo, AppWatiOrder order, AppProductCategory category)
        {
            var productList = await _pointOfSaleService.ProductList(category.Id);
            var messageText = new InteractiveButtonsMessageTextDto();
            var headerText = new InteractiveButtonMessageHeaderText();
            var buttonList = new List<InteractiveButtonMessageButton>();

            order.CurrentCategory = category.Id;
            _context.SaveChanges();

            //Send Product List
            //Set Status to CAtegory Selection Text
            await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.ProductSelectionText);

            //Send Product Message Text
            await MessageFoodOrderProductSelectionText(convo.WaId, category);

            //if (productList.Count > 9)
            //{
            //    //Set Status to CAtegory Selection Text
            //    await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.ProductSelectionText);

            //    //Send Product Message Text
            //    await MessageFoodOrderProductSelectionText(convo.WaId, category);
            //}
            //else
            //{
            //    //Set Status to Product Selection
            //    await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.ProductSelection);

            //    //Send Product Message
            //    await MessageFoodOrderProductSelectionList(convo.WaId, category);
            //}           
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
                AppTenantInfo tenant = new AppTenantInfo();
                AppBrand brand = new AppBrand();
                AppProductCategory category = new AppProductCategory();
                AppProduct product = new AppProduct();
                AppProductExtra extra = new AppProductExtra();
                List<AppBrand> brandList = new List<AppBrand>();
                List<AppProductExtra> productExtraList = new List<AppProductExtra>();
                var tenantId = new Guid();
                int brandId = 0;
                int categoryId = 0;
                int productId = 0;
                int extraId = 0;
                int orderDetail = 0;
                
                #region Logistics
                if (order.TenantId != null)
                {
                    tenant = _context.AppTenantInfos.Single(x => x.TenantId == order.TenantId);
                    tenantId = (Guid)order.TenantId;

                    brandList = await _pointOfSaleService.BrandList(tenantId);
                }

                if (order.CurrentBrand != null)
                {
                    brandId = (int)order.CurrentBrand;
                    brand = await _pointOfSaleService.BrandItemGet(brandId);
                }

                if(order.CurrentOrderDetail!= null)
                {
                    orderDetail = (int)order.CurrentOrderDetail;
                }

                if (order.CurrentCategory != null)
                {
                    categoryId = (int)order.CurrentCategory;
                    category = await _pointOfSaleService.ProductCategoryItemGet(categoryId);
                }

                if (order.CurrentProduct != null)
                {
                    productId = (int)order.CurrentProduct;
                    product = await _pointOfSaleService.ProductItemGet(productId);
                }

                if (order.CurrentProductExtra != null)
                {
                    extraId = (int)order.CurrentProductExtra;    
                    extra = await _pointOfSaleService.ProductExtraItemGet(extraId);
                }
                #endregion

                switch (order.WatiOrderStatusId)
                {
                    case (int)WatiFoodOrderStatusEnum.VendorSelection:
                        #region Vendor Selection                        
                        var tenantSelected = (await _pointOfSaleService.VendorList()).SingleOrDefault(x => x.Name == messageReply);
                        
                        if (tenantSelected == null)
                        {
                            await MessageResponseUnexpected(convo.WaId);
                        }
                        else
                        {
                            var locationList = await _pointOfSaleService.LocationList((Guid)tenantSelected.TenantId);

                            if (locationList.Count == 1)
                                order.LocationId = locationList[0].Id;
                            else
                            {
                                //TODO: Location Selection
                            }

                            order.TenantId = tenantSelected.TenantId;
                            _context.SaveChanges();

                            if (order.TenantId != null)
                            {
                                //Set Status Vendor Landing
                                await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.VendorLanding);

                                //Message welcome message
                                await MessageFoodOrderVendorWelcome(convo.WaId, (Guid)order.TenantId, tenantSelected);
                            }
                        }
                        #endregion
                        break;

                    case (int)WatiFoodOrderStatusEnum.VendorLanding:
                        #region Vendor Landing                       
                        switch (messageReply)
                        {
                            case REPLY_VENDOR_ORDER_FOOD:
                                //Brand or Category Selection
                                await BrandOrCategorySelectionFunction(convo, order, tenantId, tenant);
                                break;

                            case REPLY_CANCEL:
                                //Set status Vendor Selction
                                await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.VendorSelection);

                                //Send Vendor List
                                await MessageFoodOrderVendorSelection(convo.WaId);
                                break;

                            default:
                                await MessageResponseUnexpected(convo.WaId);
                                return;
                        }
                        #endregion
                        break;

                    case (int)WatiFoodOrderStatusEnum.BrandSelection:
                        #region Brand Selection
                        var brandSelected = (await _pointOfSaleService.BrandList(tenantId)).SingleOrDefault(x => x.Name == messageReply);

                        if (brandSelected == null)
                        {
                            await MessageResponseUnexpected(convo.WaId);
                        }
                        else
                        {
                            await ProductCategorySelectionFunction(convo, order, brandSelected, tenantId);
                        }                        
                        #endregion
                        break;

                    case (int)WatiFoodOrderStatusEnum.CategorySelection:
                        #region Category Selection                                              
                        switch (messageReply)
                        {
                            case REPLY_BACK_BRAND:
                                //Go Back to Brand List
                                //Set Status to Brand Selection
                                await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.BrandSelection);

                                //Send Brand List
                                await MessageFoodOrderBrandSelection(convo.WaId, tenantId, tenant);
                                break;

                            case REPLY_BACK_VENDOR:
                                //Go back to Vendor Landing
                                //Set Status Vendor Landing
                                await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.VendorLanding);

                                //Message welcome message
                                await MessageFoodOrderVendorWelcome(convo.WaId, tenantId, tenant);

                                break;

                            default:
                                var categorySelected = (await _pointOfSaleService.ProductCategoryList(brandId)).SingleOrDefault(x => x.Name == messageReply);

                                if (categorySelected == null)
                                {
                                    await MessageResponseUnexpected(convo.WaId);
                                }
                                else
                                {
                                    await ProductSelectionFunction(convo, order, categorySelected);
                                }
                                break;
                        }
                        #endregion 
                        break;

                    case (int)WatiFoodOrderStatusEnum.CategorySelectionText:
                        #region Product Selection Text                                                
                        int categorySelection;

                        try
                        {
                            categorySelection = Convert.ToInt32(messageReply);
                        }
                        catch
                        {
                            await MessageResponseUnexpected(convo.WaId);
                            return;
                        }
                                                
                        if (categorySelection == 0)
                        {
                            if (brandList.Count == 1)
                            {
                                //Go back to Vendor Landing
                                //Set Status Vendor Landing
                                await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.VendorLanding);

                                //Message welcome message
                                await MessageFoodOrderVendorWelcome(convo.WaId, tenantId, tenant);
                            }
                            else
                            {
                                //Go Back to Brand List
                                //Set Status to Brand Selection
                                await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.BrandSelection);

                                //Send Brand List
                                await MessageFoodOrderBrandSelection(convo.WaId, tenantId, tenant);

                            }                            
                        }
                        else
                        {
                            var categoryFromText = (await _pointOfSaleService.ProductCategoryList(brandId))[categorySelection - 1];

                            await ProductSelectionFunction(convo, order, categoryFromText);                                                        
                        }

                        #endregion

                        break;

                    case (int)WatiFoodOrderStatusEnum.ProductSelection:
                        #region Product Selection

                        if (messageReply == REPLY_BACK_CATEGORY)
                        {                            
                            await ProductCategorySelectionFunction(convo, order, brand, tenantId);
                        }
                        else
                        {
                            var productSelected = (await _pointOfSaleService.ProductList(categoryId)).SingleOrDefault(x => x.Name == messageReply);

                            if (productSelected == null)
                            {
                                await MessageResponseUnexpected(convo.WaId);
                            }
                            else
                            {
                                order.CurrentProduct = product.Id;
                                _context.SaveChanges();

                                //Set Status to Brand Selection
                                await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.ProductAddToCardConfirm);

                                //Send Product Message
                                await MessageFoodOrderProductConfirmation(convo.WaId, productSelected);                                
                            }
                        }
                        
                        #endregion
                        break;

                    case (int)WatiFoodOrderStatusEnum.ProductSelectionText:
                        #region Product Selection Text
                        int productSelection;

                        try
                        {
                            productSelection = Convert.ToInt32(messageReply);
                        }
                        catch
                        {
                            await MessageResponseUnexpected(convo.WaId);
                            return;
                        }

                        if (productSelection == 0)
                        {                            
                            await ProductCategorySelectionFunction(convo, order, brand, tenantId);
                        }
                        else
                        {
                            var productFromText = (await _pointOfSaleService.ProductList(categoryId))[productSelection - 1];

                            if (productFromText == null)
                            {
                                await MessageResponseUnexpected(convo.WaId);
                                return;
                            }
                            
                            //Set Status to Confirm Add To Cart
                            await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.ProductAddToCardConfirm);

                            //Send Product Message
                            await MessageFoodOrderProductConfirmation(convo.WaId, productFromText);

                            order.CurrentProduct = productFromText.Id;
                            _context.SaveChanges();
                        }
                        
                        #endregion
                        break;

                    case (int)WatiFoodOrderStatusEnum.ProductAddToCardConfirm:
                        #region Product Add to Cart Confirm                                                   
                        switch (messageReply)
                        {
                            case REPLY_ADD_TO_CART:                                
                                //Set Status to Confirm Add To Cart
                                await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.QuantityConfirm);

                                //Send Quanity Confirmation
                                await MessageFoodOrderProductQuantity(convo.WaId, product);                                
                                break;

                            case REPLY_CANCEL:
                                //Send Product Selection
                                await ProductSelectionFunction(convo, order, category);
                                break;

                            default:
                                await MessageResponseUnexpected(convo.WaId);
                                break;
                        }
                        #endregion
                        break;

                    case (int)WatiFoodOrderStatusEnum.QuantityConfirm:
                        #region Quantity Confirm
                        int quantity;

                        try
                        {
                            quantity = Convert.ToInt16(messageReply);
                        }
                        catch 
                        {
                            await MessageResponseUnexpected(convo.WaId);
                            return;
                        }

                        var orderFromQuantityConfirm = await FoodOrderDetailCreate(order.Id, product, quantity);

                        //Check For Product Extra
                        productExtraList = await _pointOfSaleService.ProductExtraList(product.Id);

                        if (productExtraList.Count == 0)
                        {
                            //Set Status 
                            await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.ProductMoreCheckoutConfirm);

                            //Send Message Add More Products
                            await MessageFoodOrderMoreProductsConfirmation(convo.WaId, product, orderFromQuantityConfirm);
                        }
                        else
                        {
                            //Has extras
                            extra = productExtraList[0];
                            order.CurrentExtraCount = productExtraList.Count;
                            order.CurrentExtraCompleted = 0;
                            order.CurrentProductExtra = extra.Id;
                            order.CurentExtraOptionMin = extra.Min;
                            order.CurrentExtraOptionMax = extra.Max;
                            order.CurrentExtraOptionSelected = 0;
                            _context.SaveChanges();

                            //Set Status to Confirm Add To Cart
                            await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.ProductExtraSelection);

                            //Send Message Product Extra
                            await MessageFoodOrderProductExraNotification(convo.WaId, product, productExtraList);

                            //Send Message First Product Exra                            
                            await MessageFoodOrderProductExtraSelection(convo.WaId, product, extra, (int)order.CurrentExtraOptionSelected);
                        }                        
                        #endregion
                        break;

                    case (int)WatiFoodOrderStatusEnum.ProductExtraSelection:
                        #region Product Extra Selection
                        switch (messageReply)
                        {
                            case REPLY_EXTRA_NONE:
                                //This extra completed
                                order.CurrentExtraCompleted++;
                                _context.SaveChanges();
                                
                                if (order.CurrentExtraCount > order.CurrentExtraCompleted)
                                {
                                    //More Extras to be selected                                            
                                    productExtraList = await _pointOfSaleService.ProductExtraList(product.Id);
                                    extra = productExtraList[(int)order.CurrentExtraCompleted];
                                    order.CurrentProductExtra = extra.Id;
                                    order.CurrentExtraOptionSelected = 0;
                                    _context.SaveChanges();

                                    await MessageFoodOrderProductExtraSelection(convo.WaId, product, extra, (int)order.CurrentExtraOptionSelected);
                                }
                                else
                                {
                                    // All Extras for product selected

                                    //Set Status 
                                    await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.ProductMoreCheckoutConfirm);

                                    //Send Message Add More Products
                                    await MessageFoodOrderMoreProductsConfirmation(convo.WaId, product, order);
                                }

                                break;
                            
                            default:                                
                                var extraOptionSelected = (await _pointOfSaleService.ProductExtraOptionList(extraId)).SingleOrDefault(x => x.Name == messageReply);

                                if (extraOptionSelected == null)
                                {
                                    await MessageResponseUnexpected(convo.WaId);
                                }
                                else
                                {
                                    //Save Extra Option Selection
                                    var orderDetailFromExtraConfirm = await FoodOrderDetailOptionCreate(orderDetail, extraOptionSelected);

                                    //Set Total
                                    order.SubTotal = await FoodOrderTotalsGet(order.Id);

                                    //Add to extra completed Counter
                                    order.CurrentExtraOptionSelected++;
                                    _context.SaveChanges();



                                    //Check if the same options needs to be shown again
                                    if (order.CurrentExtraOptionSelected < order.CurrentExtraOptionMax)
                                    {
                                        //Set Status to Confirm Add To Cart
                                        await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.ProductExtraSelectionMoreConfirm);

                                        //Message Topping Status
                                        await MessageFoodOrderProductExtraMoreQuestion(convo.WaId, extra, (int)order.CurrentExtraOptionSelected);
                                    }
                                    else
                                    {
                                        //Max Reached

                                        order.CurrentExtraCompleted++;
                                        _context.SaveChanges();

                                        if (order.CurrentExtraCount > order.CurrentExtraCompleted)
                                        {
                                            //More Extras to be selected                                            
                                            productExtraList = await _pointOfSaleService.ProductExtraList(product.Id);
                                            extra = productExtraList[(int)order.CurrentExtraCompleted];
                                            order.CurrentProductExtra = extra.Id;
                                            order.CurrentExtraOptionSelected = 0;
                                            _context.SaveChanges();

                                            await MessageFoodOrderProductExtraSelection(convo.WaId, product, extra, (int)order.CurrentExtraOptionSelected);
                                        }
                                        else
                                        {
                                            // All Extras for product selected

                                            //Set Status 
                                            await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.ProductMoreCheckoutConfirm);

                                            //Send Message Add More Products
                                            await MessageFoodOrderMoreProductsConfirmation(convo.WaId, product, order);
                                        }
                                    }
                                }
                                break;
                        }
                        #endregion 

                        break;

                    case (int)WatiFoodOrderStatusEnum.ProductExtraSelectionMoreConfirm:
                        #region Product Extra Selection
                        switch (messageReply)
                        {
                            case REPLY_YES:
                                //Set Status to Confirm Add To Cart
                                await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.ProductExtraSelection);

                                //Send Message Product Extra
                                await MessageFoodOrderProductExtraSelection(convo.WaId, product, extra, (int)order.CurrentExtraOptionSelected);
                                break;

                            case REPLY_NO:
                                order.CurrentExtraCompleted++;
                                _context.SaveChanges();

                                if (order.CurrentExtraCount > order.CurrentExtraCompleted)
                                {
                                    //More Extras to be selected                                            
                                    productExtraList = await _pointOfSaleService.ProductExtraList(product.Id);
                                    extra = productExtraList[(int)order.CurrentExtraCompleted];
                                    order.CurrentProductExtra = extra.Id;
                                    order.CurrentExtraOptionSelected = 0;
                                    _context.SaveChanges();

                                    //Set Status to Confirm Add To Cart
                                    await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.ProductExtraSelection);

                                    //Send Message Product Extra - Next Extra
                                    await MessageFoodOrderProductExtraSelection(convo.WaId, product, extra, (int)order.CurrentExtraOptionSelected);
                                }
                                else
                                {
                                    // All Extras for product selected

                                    //Set Status 
                                    await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.ProductMoreCheckoutConfirm);

                                    //Send Message Add More Products
                                    await MessageFoodOrderMoreProductsConfirmation(convo.WaId, product, order);
                                }                                
                                break;

                            default:
                                await MessageResponseUnexpected(convo.WaId);
                                return;                                
                        }
                        _context.SaveChanges();

                        
                        #endregion
                        break;

                    case (int)WatiFoodOrderStatusEnum.ProductMoreCheckoutConfirm:                        
                        #region Product More Checkout Confirm
                        switch (messageReply)
                        {
                            case REPLY_ADD_MORE_PRODUCTS:
                                //Brand or Category Selection                                
                                await BrandOrCategorySelectionFunction(convo, order, tenantId, tenant);
                                break;

                            case REPLY_VIEW_CART:
                                //Set Status View Cart
                                //Status alread here

                                //Send Message View Cart
                                await MessageFoodOrderMoreProductsConfirmationRemixViewCart(convo.WaId, order);
                                break;

                            case REPLY_CHECKOUT:
                                //Set Status View Cart
                                //await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.OrderConfirm);

                                //Send Message View Cart
                                await MessageFoodOrderMoreProductsConfirmationRemixCheckOut(convo.WaId, order);
                                break;

                            case REPLY_CANCEL_ORDER:                                                                
                                //Set Status Cancel Confirm
                                await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.CancelConfirm);

                                //Send Message Cancel Confirm
                                await MessageFoodOrderCancelConfirmation(convo.WaId, order);                                
                                break;

                            case REPLY_YES:
                                //Set Status Pay Meth Confirm
                                await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.OrderPayMethodConfirm);

                                //Send Message Pay Method Confirm
                                await MessageFoodOrderPayMethodConfirm(convo.WaId);

                                break;

                            case REPLY_NO:
                                //Senn Message Confirm Order
                                await MessageFoodOrderMoreProductsConfirmationRemixReplyNo(convo.WaId, order);
                                break;

                            default:
                                await MessageResponseUnexpected(convo.WaId);
                                break;
                        }
                        #endregion
                        break;

                    case (int)WatiFoodOrderStatusEnum.CancelConfirm:
                        #region CancelConfirm
                        switch (messageReply)
                        {
                            case REPLY_YES:
                                //Set Conversation to Cancelled
                                await ConversationStatusSet(convo.Id, WatiConversationStatusEnum.Cancelled);
                                convo.CompletedDate= DateTime.Now;

                                //Update Database
                                order.RejectDate = DateTime.Now;
                                order.RejectReason = CANCELLED_BY_USER;
                                _context.SaveChanges();

                                //Send Message Cancel Confirm
                                await _wati.SessionMessageSend(convo.WaId, "You order has been cancelled. We hope to chat with you again soon. Thank you!");
                                break;

                            case REPLY_NO:
                                //Set Status to Category Selection
                                await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.ProductMoreCheckoutConfirm);

                                //Send Message Add More Products (Armand Van Helden REMIX)
                                await MessageFoodOrderMoreProductsConfirmationRemixReplyNo(convo.WaId, order);
                                break;

                            default:
                                await MessageResponseUnexpected(convo.WaId);
                                break;

                        }
                        #endregion
                        break;
                    
                    case (int)WatiFoodOrderStatusEnum.OrderPayMethodConfirm:
                        #region Paymethod Confirm
                        switch (messageReply)
                        {
                            case REPLY_PAYMENT_CASH:
                                order.PaymentMethodId = (int)PosPaymentTypeEnum.Cash;
                                break;

                            case REPLY_PAYMENT_CARD:
                                order.PaymentMethodId = (int)PosPaymentTypeEnum.Card;
                                break;

                            case REPLY_PAYMENT_EFT:
                                order.PaymentMethodId = (int)PosPaymentTypeEnum.EFT; 
                                break;
                        }
                        _context.SaveChanges();


                        var saveTotal = await FoodOrderTotalsGet(order.Id);
                        var productList = await FoodOrderCartGet(order.Id);

                        var result = await _pointOfSaleService.OrderSave(order.Id, saveTotal, productList);

                        if (result.success)
                        {
                            await MessageOrderSaved(convo.WaId, result.orderId);
                        }
                        else
                        {
                            await _wati.SessionMessageSend(convo.WaId, $"Error saving order: {result.error}");   
                        }
                        #endregion
                        break;

                    default:                        
                        await MessageResponseUnexpected(convo.WaId);
                        break;
                }
            }
            else
            {
                await MessageResponseUnexpected(convo.WaId);
            }
        }
        #endregion

        #region WhatsApp Message Send
        private async Task MessageTemplateMessage(string whatsappNumber)
        { 
            TemplateMessageSendDto dto = new TemplateMessageSendDto();
            dto.Template_Name = "order_accepted_v3";
            dto.Broadcast_Name = "Test";

            var parameterList = new List<TemplateMessageParameter>
            {
                new TemplateMessageParameter { Name = "order_number", Value= "37" },
                new TemplateMessageParameter { Name = "shop_name", Value= "Bibis" },
                new TemplateMessageParameter { Name = "shop_whatsapp_url", Value= "https://wa.me/27839777068/" }
            };

            dto.Parameters = parameterList;

            await _wati.TemplateMessageSend(whatsappNumber, dto);
        }
        private async Task MessageWelcome(string whatsappNumber, bool isOrderFood)
        {
            InteractiveListMessageDto welcome = new InteractiveListMessageDto();
            List<InteractiveListMessageSection> sectionList = new List<InteractiveListMessageSection>();
            List<InteractivelistMessageSectionRow> rowList = new List<InteractivelistMessageSectionRow>();

            welcome.Header = "Welcome to Pekkish";

            welcome.Body = "How can we help you today?";
            welcome.Body += "\r\n";
            welcome.Body += "\r\n";
            welcome.Body += $"Please note: ";
            welcome.Body += "\r\n";
            welcome.Body += $"At any moment during conversation you can type '{REPLY_RESTART}' to start over, or '{REPLY_HELP}' to speak to a member of our team.";
            
            welcome.Footer = "";
            welcome.ButtonText = "Choose Option";

            if (isOrderFood)
            {
                rowList.Add(new InteractivelistMessageSectionRow
                {
                    Title = "Order Food",
                    Description = ""
                });
            }

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
        private async Task MessageResponseUnexpected(string whatsappNumber) 
        {
            string message = "Your response is unexpected.";
            message += "\r\n";
            message += "\r\n";
            message += $"Please try again, or type '{REPLY_RESTART}' to start over, or '{REPLY_HELP}' to speak to a member of our team.";

            await _wati.SessionMessageSend(whatsappNumber, message);
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
        private async Task MessageFoodOrderVendorWelcome(string whatsappNumber, Guid tenantId, AppTenantInfo tenant)
        {
            var messageMedia = new InteractiveButtonsMessageMediaDto();
            var messageText = new InteractiveButtonsMessageTextDto();
            var headerMedia = new InteractiveButtonMessageHeaderMedia();
            var headerText = new InteractiveButtonMessageHeaderText();
            var media = new InteractiveButtonHeaderMedia();
            var buttonList = new List<InteractiveButtonMessageButton>();
            var locationList = await _pointOfSaleService.LocationList(tenantId);
            var location = new AppLocation();
            
            if (locationList.Count == 1)
            {
                location = locationList[0];                
            }
            else
            {
                //TODO: Location Selection
            }

            #region Buttons
            buttonList.Add(new InteractiveButtonMessageButton()
            {
                Text = REPLY_VENDOR_ORDER_FOOD
            });

            buttonList.Add(new InteractiveButtonMessageButton()
            {
                Text = REPLY_CANCEL
            });
            #endregion

            #region Determine if image is available
            var isImage = false;
            if (location.PekkishVendorId != null)
            { 
                var pekkishVendor = _context.PekkishVendors.SingleOrDefault(x=>x.Id == location.PekkishVendorId);

                if(pekkishVendor != null)
                {
                    if (pekkishVendor.Logo != null)
                    { 
                        media.Url = pekkishVendor.Logo;
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

                messageMedia.Body += $"*{tenant.Name}*";
                messageMedia.Body += "\r\n";

                if (!tenant.WelcomeMessage.IsNullOrEmpty())
                {
                    messageMedia.Body += "\r\n";
                    messageMedia.Body += tenant.WelcomeMessage;
                    messageMedia.Body += "\r\n";
                    messageMedia.Body += "\r\n";
                }
                
                messageMedia.Footer = "";

                messageMedia.Buttons = buttonList;

                var result = await _wati.InteractiveButtonsMessageMediaSend(whatsappNumber, messageMedia);
            }
            else
            {
                headerText.Type = "Text";
                headerText.Text = tenant.NameShort;
                messageText.Header = headerText;

                if (!tenant.WelcomeMessage.IsNullOrEmpty())
                {
                    messageText.Body += "\r\n";
                    messageText.Body += tenant.WelcomeMessage;
                    messageText.Body += "\r\n";
                    messageText.Body += "\r\n";
                }
                else
                {
                    messageText.Body = $"Welcome to {tenant.Name}";
                }
                
                messageText.Footer = "";

                messageText.Buttons = buttonList;

                var result = await _wati.InteractiveButtonsMessageTextSend(whatsappNumber, messageText);
            }
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
        private async Task MessageFoodOrderProductCategorySelectionList(string whatsappNumber, AppBrand brand, Guid tenantId)
        {
            var message = new InteractiveListMessageDto();
            var sectionList = new List<InteractiveListMessageSection>();
            var rowList = new List<InteractivelistMessageSectionRow>();
            var rowBackList = new List<InteractivelistMessageSectionRow>();
            var brandList = await _pointOfSaleService.BrandList(tenantId);
            var categoryList = await _pointOfSaleService.ProductCategoryList(brand.Id);

            message.Header = brand.NameShort.ToString();
            message.Body = "Please select from the following categories:";
            message.Footer = "";
            message.ButtonText = "Choose Category";

            if (brandList.Count == 1)
            {
                //Back to Categories
                rowBackList.Add(new InteractivelistMessageSectionRow
                {
                    Title = REPLY_BACK_VENDOR,
                    Description = ""
                });

                sectionList.Add(new InteractiveListMessageSection
                {
                    Title = $"Vendor",
                    Rows = rowBackList
                });
            }
            else
            {
                //Back to Brands
                rowBackList.Add(new InteractivelistMessageSectionRow
                {
                    Title = REPLY_BACK_BRAND,
                    Description = ""
                });

                sectionList.Add(new InteractiveListMessageSection
                {
                    Title = $"Brands",
                    Rows = rowBackList
                });
            }
            
            foreach (var item in categoryList)
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
        private async Task MessageFoodOrderProductCategorySelectionText(string whatsappNumber, AppBrand brand, Guid tenantId)
        {
            string message;
            var categoryList = await _pointOfSaleService.ProductCategoryList(brand.Id);
            var brandList = await _pointOfSaleService.BrandList(tenantId);

            message = brand.Name.Replace('&', '_').ToUpper();
            message += "\r\n";
            message += "\r\n";
            message += "Please enter the number on the left hand side of your selection.";
            message += "\r\n";
            message += "\r\n";


            // Go back to either brand or store
            if (brandList.Count == 1)
            {
                message += "Vendor:";
                message += "\r\n";
                message += $"0 {REPLY_BACK_VENDOR}";
            }
            else
            {
                message += "Brands:";
                message += "\r\n";
                message += $"0 {REPLY_BACK_BRAND}";
            }

            message += "\r\n";
            message += "\r\n";
            message += "Categories:";
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
            message += "Please enter the number on the left hand side of your selection.";
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
                message += $"{c + 1} {productList[c].Name}";
                message += "\r\n";
                message += $"  R{productList[c].Price}";
                message += "\r\n";
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
        private async Task MessageFoodOrderProductExraNotification(string whatsappNumber, AppProduct product, List<AppProductExtra> productExtraList)
        {
            string message = "";

            if (productExtraList.Count == 1)
            {
                message = $"Your selection has {productExtraList.Count} product extra choice:";
                message += "\r\n";
                message += productExtraList.First().Name;
            }
            else
            {
                message = $"Your selection has {productExtraList.Count} product extra choices:";
                message += "\r\n";

                foreach (var extra in productExtraList)
                {
                    message += extra.Name;
                    message += "\r\n";
                }                
            }

            await _wati.SessionMessageSend(whatsappNumber, message);
        }
        private async Task MessageFoodOrderProductExtraSelection(string whatsappNumber, AppProduct product, AppProductExtra productExtra, int optionSelected)
        {
            var message = new InteractiveListMessageDto();
            var sectionList = new List<InteractiveListMessageSection>();            
            var rowProductExtraList = new List<InteractivelistMessageSectionRow>();
            var productExtraOptionList = await _pointOfSaleService.ProductExtraOptionList(productExtra.Id);
                       
            message.Header = product.Name;
            message.Body = $"Please select from the following {productExtra.Name}:";
            message.Footer = "";
            message.ButtonText = $"{productExtra.Name}";

            //Nothing to Select
            if (productExtra.Min == 0)
            {
                rowProductExtraList.Add(new InteractivelistMessageSectionRow
                {
                    Title = REPLY_EXTRA_NONE
                });
            }
            else
            {
                if (optionSelected >= productExtra.Min)
                {
                    rowProductExtraList.Add(new InteractivelistMessageSectionRow
                    {
                        Title = REPLY_EXTRA_NONE
                    });
                }
            }

            //Products Extra
            foreach (var item in productExtraOptionList)
            {
                rowProductExtraList.Add(new InteractivelistMessageSectionRow
                {
                    Title = item.Name,
                    Description = $"R {item.Price}"
                });
            }

            //Complile Lists
            sectionList.Add(new InteractiveListMessageSection
            {
                Title = $"{productExtra.Name}",
                Rows = rowProductExtraList
            });

            message.Sections = sectionList;

            var result = await _wati.InteractiveListMessageSend(whatsappNumber, message);
        }
        private async Task MessageFoodOrderProductExtraMoreQuestion(string whatsappNumber, AppProductExtra productExtra, int optionSelected)
        {            
            var messageText = new InteractiveButtonsMessageTextDto();
            var headerText = new InteractiveButtonMessageHeaderText();
            var buttonList = new List<InteractiveButtonMessageButton>();

            #region Buttons
            buttonList.Add(new InteractiveButtonMessageButton() { Text = REPLY_YES });
            buttonList.Add(new InteractiveButtonMessageButton() { Text = REPLY_NO });            
            #endregion

            headerText.Type = "Text";
            headerText.Text = $"{productExtra.Name}";

            messageText.Header = headerText;

            messageText.Body += $"You have selected {optionSelected} out of a possible {productExtra.Max} {productExtra.Name}. Do you want to select more?";


            messageText.Footer = "";

            messageText.Buttons = buttonList;

            var result = await _wati.InteractiveButtonsMessageTextSend(whatsappNumber, messageText);
        }
        private async Task MessageFoodOrderProductQuantity(string whatsappNumber, AppProduct product)
        {
            var message = new InteractiveListMessageDto();
            var sectionList = new List<InteractiveListMessageSection>();
            var rowCategoryList = new List<InteractivelistMessageSectionRow>();
            var rowQuantityList = new List<InteractivelistMessageSectionRow>();
            //var productList = await _pointOfSaleService.ProductList(category.Id);

            message.Header = product.Name;
            message.Body = "Please select quantity:";
            message.Footer = "";
            message.ButtonText = "Choose Quantity";
          
            //Quantities
            rowQuantityList.Add(new InteractivelistMessageSectionRow { Title = "1", Description = "" });
            rowQuantityList.Add(new InteractivelistMessageSectionRow { Title = "2", Description = "" });
            rowQuantityList.Add(new InteractivelistMessageSectionRow { Title = "3", Description = "" });
            rowQuantityList.Add(new InteractivelistMessageSectionRow { Title = "4", Description = "" });
            rowQuantityList.Add(new InteractivelistMessageSectionRow { Title = "5", Description = "" });
            rowQuantityList.Add(new InteractivelistMessageSectionRow { Title = "6", Description = "" });
            //rowQuantityList.Add(new InteractivelistMessageSectionRow { Title = "7", Description = "" });
            //rowQuantityList.Add(new InteractivelistMessageSectionRow { Title = "8", Description = "" });
            //rowQuantityList.Add(new InteractivelistMessageSectionRow { Title = "9", Description = "" });
            //rowQuantityList.Add(new InteractivelistMessageSectionRow { Title = "10", Description = "" });

            //Complile Lists
            sectionList.Add(new InteractiveListMessageSection
            {
                Title = $"Quantities",
                Rows = rowQuantityList
            });

            message.Sections = sectionList;

            var result = await _wati.InteractiveListMessageSend(whatsappNumber, message);
        }
        private async Task MessageFoodOrderMoreProductsConfirmation(string whatsappNumber, AppProduct product, AppWatiOrder order)
        {
            var messageText = new InteractiveButtonsMessageTextDto();
            var headerText = new InteractiveButtonMessageHeaderText();
            var buttonList = new List<InteractiveButtonMessageButton>();

            #region Buttons
            buttonList.Add(new InteractiveButtonMessageButton() { Text = REPLY_ADD_MORE_PRODUCTS });
            buttonList.Add(new InteractiveButtonMessageButton() { Text = REPLY_VIEW_CART });
            buttonList.Add(new InteractiveButtonMessageButton() { Text = REPLY_CANCEL_ORDER });
            #endregion


            headerText.Type = "Text";
            headerText.Text = $"{product.Name}";

            messageText.Header = headerText;

            messageText.Body = $"Your product has been added to cart. Your cart value is R{order.SubTotal.ToString("F2")}.";
            messageText.Body += "\r\n";
            messageText.Body += "\r\n";
            messageText.Body += "What would you lke to do next?";
            

            messageText.Footer = "";
                
            messageText.Buttons = buttonList;

            var result = await _wati.InteractiveButtonsMessageTextSend(whatsappNumber, messageText);
        }
        private async Task MessageFoodOrderMoreProductsConfirmationRemixReplyNo(string whatsappNumber, AppWatiOrder order)
        {
            var messageText = new InteractiveButtonsMessageTextDto();
            var headerText = new InteractiveButtonMessageHeaderText();
            var buttonList = new List<InteractiveButtonMessageButton>();

            #region Buttons
            buttonList.Add(new InteractiveButtonMessageButton() { Text = REPLY_ADD_MORE_PRODUCTS });
            buttonList.Add(new InteractiveButtonMessageButton() { Text = REPLY_VIEW_CART });
            buttonList.Add(new InteractiveButtonMessageButton() { Text = REPLY_CANCEL_ORDER });
            #endregion


            headerText.Type = "Text";
            headerText.Text = $"Current Order";

            messageText.Header = headerText;

            messageText.Body = $"Your cart value is R{order.SubTotal.ToString("F2")}.";
            messageText.Body += "\r\n";
            messageText.Body += "\r\n";
            messageText.Body += "What would you lke to do next?";


            messageText.Footer = "";

            messageText.Buttons = buttonList;

            var result = await _wati.InteractiveButtonsMessageTextSend(whatsappNumber, messageText);
        }
        private async Task MessageFoodOrderMoreProductsConfirmationRemixViewCart(string whatsappNumber, AppWatiOrder order)
        {
            var messageText = new InteractiveButtonsMessageTextDto();
            var headerText = new InteractiveButtonMessageHeaderText();
            var buttonList = new List<InteractiveButtonMessageButton>();

            var cartList = await FoodOrderCartGet(order.Id);
            var cartTotal = await FoodOrderTotalsGet(order.Id);

            #region Buttons
            buttonList.Add(new InteractiveButtonMessageButton() { Text = REPLY_CHECKOUT });
            buttonList.Add(new InteractiveButtonMessageButton() { Text = REPLY_ADD_MORE_PRODUCTS });
            buttonList.Add(new InteractiveButtonMessageButton() { Text = REPLY_CANCEL_ORDER });
            #endregion

            headerText.Type = "Text";
            headerText.Text = $"Your Cart";

            messageText.Header = headerText;

            messageText.Body = "";

            foreach (var product in cartList)
            {
                messageText.Body += $"{product.Quantity} X {product.Name}";
                messageText.Body += "\r\n";                

                var productExtraOptionList = (from Option in _context.AppWatiOrderDetailOptions
                                              where Option.WatiOrderDetailId == product.Id
                                              select Option).ToList();

                foreach(var extraOption in productExtraOptionList)
                {
                    //var extra = _context.AppProductExtras.Single(x => x.Id == extraOption.ProductExtraId);

                    messageText.Body += $"    {extraOption.Name}";
                    messageText.Body += "\r\n";
                }

                messageText.Body += $"    R{product.Quantity * product.Amount}";
                messageText.Body += "\r\n";
                messageText.Body += "\r\n";
            }

            messageText.Body += $"Your order total is {cartTotal.ToString("F2")}";            
            messageText.Body += "\r\n";
            messageText.Body += "\r\n";

            messageText.Body += "What would you like to do next?";

            messageText.Footer = "";

            messageText.Buttons = buttonList;

            var result = await _wati.InteractiveButtonsMessageTextSend(whatsappNumber, messageText);
        }
        private async Task MessageFoodOrderMoreProductsConfirmationRemixCheckOut(string whatsappNumber, AppWatiOrder order)
        {
            var messageText = new InteractiveButtonsMessageTextDto();
            var headerText = new InteractiveButtonMessageHeaderText();
            var buttonList = new List<InteractiveButtonMessageButton>();

            var cartList = await FoodOrderCartGet(order.Id);

            #region Buttons
            buttonList.Add(new InteractiveButtonMessageButton() { Text = REPLY_YES });
            buttonList.Add(new InteractiveButtonMessageButton() { Text = REPLY_NO });
            #endregion

            headerText.Type = "Text";
            headerText.Text = $"Check Out";

            messageText.Header = headerText;

            messageText.Body = "";

            messageText.Body += "Are you sure you would lke to check out?";

            messageText.Footer = "";

            messageText.Buttons = buttonList;

            var result = await _wati.InteractiveButtonsMessageTextSend(whatsappNumber, messageText);
        }
        private async Task MessageFoodOrderCancelConfirmation(string whatsappNumber, AppWatiOrder order)
        {
            var messageText = new InteractiveButtonsMessageTextDto();
            var headerText = new InteractiveButtonMessageHeaderText();
            var buttonList = new List<InteractiveButtonMessageButton>();

            #region Buttons
            buttonList.Add(new InteractiveButtonMessageButton() { Text = REPLY_YES });
            buttonList.Add(new InteractiveButtonMessageButton() { Text = REPLY_NO });            
            #endregion


            headerText.Type = "Text";
            headerText.Text = $"Cancel Order";

            messageText.Header = headerText;

            messageText.Body += "Are you sure?";


            messageText.Footer = "";

            messageText.Buttons = buttonList;

            var result = await _wati.InteractiveButtonsMessageTextSend(whatsappNumber, messageText);
        }
        private async Task MessageFoodOrderPayMethodConfirm(string whatsappNumber)
        {
            var messageText = new InteractiveButtonsMessageTextDto();
            var headerText = new InteractiveButtonMessageHeaderText();
            var buttonList = new List<InteractiveButtonMessageButton>();

            #region Buttons
            buttonList.Add(new InteractiveButtonMessageButton() { Text = REPLY_PAYMENT_CASH });
            buttonList.Add(new InteractiveButtonMessageButton() { Text = REPLY_PAYMENT_CARD });            
            buttonList.Add(new InteractiveButtonMessageButton() { Text = REPLY_PAYMENT_EFT });
            #endregion

            headerText.Type = "Text";
            headerText.Text = $"Payment Method";

            messageText.Header = headerText;

            messageText.Body += "Please confirm payment method.";


            messageText.Footer = "";

            messageText.Buttons = buttonList;

            var result = await _wati.InteractiveButtonsMessageTextSend(whatsappNumber, messageText);
        }
        private async Task MessageOrderSaved(string whatsappNumber, int orderId)
        {
            string message = "";
            message += "Your order has been submitted for processing. You will be notified shortly with the vendor's response";
            message += "\r\n";
            message += "\r\n";
            message += $"Your order number is {orderId}";

            await _wati.SessionMessageSend(whatsappNumber, message);
        }
        #endregion

        private async Task SessionAssignCustomerService(string whatsappNumber)
        {
            await _wati.SessionAssignOperator(whatsappNumber, _watiConfig.DefaultOperatorEmail);
        }        
    }
}
