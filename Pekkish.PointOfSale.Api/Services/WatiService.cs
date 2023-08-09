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
        private readonly object whatsappNumber;

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
                            await MessageFoodOrderVendorWelcome(convo.WaId, tenantId, tenant, true);
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
                AppWatiUser watiUser;
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

                if (order.CurrentOrderDetail != null)
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

                if (order.WaId != null)
                {
                    watiUser = _context.AppWatiUsers.Single(x => x.WaId == message.WaId);
                }
                else
                {
                    watiUser = new AppWatiUser();
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
                                await MessageFoodOrderVendorWelcome(convo.WaId, (Guid)order.TenantId, tenantSelected, true);
                            }
                        }
                        #endregion
                        break;

                    case (int)WatiFoodOrderStatusEnum.VendorLanding:
                        #region Vendor Landing                       
                        switch (messageReply)
                        {
                            case REPLY_PLACE_ORDER_PICKUP:
                                //Brand or Category Selection
                                await BrandOrCategorySelectionFunction(convo, order, tenantId, tenant);
                                break;

                            case REPLY_PLACE_ORDER_DELIVERY:
                                //Check if users's address is saved?                                

                                //Set Food Order Fulfillment to Delivery                                    
                                order.OrderFulfillmentId = (int)PosFulfillmentTypeEnum.Delivery;
                                _context.SaveChanges();

                                if (watiUser.AddressStreet.IsNullOrEmpty() || watiUser.AddressSuburb.IsNullOrEmpty() || watiUser.AddressPostCode.IsNullOrEmpty())
                                {                                    
                                    //Set Status Address Confirm
                                    await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.AddressStreetConfirm);

                                    //Message - We require your delivery address in order to continue                                    
                                    await _wati.SessionMessageSend(message.WaId, "We currently do not have your address saved.");

                                    await MessageAddressRequired(message.WaId);

                                    //Message Get Address Street
                                    await _wati.SessionMessageSend(message.WaId, "Please enter your street address.");
                                }
                                else
                                {
                                    //Set Status Address Correct Confirm
                                    await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.AddressCorrectConfirm);

                                    //Message Address Correct Confirm
                                    await MessageAddressCorrectConfirmation(message.WaId, watiUser);
                                }
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

                    case (int)WatiFoodOrderStatusEnum.AddressStreetConfirm:
                        #region Addresss Street Confirm
                        //Save Address Street
                        watiUser.AddressStreet = messageReply;
                        _context.SaveChanges();

                        //Status set Adddress Suburb Confirm
                        await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.AddressSuburbConfirm);

                        //Message Confirm Suburb
                        //Message Get Address Street
                        await _wati.SessionMessageSend(message.WaId, "Please enter your suburb.");
                        #endregion

                        break;

                    case (int)WatiFoodOrderStatusEnum.AddressSuburbConfirm:
                        #region Address Suburb Confirm
                        //Save Address Suburb
                        watiUser.AddressSuburb = messageReply;
                        _context.SaveChanges();

                        //Status set Adddress Suburb Confirm
                        await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.AddressPostCodeConfirm);

                        //Message Confirm Post Code
                        //Message Get Address Street
                        await _wati.SessionMessageSend(message.WaId, "Please enter your post code.");
                        #endregion

                        break;

                    case (int)WatiFoodOrderStatusEnum.AddressPostCodeConfirm:
                        #region Address Post Code Confirm
                        var postCodeCheck = _context.AppOrderPostCodes.FirstOrDefault(x => x.PostCode == messageReply);

                        if (postCodeCheck != null)
                        {
                            //Save Address Post Code
                            watiUser.AddressPostCode = messageReply;
                            _context.SaveChanges();

                            //Status set Adddress Correct Confirm
                            await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.AddressCorrectConfirm);

                            //Message Address Correct Confirm
                            await MessageAddressCorrectConfirmation(message.WaId, watiUser);
                        }
                        else
                        {
                            await _wati.SessionMessageSend(message.WaId, "Your post code supplied is invalid. Please enter your postcode again.");
                        }                        
                        #endregion
                        break;

                    case (int)WatiFoodOrderStatusEnum.AddressCorrectConfirm:
                        #region Address Correct Confirm
                        switch (messageReply)
                        {
                            case REPLY_YES:
                                //Check if Vendor Delivers to Post Code
                                var deliveryCheck = _context.AppOrderDeliveryFees.FirstOrDefault(x => x.TenantId == tenant.TenantId && x.PostCode == watiUser.AddressPostCode);

                                if (deliveryCheck != null)
                                {
                                    //Save delivery Fee
                                    order.DeliveryFee = deliveryCheck.DeliveryFee;
                                    _context.SaveChanges();

                                    //Set Status Delveriry Cost OK Confirm
                                    await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.DeliveryCostOkConfirm);

                                    //Message Delivery Cost Ok Question
                                    await MessageDeliveryCostConfirmation(message.WaId, watiUser, tenant, (order.DeliveryFee == null) ? 0 : (decimal)order.DeliveryFee);
                                }
                                else
                                {
                                    //Does Not Deliver
                                    await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.NoDeliveryPickupConfirm);

                                    //Message No Delivery Pickup Confirm
                                    MessageNoDeliveryPickupConfirmation(message.WaId, watiUser, tenant);
                                }

                                break;

                            case REPLY_NO:
                                //Set Status Receive Address Street
                                await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.AddressStreetConfirm);

                                //Message Address Street Confirm
                                await MessageAddressRequired(message.WaId);

                                //Message Get Address Street
                                await _wati.SessionMessageSend(message.WaId, "Please enter your street address.");

                                break;

                            default:
                                await MessageResponseUnexpected(convo.WaId);
                                return;
                        }
                        #endregion
                        break;

                    case (int)WatiFoodOrderStatusEnum.DeliveryCostOkConfirm:
                        #region Delivery Cost OK Confirm
                        switch (messageReply)
                        {
                            case REPLY_YES:                                
                                //Set Status
                                await BrandOrCategorySelectionFunction(convo, order, (Guid)tenant.TenantId, tenant);
                                break;

                            case REPLY_PLACE_ORDER_PICKUP:
                                //Set Order to Pickup Order
                                order.OrderFulfillmentId = (int)PosFulfillmentTypeEnum.Pickup;
                                _context.SaveChanges();

                                //Pickup Order
                                await BrandOrCategorySelectionFunction(convo, order, (Guid)tenant.TenantId, tenant);

                                break;

                            case REPLY_CANCEL_ORDER:
                                //Set Conversation to Cancelled
                                await ConversationStatusSet(convo.Id, WatiConversationStatusEnum.Cancelled);
                                convo.CompletedDate = DateTime.Now;

                                //Update Database
                                order.RejectDate = DateTime.Now;
                                order.RejectReason = CANCELLED_BY_USER;
                                _context.SaveChanges();

                                //Send Message Cancel Confirm
                                await _wati.SessionMessageSend(convo.WaId, $"You order with {tenant.Name} has been cancelled. We hope to chat with you again soon. Thank you!");
                                break;

                            default:
                                await MessageResponseUnexpected(convo.WaId);
                                return;
                        }


                        #endregion
                        break;

                    case (int)WatiFoodOrderStatusEnum.NoDeliveryPickupConfirm:
                        #region No Delivery Pickup Confirm
                        switch (messageReply)
                        {
                            case REPLY_PLACE_ORDER_PICKUP:
                                //Brand or Category Selction
                                await BrandOrCategorySelectionFunction(convo, order, tenantId, tenant);                                
                                break;

                            case REPLY_CANCEL_ORDER:
                                //Set Conversation to Cancelled
                                await ConversationStatusSet(convo.Id, WatiConversationStatusEnum.Cancelled);
                                convo.CompletedDate = DateTime.Now;

                                //Update Database
                                order.RejectDate = DateTime.Now;
                                order.RejectReason = CANCELLED_BY_USER;
                                _context.SaveChanges();

                                //Send Message Cancel Confirm
                                await _wati.SessionMessageSend(convo.WaId, $"You order with {tenant.Name} has been cancelled. We hope to chat with you again soon. Thank you!");
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
                                await MessageFoodOrderVendorWelcome(convo.WaId, tenantId, tenant, true);

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
                                await MessageFoodOrderVendorWelcome(convo.WaId, tenantId, tenant, true);
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

                        var orderFromQuantityConfirm = await FoodOrderDetailCreate(order.Id, product, quantity, order.CurrentProductComment);

                        //Check For Product Extra
                        productExtraList = await _pointOfSaleService.ProductExtraList(product.Id);

                        if (productExtraList.Count == 0)
                        {
                            if (tenant.IsWhatsAppSpecialInstruction)
                            {
                                //Set Status to Confirm Add To Cart
                                await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.ProductCommentQuestion);

                                //Send CommentConfirmation
                                await MessageFoodOrderProductCommentQuestion(convo.WaId, product);
                            }
                            else
                            {
                                //Set Status 
                                await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.ProductMoreCheckoutConfirm);

                                //Send Message Add More Products
                                await MessageFoodOrderMoreProductsConfirmation(convo.WaId, product, order);
                            }
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

                    case (int)WatiFoodOrderStatusEnum.ProductCommentQuestion:
                        #region Product Add to Cart Confirm                                                   
                        switch (messageReply)
                        {
                            case REPLY_YES:
                                //Set Status to Confirm Product Comment
                                await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.ProductCommentConfirm);

                                //Send Comment Confirmation
                                await MessageFoodOrderProductCommentConfirm(convo.WaId);
                                break;

                            case REPLY_NO:
                                //Set Status 
                                await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.ProductMoreCheckoutConfirm);

                                //Send Message Add More Products
                                await MessageFoodOrderMoreProductsConfirmation(convo.WaId, product, order);
                                break;

                            default:
                                await MessageResponseUnexpected(convo.WaId);
                                break;
                        }
                        #endregion
                        break;

                    case (int)WatiFoodOrderStatusEnum.ProductCommentConfirm:
                        #region Product Product Confirm                                                                           
                        order.CurrentProductComment = messageReply;
                        _context.SaveChanges();

                        //Set Status 
                        await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.ProductMoreCheckoutConfirm);

                        //Send Message Add More Products
                        await MessageFoodOrderMoreProductsConfirmation(convo.WaId, product, order);

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

                                    if (tenant.IsWhatsAppSpecialInstruction)
                                    {
                                        //Set Status to Confirm Add To Cart
                                        await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.ProductCommentQuestion);

                                        //Send CommentConfirmation
                                        await MessageFoodOrderProductCommentQuestion(convo.WaId, product);
                                    }
                                    else
                                    {
                                        //Set Status 
                                        await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.ProductMoreCheckoutConfirm);

                                        //Send Message Add More Products
                                        await MessageFoodOrderMoreProductsConfirmation(convo.WaId, product, order);
                                    }
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

                                            if (tenant.IsWhatsAppSpecialInstruction)
                                            {
                                                //Set Status to Confirm Add To Cart
                                                await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.ProductCommentQuestion);

                                                //Send CommentConfirmation
                                                await MessageFoodOrderProductCommentQuestion(convo.WaId, product);
                                            }
                                            else
                                            {
                                                //Set Status 
                                                await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.ProductMoreCheckoutConfirm);

                                                //Send Message Add More Products
                                                await MessageFoodOrderMoreProductsConfirmation(convo.WaId, product, order);
                                            }
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

                            case REPLY_CHECKOUT:
                                //Check if can order later
                                var isTooLate = false;

                                if (DateTime.Now > DateTime.Now.AddHours(19))
                                {
                                    isTooLate = true;
                                }

                                if (tenant.IsWhatsAppPreorder && !isTooLate)
                                {
                                    //Set Status Receive Time Confirm
                                    await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.RecieveTimeConfirm);

                                    //Message Revceive Time Confirm
                                    await MessageRecieveTimeConfirmation(convo.WaId);
                                    break;
                                }
                                else
                                {
                                    await MessageFoodOrderCheckOutConfirm(convo.WaId, order);
                                    break;
                                }                                

                            case REPLY_CANCEL_ORDER:
                                //Set Status Cancel Confirm
                                await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.CancelConfirm);

                                //Send Message Cancel Confirm
                                await MessageFoodOrderCancelConfirmation(convo.WaId, order);
                                break;

                            case REPLY_YES:
                                order.PaymentMethodId = (int)PosPaymentTypeEnum.PayLater;
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
                                break;

                            case REPLY_NO:
                                //Send Message Confirm Order
                                await MessageFoodOrderMoreProductsConfirmationRemixReplyNo(convo.WaId, order);
                                break;

                            default:
                                await MessageResponseUnexpected(convo.WaId);
                                break;
                        }
                        #endregion
                        break;

                    case (int)WatiFoodOrderStatusEnum.RecieveTimeConfirm:
                        #region Correct Time Confirm
                        switch (messageReply)
                        {
                            case REPLY_ASAP:
                                //Set Status - Checkout Holding Status
                                await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.ProductMoreCheckoutConfirm);

                                //Send Message Checkout Confirm
                                await MessageFoodOrderCheckOutConfirm(convo.WaId, order);
                                break;

                            case REPLY_LATER_TODAY:
                                //Set status to Later today confirm
                                await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.RecieveTimeLaterConfirm);

                                //Send Message Confirm Order
                                await MessageRecieveTimeLaterConfirmation(convo.WaId);
                                break;

                            case REPLY_BACK_TO_CART:
                                //Set status to Product More Checkout Confirm
                                await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.ProductMoreCheckoutConfirm);

                                //Send Message Product More Checkout Confirm
                                await MessageFoodOrderMoreProductsConfirmation(convo.WaId, product, order);
                                break;

                            default:
                                await MessageResponseUnexpected(convo.WaId);
                                break;
                        }
                        #endregion
                        break;

                    case (int)WatiFoodOrderStatusEnum.RecieveTimeLaterConfirm:
                        #region Recieve Time Later Confirm

                        var orderTime =  messageReply;
                        string hour;
                        string minute;
                        int hourInt = 0;
                        int minuteInt = 0;
                                               
                        try
                        {
                            #region Validate Hour portion of time supplied
                            hour = orderTime.Substring(0, 2); // hour part of orderTime
                            hourInt = Convert.ToInt32(hour);
                   
                            if (hourInt < 12)
                            {
                                await _wati.SessionMessageSend(convo.WaId, REPLY_INCORRECT_TIME_SUPPLIED);
                                await MessageRecieveTimeLaterConfirmation(convo.WaId);
                                break;
                            }
                            if (hourInt > 20)
                            {
                                await _wati.SessionMessageSend(convo.WaId, REPLY_INCORRECT_TIME_SUPPLIED);
                                await MessageRecieveTimeLaterConfirmation(convo.WaId);
                                break;
                            }
                            if (hourInt < DateTime.Now.Hour)
                            {
                                await _wati.SessionMessageSend(convo.WaId, REPLY_INCORRECT_TIME_SUPPLIED);
                                await MessageRecieveTimeLaterConfirmation(convo.WaId);
                                break;
                            }
                            #endregion

                            #region Validate Hour portion of time supplied                       
                            minute = orderTime.Substring(3, 2); ; // minute portion of oderTime
                            minuteInt = Convert.ToInt32(minute);

                            if (minuteInt < 0)
                            {
                                await _wati.SessionMessageSend(convo.WaId, REPLY_INCORRECT_TIME_SUPPLIED);
                                await MessageRecieveTimeLaterConfirmation(convo.WaId);
                                break;
                            }
                            if (minuteInt > 59)
                            {
                                await _wati.SessionMessageSend(convo.WaId, REPLY_INCORRECT_TIME_SUPPLIED);
                                await MessageRecieveTimeLaterConfirmation(convo.WaId);
                                break;
                            }
                            #endregion
                        }
                        catch
                        {
                            await _wati.SessionMessageSend(convo.WaId, REPLY_INCORRECT_TIME_SUPPLIED);
                            await MessageRecieveTimeLaterConfirmation(convo.WaId);
                            break;
                        }

                        //Once Confirmed:
                        order.EffectiveDate = DateTime.Today.AddHours(hourInt).AddMinutes(minuteInt);
                        _context.SaveChanges();

                        //Set Status Checkout Holding
                        await FoodOrderStatusSet(order.Id, WatiFoodOrderStatusEnum.ProductMoreCheckoutConfirm);

                        //Send Message Checkout Confirm (Later)
                        await MessageFoodOrderCheckOutConfirm(convo.WaId, order);                        
                        #endregion
                        break;

                    case (int)WatiFoodOrderStatusEnum.CancelConfirm:
                        #region CancelConfirm
                        switch (messageReply)
                        {
                            case REPLY_YES:
                                //Set Conversation to Cancelled
                                await ConversationStatusSet(convo.Id, WatiConversationStatusEnum.Cancelled);
                                convo.CompletedDate = DateTime.Now;

                                //Update Database
                                order.RejectDate = DateTime.Now;
                                order.RejectReason = CANCELLED_BY_USER;
                                _context.SaveChanges();

                                //Send Message Cancel Confirm
                                await _wati.SessionMessageSend(convo.WaId, $"You order with {tenant.Name} has been cancelled. We hope to chat with you again soon. Thank you!");
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
                        //switch (messageReply)
                        //{
                        //    case REPLY_PAYMENT_CASH:
                        //        order.PaymentMethodId = (int)PosPaymentTypeEnum.Cash;
                        //        break;

                        //    case REPLY_PAYMENT_CARD:
                        //        order.PaymentMethodId = (int)PosPaymentTypeEnum.Card;
                        //        break;

                        //    case REPLY_PAYMENT_EFT:
                        //        order.PaymentMethodId = (int)PosPaymentTypeEnum.EFT;
                        //        break;
                        //}
                        //_context.SaveChanges();


                        //var saveTotal = await FoodOrderTotalsGet(order.Id);
                        //var productList = await FoodOrderCartGet(order.Id);

                        //var result = await _pointOfSaleService.OrderSave(order.Id, saveTotal, productList);

                        //if (result.success)
                        //{
                        //    await MessageOrderSaved(convo.WaId, result.orderId);
                        //}
                        //else
                        //{
                        //    await _wati.SessionMessageSend(convo.WaId, $"Error saving order: {result.error}");
                        //}
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
        private async Task<AppWatiOrder> FoodOrderDetailCreate(int orderId, AppProduct product, int quantity, string? comment)
        {
            var order = _context.AppWatiOrders.Single(x => x.Id == orderId);
            var orderDetail = new AppWatiOrderDetail();

            orderDetail.WatiOrderId = orderId;
            orderDetail.BrandId = (int)order.CurrentBrand;
            orderDetail.ProductId = product.Id;
            orderDetail.Amount = product.Price;
            orderDetail.Name = product.Name;
            orderDetail.Quantity = quantity;
            orderDetail.Comment = comment;
            
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
            order.CurrentBrand = null;
            order.CurrentCategory = null;
            order.CurrentProduct = null;
            order.CurrentProductComment = null;
            order.CurrentProductExtra = null;
            order.CurrentExtraCompleted = null;
            order.CurrentExtraCount = null;
            order.CurrentExtraOptionMax = null;
            order.CurrentExtraOptionSelected = null;
            _context.SaveChanges();
            
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
            await MessageFoodOrderProductSelectionText(convo.WaId, category, (Guid)order.TenantId);

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
            welcome.Body += $"At any moment during this conversation you can type '{REPLY_RESTART}' to start over, or '{REPLY_HELP}' to speak to a member of our team.";
            
            welcome.Footer = "";
            welcome.ButtonText = "Choose Option";

            if (isOrderFood)
            {
                rowList.Add(new InteractivelistMessageSectionRow
                {
                    Title = REPLY_FOOD_ORDER,
                    Description = ""
                });
            }

            rowList.Add(new InteractivelistMessageSectionRow
            {
                Title = REPLY_CHAT_TO_HUMAN,
                Description = "Chat to a member our team"
            });

            rowList.Add(new InteractivelistMessageSectionRow
            {
                Title = REPLY_BECOME_A_VENDOR,
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

            welcome.Header = "Place Order";
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
        private async Task MessageFoodOrderVendorWelcome(string whatsappNumber, Guid tenantId, AppTenantInfo tenant, bool isDeliveryOption)
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
                Text = REPLY_PLACE_ORDER_PICKUP
            });

            if (tenant.IsWhatsAppDelivery && isDeliveryOption)
            {
                buttonList.Add(new InteractiveButtonMessageButton()
                {
                    Text = REPLY_PLACE_ORDER_DELIVERY
                });
            }

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
        private async Task MessageAddressRequired(string whatsappNumber)
        {            
            string message = "\r\n";
            message += "\r\n";
            message += $"We will now be prompting you for your address details:";
            message += "\r\n";
            message += "\r\n";
            message += $"Firstly, your street address.";
            message += "\r\n";
            message += $"Secondly, your suburb.";
            message += "\r\n";
            message += $"Finally, your post code.";
            message += "\r\n";
            message += "\r\n";

            await _wati.SessionMessageSend(whatsappNumber, message);
        }
        private async Task MessageAddressCorrectConfirmation(string whatsappNumber, AppWatiUser user)
        {
            var messageText = new InteractiveButtonsMessageTextDto();
            var headerText = new InteractiveButtonMessageHeaderText();
            var buttonList = new List<InteractiveButtonMessageButton>();
            
            #region Buttons
            buttonList.Add(new InteractiveButtonMessageButton() { Text = REPLY_YES });
            buttonList.Add(new InteractiveButtonMessageButton() { Text = REPLY_NO });            
            #endregion

            headerText.Type = "Text";
            headerText.Text = $"{user.Name}";

            messageText.Header = headerText;

            messageText.Body = $"We currently have your address saved as the following:";
            messageText.Body += "\r\n";
            messageText.Body += "\r\n";
            messageText.Body += $"{user.AddressStreet}";
            messageText.Body += "\r\n";
            messageText.Body += $"{user.AddressSuburb}";
            messageText.Body += "\r\n";
            messageText.Body += $"{user.AddressPostCode}";
            messageText.Body += "\r\n";
            messageText.Body += "\r\n";
            messageText.Body += "Is the above address correct?";

            messageText.Footer = "";

            messageText.Buttons = buttonList;

            var result = await _wati.InteractiveButtonsMessageTextSend(whatsappNumber, messageText);
        }
        private async Task MessageDeliveryCostConfirmation(string whatsappNumber, AppWatiUser user, AppTenantInfo tenant, decimal DeliveryFee)

        {
            var messageText = new InteractiveButtonsMessageTextDto();
            var headerText = new InteractiveButtonMessageHeaderText();
            var buttonList = new List<InteractiveButtonMessageButton>();

            #region Buttons
            buttonList.Add(new InteractiveButtonMessageButton() { Text = REPLY_YES });
            buttonList.Add(new InteractiveButtonMessageButton() { Text = REPLY_PLACE_ORDER_PICKUP});
            buttonList.Add(new InteractiveButtonMessageButton() { Text = REPLY_CANCEL_ORDER });
            #endregion

            headerText.Type = "Text";
            headerText.Text = $"Delivery to {user.AddressPostCode}";

            messageText.Header = headerText;
            messageText.Body = $"The Delivery cost is *R{DeliveryFee}*";
            messageText.Body += "\r\n";
            messageText.Body += "\r\n";
            messageText.Body += $"Would you like to continue?";
            messageText.Footer = "";

            messageText.Buttons = buttonList;

            var result = await _wati.InteractiveButtonsMessageTextSend(whatsappNumber, messageText);
        }
        private async Task MessageNoDeliveryPickupConfirmation(string whatsappNumber, AppWatiUser user, AppTenantInfo tenant)

        {
            var messageText = new InteractiveButtonsMessageTextDto();
            var headerText = new InteractiveButtonMessageHeaderText();
            var buttonList = new List<InteractiveButtonMessageButton>();

            #region Buttons            
            buttonList.Add(new InteractiveButtonMessageButton() { Text = REPLY_PLACE_ORDER_PICKUP });
            buttonList.Add(new InteractiveButtonMessageButton() { Text = REPLY_CANCEL_ORDER });
            #endregion

            headerText.Type = "Text";
            headerText.Text = $"Delivery unavailable for {user.AddressPostCode}";

            messageText.Header = headerText;
            messageText.Body = $"{tenant.Name} does not deliver to your post code.";
            messageText.Body += "\r\n";
            messageText.Body += "\r\n";
            messageText.Body += $"Would you like to place a pickup order instead?";
            messageText.Footer = "";

            messageText.Buttons = buttonList;

            var result = await _wati.InteractiveButtonsMessageTextSend(whatsappNumber, messageText);
        }
        private async Task MessageFoodOrderBrandSelection(string whatsappNumber, Guid tenantId, AppTenantInfo tenant)
        {
            var message = new InteractiveListMessageDto();
            var sectionList = new List<InteractiveListMessageSection>();
            var rowList = new List<InteractivelistMessageSectionRow>();
            var brandList = await _pointOfSaleService.BrandList(tenantId);

            message.Header = tenant.NameShort;

            if (tenant.LabelBrand.IsNullOrEmpty())
            {
                message.Body = "Please select from the following brands:";
                message.ButtonText = "Choose Brand";                
            }
            else
            {
                message.Body = $"Please select from the following {tenant.LabelBrandPlural.ToLower()}:";
                message.ButtonText = $"Choose {tenant.LabelBrand}";
            }

            message.Footer = "";
            
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
            var tenant = _context.AppTenantInfos.Single(x => x.TenantId == tenantId);

            message.Header = brand.NameShort.ToString();

            if (tenant.LabelCategory.IsNullOrEmpty())
            {
                message.Body = "Please select from the following categories:";
                message.ButtonText = "Choose Category";
            }
            else
            {
                message.Body = $"Please select from the following {tenant.LabelCategoryPlural.ToLower()}:";
                message.ButtonText = $"Choose {tenant.LabelCategory}";
            }
           
            message.Footer = "";            

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
                if (tenant.LabelBrand.IsNullOrEmpty())
                {
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
                else
                {
                    rowBackList.Add(new InteractivelistMessageSectionRow
                    {
                        Title = $"Go back to {tenant.LabelBrand} list",
                        Description = ""
                    });

                    sectionList.Add(new InteractiveListMessageSection
                    {
                        Title = $"{tenant.LabelBrandPlural}",
                        Rows = rowBackList
                    });
                }
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
            var tenant = _context.AppTenantInfos.Single(x => x.TenantId == tenantId);

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
                if (tenant.LabelBrand.IsNullOrEmpty())
                {
                    message += "Brands:";
                    message += "\r\n";
                    message += $"0 {REPLY_BACK_BRAND}";
                }
                else
                {
                    message += $"{tenant.LabelBrandPlural}:";
                    message += "\r\n";
                    message += $"0 Go back to {tenant.LabelBrand}";
                }                
            }

            message += "\r\n";
            message += "\r\n";

            if (tenant.LabelCategoryPlural.IsNullOrEmpty())            
                message += "Categories:";
            else
                message += $"{tenant.LabelCategoryPlural}:";

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
        private async Task MessageFoodOrderProductSelectionText(string whatsappNumber, AppProductCategory category, Guid tenantId)
        {
            string message = "";
            var productList = await _pointOfSaleService.ProductList(category.Id);
            var tenant = _context.AppTenantInfos.Single(x => x.TenantId == tenantId);

            message = category.Name.Replace('&', '_').ToUpper();
            message += "\r\n";
            message += "\r\n";
            message += "Please enter the number on the left hand side of your selection.";
            message += "\r\n";
            message += "\r\n";

            if (tenant.LabelCategory.IsNullOrEmpty())
            {
                message += "Categories:";
                message += "\r\n";
                message += $"0 {REPLY_BACK_CATEGORY}";
            }
            else
            {
                message += $"{tenant.LabelCategoryPlural}:";
                message += "\r\n";
                message += $"0 Go back to {tenant.LabelCategory} list";
            }

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
        private async Task MessageFoodOrderProductCommentQuestion(string whatsappNumber, AppProduct product)
        {
            var messageText = new InteractiveButtonsMessageTextDto();
            var headerText = new InteractiveButtonMessageHeaderText();
            var buttonList = new List<InteractiveButtonMessageButton>();

            #region Buttons
            buttonList.Add(new InteractiveButtonMessageButton() { Text = REPLY_YES });
            buttonList.Add(new InteractiveButtonMessageButton() { Text = REPLY_NO });
            #endregion

            headerText.Type = "Text";
            headerText.Text = $"{product.Name}";

            messageText.Header = headerText;

            messageText.Body += $"Do you want to add a special instruction?";

            messageText.Footer = "";

            messageText.Buttons = buttonList;

            var result = await _wati.InteractiveButtonsMessageTextSend(whatsappNumber, messageText);
        }
        private async Task MessageFoodOrderProductCommentConfirm(string whatsappNumber)
        {            
            var result = await _wati.SessionMessageSend(whatsappNumber, "Please submit your special instruction:");            
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
            rowQuantityList.Add(new InteractivelistMessageSectionRow { Title = "7", Description = "" });
            rowQuantityList.Add(new InteractivelistMessageSectionRow { Title = "8", Description = "" });
            rowQuantityList.Add(new InteractivelistMessageSectionRow { Title = "9", Description = "" });
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

            var cartList = await FoodOrderCartGet(order.Id);
            var cartTotal = await FoodOrderTotalsGet(order.Id);

            #region Buttons
            buttonList.Add(new InteractiveButtonMessageButton() { Text = REPLY_CHECKOUT });
            buttonList.Add(new InteractiveButtonMessageButton() { Text = REPLY_ADD_MORE_PRODUCTS });            
            buttonList.Add(new InteractiveButtonMessageButton() { Text = REPLY_CANCEL_ORDER });
            #endregion

            headerText.Type = "Text";
            headerText.Text = $"{product.Name}";

            messageText.Header = headerText;

            messageText.Body = $"Your product has been added to cart. Your cart value is R{cartTotal.ToString("F2")}:";
            messageText.Body += "\r\n";
            messageText.Body += "\r\n";            

            foreach (var cartProduct in cartList)
            {
                messageText.Body += $"{cartProduct.Quantity} X {cartProduct.Name}";
                messageText.Body += "\r\n";

                var productExtraOptionList = (from Option in _context.AppWatiOrderDetailOptions
                                              where Option.WatiOrderDetailId == cartProduct.Id
                                              select Option).ToList();

                foreach (var extraOption in productExtraOptionList)
                {                    
                    messageText.Body += $"    {extraOption.Name}";
                    messageText.Body += "\r\n";
                }

                messageText.Body += $"    R{cartProduct.Quantity * cartProduct.Amount}";
                messageText.Body += "\r\n";
                messageText.Body += "\r\n";
            }

            if (order.OrderFulfillmentId == (int)PosFulfillmentTypeEnum.Delivery)
            {
                messageText.Body += $"Delivery Fee: R{order.DeliveryFee}";
                messageText.Body += "\r\n";
                messageText.Body += "\r\n";

                messageText.Body += $"Order Total: R{cartTotal + ((order.DeliveryFee == null) ? 0 : (decimal)order.DeliveryFee)}";
                messageText.Body += "\r\n";
                messageText.Body += "\r\n";
            }

            
            messageText.Body += "Would you like to Check Out?";

            messageText.Footer = "";
                
            messageText.Buttons = buttonList;

            var result = await _wati.InteractiveButtonsMessageTextSend(whatsappNumber, messageText);
        }
        private async Task MessageRecieveTimeConfirmation(string whatsappNumber)
        {
            var messageText = new InteractiveButtonsMessageTextDto();
            var headerText = new InteractiveButtonMessageHeaderText();
            var buttonList = new List<InteractiveButtonMessageButton>();

            #region Buttons
            buttonList.Add(new InteractiveButtonMessageButton() { Text = REPLY_ASAP });
            
            buttonList.Add(new InteractiveButtonMessageButton() { Text = REPLY_LATER_TODAY });

            buttonList.Add(new InteractiveButtonMessageButton() { Text = REPLY_BACK_TO_CART });
            #endregion

            headerText.Type = "Text";
            headerText.Text = $"Confirm Order Time";


            messageText.Header = headerText;

            messageText.Body = $"For when would you like to place your order?";
            messageText.Body += "\r\n";
            messageText.Body += "\r\n";
            messageText.Footer = "";

            messageText.Buttons = buttonList;

            var result = await _wati.InteractiveButtonsMessageTextSend(whatsappNumber, messageText);
        }        
        private async Task MessageRecieveTimeLaterConfirmation(string whatsappNumber)
        {
            string message = "\r\n";
           
            message = $"Please enter your order time using the 24-hour format: HH:MM";
            message += "\r\n";
            message += "\r\n";
            message += $"Example, for 5:30PM enter the following:";
            message += "\r\n";
            message += $"17:30";
            message += "\r\n";
            message += "\r\n";
            message += $"Choose a time up until 20:00";
                
            await _wati.SessionMessageSend(whatsappNumber, message);
        }
        private async Task MessageFoodOrderMoreProductsConfirmationRemixReplyNo(string whatsappNumber, AppWatiOrder order)
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
            headerText.Text = $"Current Order";

            messageText.Header = headerText;

            messageText.Body = $"Your cart value is R{cartTotal.ToString("F2")}:";
            messageText.Body += "\r\n";
            messageText.Body += "\r\n";

            foreach (var cartProduct in cartList)
            {
                messageText.Body += $"{cartProduct.Quantity} X {cartProduct.Name}";
                messageText.Body += "\r\n";

                var productExtraOptionList = (from Option in _context.AppWatiOrderDetailOptions
                                              where Option.WatiOrderDetailId == cartProduct.Id
                                              select Option).ToList();

                foreach (var extraOption in productExtraOptionList)
                {
                    messageText.Body += $"    {extraOption.Name}";
                    messageText.Body += "\r\n";
                }

                messageText.Body += $"    R{cartProduct.Quantity * cartProduct.Amount}";
                messageText.Body += "\r\n";
                messageText.Body += "\r\n";
            }

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
        private async Task MessageFoodOrderCheckOutConfirm(string whatsappNumber, AppWatiOrder order)
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

            if (order.EffectiveDate != null)
            {
                messageText.Body = "Order time:";
                messageText.Body += "\r\n";
                messageText.Body += $"{Convert.ToDateTime(order.EffectiveDate).TimeOfDay.ToString(@"hh\:mm")}";
                messageText.Body += "\r\n";
                messageText.Body += "\r\n";
            }

            messageText.Body += "Are you sure you would like to check out?";
            
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
