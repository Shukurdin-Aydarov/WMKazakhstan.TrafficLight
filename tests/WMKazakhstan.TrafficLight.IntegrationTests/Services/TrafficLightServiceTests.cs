using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;

using WMKazakhstan.TrafficLight.Core.Interfaces;
using WMKazakhstan.TrafficLight.Core.Models;
using WMKazakhstan.TrafficLight.Core.Services;
using WMKazakhstan.TrafficLight.Infrastructure;
using WMKazakhstan.TrafficLight.Infrastructure.DataStorage;
using WMKazakhstan.TrafficLight.Core.Exceptions;

using Light = WMKazakhstan.TrafficLight.Core.Models.TrafficLight;

namespace WMKazakhstan.TrafficLight.IntegrationTests.Services
{
    public class TrafficLightServiceTests
    {
        private readonly ITrafficLightService service = CreateService();

        [Fact]
        public async ValueTask Predict_StartWith05_SecondAndFifthSectionsAreBrokenInSecondDigit()
        {
            var sequence = await service.CreateSequenceAsync();

            // 05
            var prediction = await service.PredictAsync(sequence, 
                new Light(Color.Green, new TrafficLightDigits(new Digit(0), new Digit("1101001"))));

            Assert.Equal(new int[] { 5, 6, 8, 9, 85, 86, 88, 89 }, prediction.Start);
            Assert.Equal(new[] { "0000000", "0000010" }, prediction.Missing);

            //04
            prediction = await service.PredictAsync(sequence,
                new Light(Color.Green, new TrafficLightDigits(new Digit(0), new Digit("0101000"))));

            Assert.Equal(new int[] { 5, 6, 9, 85, 86, 89 }, prediction.Start);
            Assert.Equal(new[] { "0000000", "0000010" }, prediction.Missing);

            //03
            prediction = await service.PredictAsync(sequence,
                new Light(Color.Green, new TrafficLightDigits(new Digit(0), new Digit("1001001"))));

            Assert.Equal(new int[] { 5, 85 }, prediction.Start);
            Assert.Equal(new[] { "0000000", "0010010" }, prediction.Missing);

            //02
            prediction = await service.PredictAsync(sequence,
                new Light(Color.Green, new TrafficLightDigits(new Digit(0), new Digit("1001101"))));

            Assert.Equal(new int[] { 5, 85 }, prediction.Start);
            Assert.Equal(new[] { "0000000", "0010010" }, prediction.Missing);

            //01
            prediction = await service.PredictAsync(sequence,
                new Light(Color.Green, new TrafficLightDigits(new Digit(0), new Digit("0000000"))));

            Assert.Equal(new int[] { 5, 85 }, prediction.Start);
            Assert.Equal(new[] { "0000000", "0010010" }, prediction.Missing);

            //red
            prediction = await service.PredictAsync(sequence, new Light(Color.Red));

            Assert.Equal(new int[] { 5 }, prediction.Start);
            Assert.Equal(new[] { "0000000", "0010010" }, prediction.Missing);
        }

        [Fact]
        public async Task StartWith99_WhenAllSectionsAreServiceable_ShouldReturn99()
        {
            var lights = new[]
            {
                new Light(Color.Green, new TrafficLightDigits(99)),
                new Light(Color.Green, new TrafficLightDigits(98)),
                new Light(Color.Green, new TrafficLightDigits(97)),
                new Light(Color.Green, new TrafficLightDigits(96)),
                new Light(Color.Green, new TrafficLightDigits(95)),
                new Light(Color.Green, new TrafficLightDigits(94)),
                new Light(Color.Green, new TrafficLightDigits(93)),
                new Light(Color.Green, new TrafficLightDigits(92)),
                new Light(Color.Green, new TrafficLightDigits(91)),
                new Light(Color.Green, new TrafficLightDigits(90)),
                new Light(Color.Green, new TrafficLightDigits(89))
            };

            Prediction prediction = null;
            var sequence = await service.CreateSequenceAsync();

            foreach (var ligh in lights)
                prediction = await service.PredictAsync(sequence, ligh);

            Assert.Single(prediction.Start);
            Assert.Equal(99, prediction.Start[0]);
            Assert.Equal(new[] { "0000000", "0000000" }, prediction.Missing);
        }

        [Fact]
        public async Task Predict_WithDuplicate_ShouldThrow()
        {
            var lights = new[]
            {
                new Light(Color.Green, new TrafficLightDigits(99)),
                new Light(Color.Green, new TrafficLightDigits(98)),
                new Light(Color.Green, new TrafficLightDigits(97)),
                new Light(Color.Green, new TrafficLightDigits(97)),
            };

            var sequence = await service.CreateSequenceAsync();

            await Assert.ThrowsAsync<CoreException>(async () =>
            {
                foreach (var ligh in lights)
                   await service.PredictAsync(sequence, ligh);
            });
        }

        private static ITrafficLightService CreateService()
        {
            var digitPredictor = new DigitPredictor(new BrokenSectionsExpert());
            var timePredictor = new TrafficLightTimePredictor(digitPredictor);
            var storage = new EfStorage(new Context(CreateOptions()));

            return new TrafficLightService(storage, new BrokenSectionsExpert(), timePredictor);
        }

        private static DbContextOptions<Context> CreateOptions()
        {
            return new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase("TestTrafficLightDb")
                .Options;
        }
    }
}
