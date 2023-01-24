using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pekkish.PointOfSale.Wati.Models.Dtos
{
    public class InteractiveButtonsMessageTextDto
    {
        public InteractiveButtonMessageHeaderText Header { get; set; } = new InteractiveButtonMessageHeaderText();
        public string Body { get; set; } = string.Empty;
        public string Footer { get; set; } = string.Empty;
        public List<InteractiveButtonMessageButton> Buttons { get; set; } = new List<InteractiveButtonMessageButton>();
    }
    public class InteractiveButtonsMessageMediaDto
    {
        public InteractiveButtonMessageHeaderMedia Header { get; set; } = new InteractiveButtonMessageHeaderMedia();
        public string Body { get; set; } = string.Empty;
        public string Footer { get; set; } = string.Empty;
        public List<InteractiveButtonMessageButton> Buttons { get; set; } = new List<InteractiveButtonMessageButton>();
    }

    public class InteractiveButtonMessageHeaderText
    {
        public string Type { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;        
    }
    public class InteractiveButtonMessageHeaderMedia
    {
        public string Type { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty; //Test for removal
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
