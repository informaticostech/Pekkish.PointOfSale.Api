using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pekkish.PointOfSale.Api.Models.Wati
{
    public class Constants
    {
        public const string MESSAGE_TYPE_INTERACTIVE = "interactive";
        public const string MESSAGE_TYPE_TEXT = "text";
        public const string CANCELLED_BY_USER = "Cancelled by User";
        public const string REPLY_FOOD_ORDER = "Order Food";
        public const string REPLY_CHAT_TO_HUMAN = "Chat to a human";
        public const string REPLY_BECOME_A_VENDOR = "Become a food vendor";
        public const string REPLY_VENDOR_ORDER_FOOD = "Place Order";
        public const string REPLY_ADD_TO_CART = "Add to Cart";
        public const string REPLY_CANCEL = "Cancel";
        public const string REPLY_BACK_VENDOR = "Go back to Vendor";
        public const string REPLY_BACK_BRAND = "Go back to Brand list";
        public const string REPLY_BACK_CATEGORY = "Go back to Category list";
        public const string REPLY_ADD_MORE_PRODUCTS = "Add Another Product";
        public const string REPLY_VIEW_CART = "View Cart";
        public const string REPLY_CHECKOUT = "Check Out";
        public const string REPLY_CANCEL_ORDER = "Cancel Order";
        public const string REPLY_YES = "Yes";
        public const string REPLY_NO = "No";
        public const string REPLY_PAYMENT_CASH = "Cash";
        public const string REPLY_PAYMENT_CARD = "Card";
        public const string REPLY_PAYMENT_EFT = "EFT";
        public const string REPLY_HELP = "help";
        public const string REPLY_RESTART = "restart";
        public const string REPLY_EXTRA_NONE = "None / Nothing";
        //public const string REPLY_EXTRA_NONE_MORE = "None / Nothing More";

        public enum WatiConversationStatusEnum { Initialised = 1, InProgress, Completed, Expired, Cancelled }
        public enum WatiConversationTypeEnum { FoodOrder = 1, ChatToHuman, BecomeVendor }
        public enum WatiFoodOrderStatusEnum { VendorSelection = 1, CategorySelection, ProductSelection, ProductAddToCardConfirm, ProductExtraSelection, 
            ProductMoreCheckoutConfirm, VendorLanding, OrderDateConfirm, OrderFulfillmentConfirm, OrderPayMethodConfirm, OrderSeastingAreaConfirm, BrandSelection, 
            CategorySelectionText, ProductSelectionText, QuantityConfirm, CancelConfirm, Cancelled, Completed, ProductExtraSelectionMoreConfirm, ProductCommentQuestion, 
            ProductCommentConfirm }

        public enum PosOrderStatusEnum
        {
            Pending = 1, PartiallyAccepted, Accepted, ReadyForPickup, Rejected, CancelledByBusiness,
            CancelledByCustomer, Completed
        }
        public enum PosPaymentTypeEnum
        {
            Cash = 1, Card, Online, EFT, Other, Multiple, PayLater
        }
        public enum PosSalesChannelEnum
        {
            Pekkish = 1, InStore, WhatsApp, UberEats, MrDFood, BoltFood, Instagram, PhoneCall,
            Facebook, TikTok, WhatsAppPekkish
        }
        public enum PosFulfillmentTypeEnum
        {
            Pickup = 1, Delivery, SitDown
        }
    }
}
