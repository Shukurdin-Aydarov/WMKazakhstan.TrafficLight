using System;
using System.Linq;
using Xunit;

using WMKazakhstan.TrafficLight.Core;
using WMKazakhstan.TrafficLight.Core.Models;
using WMKazakhstan.TrafficLight.Core.Services;

using Light = WMKazakhstan.TrafficLight.Core.Models.TrafficLight;

namespace WMKazakhstan.TrafficLight.UnitTests.Services
{
    public class DigitPredictorTests
    {
        [Fact]
        public void PredictDigits_WithoutPreviousObservations()
        {
            var predictor = new DigitPredictor(new BrokenSectionsExpert());

            var trafficLight = new Light(Color.Green, new TrafficLightDigits(Constants.Zero, new Digit("0011101")));

            var result = predictor
                .Predict(trafficLight, new Observation[0])
                .ToArray();

            Assert.Contains(new TrafficLightDigits(02), result);
            Assert.Contains(new TrafficLightDigits(08), result);
            Assert.Contains(new TrafficLightDigits(82), result);
            Assert.Contains(new TrafficLightDigits(88), result);
        }

        [Fact]
        public void PredictDigits_WhenAllSectionsAreServiceable_ShouldReturnSingle()
        {
            var predictor = new DigitPredictor(new BrokenSectionsExpert());

            var trafficLight = new Light(Color.Green, new TrafficLightDigits(Constants.Zero, new Digit(02)));

            var observation = new Observation(Guid.NewGuid(), new Light(Color.Green, new TrafficLightDigits(88)), null);

            var result = predictor
                .Predict(trafficLight, new[] { observation })
                .ToArray();

            Assert.Single(result);
            Assert.Equal(new TrafficLightDigits(02), result[0]);
        }
    }
}
