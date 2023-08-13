namespace Pekkish.PointOfSale.Api.Models.PointOfSale
{
    public class PrintEntry
    {
        public string type { get; set; }
        public string content { get; set; } = string.Empty;
        public int bold { get; set; }
        public int align { get; set; }
        public int format { get; set; }
        //public string path { get; set; } = string.Empty;
        //public int width { get; set; }
        //public int height { get; set; }
        //public int size { get; set; }
    }
}
