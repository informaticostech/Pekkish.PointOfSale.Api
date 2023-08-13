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
                Dictionary<int, PrintEntry> entries = new Dictionary<int, PrintEntry>();
                
                // Sending text entry
                PrintEntry obj1 = new PrintEntry
                {
                    type = "0", // text
                    content = "Pekkish POS", // any string
                    bold = 1, // 0 if no, 1 if yes
                    align = 2, // 0 if left, 1 if center, 2 if right
                    format = 3 // 0 if normal, 1 if double Height, 2 if double Height + Width, 3 if double Width, 4 if small
                };
                entries.Add(0, obj1);

                PrintEntry obj2 = new PrintEntry
                {
                    type = "0", // text
                    content = "HO$H", // any string
                    bold = 1, // 0 if no, 1 if yes
                    align = 2, // 0 if left, 1 if center, 2 if right
                    format = 3 // 0 if normal, 1 if double Height, 2 if double Height + Width, 3 if double Width, 4 if small
                };
                entries.Add(1, obj2);


                string json = Newtonsoft.Json.JsonConvert.SerializeObject(entries);

                json = json.Trim('[', ']');
                
                return Content(json, "application/json");
            });            
        }
    }
}
