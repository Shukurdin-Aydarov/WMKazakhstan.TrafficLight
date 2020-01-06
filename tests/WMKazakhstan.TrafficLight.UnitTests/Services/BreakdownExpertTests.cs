using System;
using Xunit;

using WMKazakhstan.TrafficLight.Core;
using WMKazakhstan.TrafficLight.Core.Models;
using WMKazakhstan.TrafficLight.Core.Services;

using Light = WMKazakhstan.TrafficLight.Core.Models.TrafficLight;

namespace WMKazakhstan.TrafficLight.UnitTests.Services
{
    public class BreakdownExpertTests
    {
        [Fact]
        public void IdentifyBrokenSections_WhenSection5IsBrokenInLow_ShouldBeSection5IsBroken()
        {
            var predictor = new BrokenSectionsExpert();

            var trafficLight = new Light(Color.Green, new TrafficLightDigits(Constants.Zero, new Digit("1101001")));

            var predictedDigits = new[]
            {
                new TrafficLightDigits(5),
                new TrafficLightDigits(6),
                new TrafficLightDigits(8),
                new TrafficLightDigits(9),
                new TrafficLightDigits(85),
                new TrafficLightDigits(86),
                new TrafficLightDigits(88),
                new TrafficLightDigits(89)
            };

            var brokenSections = predictor.IdentifyBrokenSections(trafficLight, predictedDigits, new Observation[0]);

            Assert.Equal(new TrafficLightDigits(new Digit(), new Digit("0000010")), brokenSections);
        }

        [Fact]
        public void IdentifyBrokenSections_WithPreviousObservations()
        {
            var predictor = new BrokenSectionsExpert();

            var light = new Light(Color.Green, new TrafficLightDigits(Constants.Zero, new Digit("1001101")));

            var predictedDigits = new[]
            {
                new TrafficLightDigits(5),
                new TrafficLightDigits(85),
            };

            var observations = new[]
            {
                new Observation(Guid.NewGuid(), new Light(Color.Green, new TrafficLightDigits(new Digit(0), new Digit("1101001"))), null),
                new Observation(Guid.NewGuid(), new Light(Color.Green, new TrafficLightDigits(new Digit(0), new Digit("0101000"))), null),
                new Observation(Guid.NewGuid(), new Light(Color.Green, new TrafficLightDigits(new Digit(0), new Digit("1001001"))), null),
            };

            var brokenSections = predictor.IdentifyBrokenSections(light, predictedDigits, observations);

            Assert.Equal(new TrafficLightDigits(new Digit(), new Digit("0010010")), brokenSections);
        }
    }
}
