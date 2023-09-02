using Microsoft.AspNetCore.Mvc;
using Pekkish.PointOfSale.Api.Models.PointOfSale;
using Pekkish.PointOfSale.Wati.Models.Dtos;
using System.Text.RegularExpressions;

namespace Pekkish.PointOfSale.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrintController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("PrintString")]
        public async Task<IActionResult> PrintString()
        {
            return await Task.Run(() =>
            {
                //Dictionary<int, PrintEntry> entries = new Dictionary<int, PrintEntry>();

                //// Sending text entry
                //PrintEntry obj1 = new PrintEntry
                //{
                //    type = "0", // text
                //    content = "Pekkish POS", // any string
                //    bold = 1, // 0 if no, 1 if yes
                //    align = 2, // 0 if left, 1 if center, 2 if right
                //    format = 3 // 0 if normal, 1 if double Height, 2 if double Height + Width, 3 if double Width, 4 if small
                //};
                //entries.Add(0, obj1);

                //PrintEntry obj2 = new PrintEntry
                //{
                //    type = "0", // text
                //    content = "HO$H", // any string
                //    bold = 1, // 0 if no, 1 if yes
                //    align = 2, // 0 if left, 1 if center, 2 if right
                //    format = 3 // 0 if normal, 1 if double Height, 2 if double Height + Width, 3 if double Width, 4 if small
                //};
                //entries.Add(1, obj2);


                //string json = Newtonsoft.Json.JsonConvert.SerializeObject(entries);

                //json = json.Trim('[', ']');

                string json = "{\"0\":{\"type\":\"0\",\"content\":\"TEST\",\"bold\":1,\"align\":1,\"format\":3},\"1\":{\"type\":\"0\",\"content\":\"123456\",\"bold\":1,\"align\":1,\"format\":3},\"2\":{\"type\":\"0\",\"content\":\"Order Number: 18291\",\"bold\":1,\"align\":1,\"format\":3},\"3\":{\"type\":\"0\",\"content\":\"Date: 14/08/2023 23:14\",\"bold\":1,\"align\":1,\"format\":3},\"4\":{\"type\":\"0\",\"content\":\"Seating: Table 2\",\"bold\":1,\"align\":1,\"format\":3},\"5\":{\"type\":\"0\",\"content\":\".\",\"bold\":1,\"align\":1,\"format\":3},\"6\":{\"type\":\"0\",\"content\":\"1 X Choc Chip Cookie  -  R10.00\",\"bold\":1,\"align\":1,\"format\":3},\"7\":{\"type\":\"0\",\"content\":\".\",\"bold\":1,\"align\":1,\"format\":3},\"8\":{\"type\":\"0\",\"content\":\"Order Sub Total: R10.00\",\"bold\":1,\"align\":1,\"format\":3},\"9\":{\"type\":\"0\",\"content\":\"Order Total: R10.00\",\"bold\":1,\"align\":1,\"format\":3},\"10\":{\"type\":\"0\",\"content\":\"VAT: 1.30\",\"bold\":1,\"align\":1,\"format\":3},\"11\":{\"type\":\"0\",\"content\":\"Paid Card: R10.00\",\"bold\":1,\"align\":1,\"format\":3},\"12\":{\"type\":\"0\",\"content\":\"Paid Cash: R10.00\",\"bold\":1,\"align\":1,\"format\":3},\"13\":{\"type\":\"0\",\"content\":\"Paid Total : R20.00\",\"bold\":1,\"align\":1,\"format\":3},\"14\":{\"type\":\"0\",\"content\":\".\",\"bold\":1,\"align\":1,\"format\":3},\"15\":{\"type\":\"0\",\"content\":\"Change : R10.00\",\"bold\":1,\"align\":1,\"format\":3},\"16\":{\"type\":\"0\",\"content\":\".\",\"bold\":1,\"align\":1,\"format\":3},\"17\":{\"type\":\"0\",\"content\":\"Powered by PekkishPOS\",\"bold\":1,\"align\":1,\"format\":3},\"18\":{\"type\":\"0\",\"content\":\"business.pekkish.co.za\",\"bold\":1,\"align\":1,\"format\":3},\"19\":{\"type\":\"0\",\"content\":\"business.pekkish.co.za\",\"bold\":1,\"align\":1,\"format\":3}}";


                return Content(json, "application/json");
            });            
        }
    }
}
