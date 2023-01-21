using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pekkish.PointOfSale.Wati.Models.Dtos
{
    public class InteractiveButtonsMessageDto
    {
        public InteractiveButtonMessageHeader Header { get; set; } = new InteractiveButtonMessageHeader();
        public string Body { get; set; } = string.Empty;
        public string Footer { get; set; } = string.Empty;
        public InteractiveButtonMessageButton Buttons { get; set; } = new InteractiveButtonMessageButton();
    }

    public class InteractiveButtonMessageHeader
    {
        public string Type { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public InteractiveButtonHeaderMedia Media { get; set; } = new InteractiveButtonHeaderMedia();
    }

    public class InteractiveButtonHeaderMedia
    {
        public string Url { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
    }
    public class InteractiveButtonMessageButton
    { 
        public string Text { get; set; } = string.Empty;
    }
}
