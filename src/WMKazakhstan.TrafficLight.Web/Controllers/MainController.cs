using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using WMKazakhstan.TrafficLight.Core.Interfaces;
using WMKazakhstan.TrafficLight.Infrastructure;
using WMKazakhstan.TrafficLight.Web.Models;

namespace WMKazakhstan.TrafficLight.Web.Controllers
{
    [ApiController]
    [Route("[action]")]
    public class MainController : ControllerBase
    {
        private readonly IStorage storage;

        public MainController(IStorage storage)
        {
            this.storage = storage;
        }

        [HttpGet]
        public async Task<IActionResult> Clear()
        {
            await storage.ClearAsync();

            return Ok(new { Status = Status.Ok });
        }
    }
}
