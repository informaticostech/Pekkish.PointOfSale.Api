using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pekkish.PointOfSale.Wati.Models.Dtos
{
    public class InteractiveListMessageDto
    {
        [StringLength(50)]
        public string Header { get; set; } = string.Empty;
        [StringLength(1024)]
        public string Body { get; set; } = string.Empty;
        [StringLength(60)]
        public string Footer { get; set; } = string.Empty;
        public string ButtonText { get; set; } = string.Empty;
        public List<InteractiveListMessageSection> Sections = new List<InteractiveListMessageSection>();
    }

    public class InteractiveListMessageSection
    {
        [StringLength(24)]
        public string Title { get; set; } = string.Empty;
        public List<InteractivelistMessageSectionRow> Rows { get; set; } = new List<InteractivelistMessageSectionRow>();
    }

    public class InteractivelistMessageSectionRow
    {
        [StringLength(24)]
        public string Title { get; set; } = string.Empty;
        [StringLength(72)]
        public string Description { get; set; } = string.Empty;
    }
}
