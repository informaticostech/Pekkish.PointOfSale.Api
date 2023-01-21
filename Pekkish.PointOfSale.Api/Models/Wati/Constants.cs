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
        public const string REPLY_FOOD_ORDER = "Order Food";
        public const string REPLY_CHAT_TO_HUMAN = "Chat to a human";
        public const string REPLY_BECOME_A_VENDOR = "Become a food vendor";

        public enum WatiConversationStatusEnum { Initialised = 1, InProgress, Completed, Expired }
        public enum WatiConversationTypeEnum { FoodOrder = 1, ChatToHuman, BecomeVendor }
    }
}
