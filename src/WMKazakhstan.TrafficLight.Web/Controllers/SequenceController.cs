using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using WMKazakhstan.TrafficLight.Core.Interfaces;
using WMKazakhstan.TrafficLight.Infrastructure;
using WMKazakhstan.TrafficLight.Web.Models;

namespace WMKazakhstan.TrafficLight.Web.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class SequenceController : ControllerBase
    {
        private readonly ITrafficLightService trafficLightService;

        public SequenceController(ITrafficLightService trafficLightService)
        {
            this.trafficLightService = trafficLightService;
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            var sequence = await trafficLightService.CreateSequenceAsync();

            return Ok(new CreatedSequence(sequence));
        }
    }
}
