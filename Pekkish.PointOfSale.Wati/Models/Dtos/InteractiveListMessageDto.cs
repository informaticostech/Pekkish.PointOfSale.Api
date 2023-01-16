using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pekkish.PointOfSale.Wati.Models.Dtos
{
    public class InteractiveListMessageDto
    {
        public string Header { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string Footer { get; set; } = string.Empty;
        public string ButtonText { get; set; } = string.Empty;
        public List<InteractiveListMessageSection> Sections = new List<InteractiveListMessageSection>();
    }

    public class InteractiveListMessageSection
    {
        public string Title { get; set; } = string.Empty;
        public List<InteractivelistMessageSectionRow> Rows { get; set; } = new List<InteractivelistMessageSectionRow>();
    }

    public class InteractivelistMessageSectionRow
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
