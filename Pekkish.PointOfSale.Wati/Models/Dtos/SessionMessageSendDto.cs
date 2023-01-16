using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pekkish.PointOfSale.Wati.Models.Dtos
{
    public class SessionMessageSendDto
    {
        public string WhatsAppNumber { get; set; }
        public string MessageText { get; set; }

        public SessionMessageSendDto()
        {
            WhatsAppNumber = string.Empty;
            MessageText = string.Empty;
        }
    }
}
