using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

using WMKazakhstan.TrafficLight.Core.Models;
using WMKazakhstan.TrafficLight.Core.Services;
using WMKazakhstan.TrafficLight.Core.Exceptions;

using Light = WMKazakhstan.TrafficLight.Core.Models.TrafficLight;

namespace WMKazakhstan.TrafficLight.UnitTests.Services
{
    public class TrafficLightTimePredictorTests
    {
        [Fact]
        public void StartWith05_WhenSection2Section5AreBrokenInLow_ShouldReturn5()
        {
            var lights = new []
            {
                new Light(Color.Green, new TrafficLightDigits(new Digit(0), new Digit("1101001"))),
                new Light(Color.Green, new TrafficLightDigits(new Digit(0), new Digit("0101000"))),
                new Light(Color.Green, new TrafficLightDigits(new Digit(0), new Digit("1001001"))),
                new Light(Color.Green, new TrafficLightDigits(new Digit(0), new Digit("1001101"))),
                new Light(Color.Green, new TrafficLightDigits(new Digit(0), new Digit("0000000"))),
                new Light(Color.Red)
            };

            var prediction = Test(lights, new List<Observation>());

            Assert.Single(prediction);
            Assert.Equal(new TrafficLightDigits(5), prediction[0]);
        }

        [Fact]
        public void StartWith95_WhenSection3IsBrokenInHigh_ShouldReturn95()
        {
            var lights = new[]
            {
                new Light(Color.Green, new TrafficLightDigits(new Digit("1110011"), new Digit(5))),
                new Light(Color.Green, new TrafficLightDigits(new Digit("1110011"), new Digit(4))),
                new Light(Color.Green, new TrafficLightDigits(new Digit("1110011"), new Digit(3))),
                new Light(Color.Green, new TrafficLightDigits(new Digit("1110011"), new Digit(2))),
                new Light(Color.Green, new TrafficLightDigits(new Digit("1110011"), new Digit(1))),
                new Light(Color.Green, new TrafficLightDigits(new Digit("1110011"), new Digit(0))),
                new Light(Color.Green, new TrafficLightDigits(new Digit("1110111"), new Digit(9)))
            };

            var prediction = Test(lights, new List<Observation>());

            Assert.Single(prediction);
            Assert.Equal(new TrafficLightDigits(95), prediction[0]);
        }

        [Fact]
        public void StartWith80_WhenNoBrokenSections_ShouldReturn80()
        {
            var lights = new[]
            {
                new Light(Color.Green, new TrafficLightDigits(80)),
                new Light(Color.Green, new TrafficLightDigits(79))
            };

            var prediction = Test(lights, new List<Observation>());

            Assert.Single(prediction);
            Assert.Equal(new TrafficLightDigits(80), prediction[0]);
        }

        [Fact]
        public void StartWith02_WhenNoBrokenSections_ShouldReturn02()
        {
            var lights = new[]
            {
                new Light(Color.Green, new TrafficLightDigits(02)),
                new Light(Color.Green, new TrafficLightDigits(01)),
                new Light(Color.Red)
            };

            var prediction = Test(lights, new List<Observation>());

            Assert.Single(prediction);
            Assert.Equal(new TrafficLightDigits(02), prediction[0]);
        }

        [Theory]
        [InlineData(new[] { 5, 3, 2, 1 })]
        [InlineData(new[] { 88, 87, 86, 84 })]
        public void Predict_WhenMissingObservation_ShouldThrow(int[] numbers)
        {
            var lights = numbers
                .Select(x => new Light(Color.Green, new TrafficLightDigits(x))).ToArray();

            Assert.Throws<CoreException>(() => Test(lights, new List<Observation>()));
        }

        [Fact]
        public void Predict_WhenFirstObservationIsRed_ShouldThrow()
        {
            var lights = new[]
            {
                new Light(Color.Red)
            };

            Assert.Throws<CoreException>(() => Test(lights, new List<Observation>()));
        }

        [Fact]
        public void Predict_WhenSendOnservationAfterRed_ShouldThrow()
        {
            var lights = new[]
            {
                new Light(Color.Green, new TrafficLightDigits(02)),
                new Light(Color.Green, new TrafficLightDigits(01)),
                new Light(Color.Red),
                new Light(Color.Green, new TrafficLightDigits(01))
            };

            Assert.Throws<CoreException>(() => Test(lights, new List<Observation>()));
        }

        private TrafficLightDigits[] Test(Light[] lights, List<Observation> storage)
        {
            var predictor = new TrafficLightTimePredictor(new DigitPredictor(new BrokenSectionsExpert()));

            var sequence = Guid.NewGuid();

            TrafficLightDigits[] prediction = null;

            foreach (var light in lights)
            {
                prediction = predictor.Predict(light, storage.ToArray());

                var count = storage.Count;

                var observation = new Observation(sequence, light, prediction.Select(x => x - count).ToArray());

                storage.Add(observation);
            }

            return prediction;
        }
    }
}
