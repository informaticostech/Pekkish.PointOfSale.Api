using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pekkish.PointOfSale.Wati.Models.Dtos
{
    public class TemplateMessageSendDto
    {
        public string Template_Name { get; set; } = string.Empty;
        public string Broadcast_Name {get;set;} = string.Empty;
        public List<TemplateMessageParameter> Parameters { get; set; } = new List<TemplateMessageParameter>();
    }

    public class TemplateMessageParameter
    { 
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
