using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using WMKazakhstan.TrafficLight.Core.Interfaces;
using WMKazakhstan.TrafficLight.Infrastructure;
using WMKazakhstan.TrafficLight.Web.Models;

namespace WMKazakhstan.TrafficLight.Web.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ObservationController : ControllerBase
    {
        private readonly ITrafficLightService trafficLightService;

        public ObservationController(ITrafficLightService trafficLightService)
        {
            this.trafficLightService = trafficLightService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddObservation request)
        {
            var observation = request.Observation;

            var trafficLight = CreateTrafficLight(observation);

            var prediction = await trafficLightService.PredictAsync(request.Sequence.Value, trafficLight);

            return Ok(new GetPrediction(prediction));
        }

        private Core.Models.TrafficLight CreateTrafficLight(Observation observation)
        {
            if (observation.Color == Core.Models.Color.Red)
                return new Core.Models.TrafficLight(Core.Models.Color.Red);

            return new Core.Models.TrafficLight(
                observation.Color.Value,
                new Core.Models.TrafficLightDigits(observation.Numbers[0], observation.Numbers[1]));
        }
    }
}
