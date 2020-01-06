using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

using WMKazakhstan.TrafficLight.Core;
using WMKazakhstan.TrafficLight.Web;
using WMKazakhstan.TrafficLight.Web.Models;

namespace WMKazakhstan.TrafficLight.FunctionalTests.Controllers
{
    public class ObservationControllerTests : ClassFixture
    {
        public ObservationControllerTests(WebApplicationFactory<Startup> factory) : base(factory) { }

        [Fact]
        public async Task SendObservation()
        {
            var sequence = await CreateSequenceAsync();

            var response = await SendObservationAsync(new AddObservation
            {
                Sequence = sequence,
                Observation = new Observation
                {
                    Color = Core.Models.Color.Green,
                    Numbers = new[] { Constants.Nine.ToBitString(), Constants.Nine.ToBitString() }
                }
            });

            response.EnsureSuccessStatusCode();

            var prediction = await FromJsonAsync<GetPrediction>(response);

            Assert.Equal("ok", prediction.Status);
            Assert.Equal(new[] { 88, 89, 98, 99 }, prediction.Response.Start);
            Assert.Equal(new[] { "0000000", "0000000" }, prediction.Response.Missing);
        }

        [Fact]
        public async Task SendObservations_WithoutSequence_ShouldReturnError()
        {
            var response = await SendObservationAsync(new AddObservation
            {
                Sequence = null,
                Observation = new Observation
                {
                    Color = Core.Models.Color.Green,
                    Numbers = new[] { Constants.Nine.ToBitString(), Constants.Nine.ToBitString() }
                }
            });

            response.EnsureSuccessStatusCode();

            var error = await FromJsonAsync<Error>(response);

            Assert.Equal("error", error.Status);
            Assert.Equal("sequence is required", error.Msg);
        }

        [Fact]
        public async Task SendObservations_WithoutObservation_ShouldReturnError()
        {
            var response = await SendObservationAsync(new AddObservation
            {
                Sequence = await CreateSequenceAsync(),
                Observation = null
            });

            response.EnsureSuccessStatusCode();

            var error = await FromJsonAsync<Error>(response);

            Assert.Equal("error", error.Status);
            Assert.Equal("observation is required", error.Msg);
        }

        private async Task<HttpResponseMessage> SendObservationAsync(AddObservation observation)
        {
            var content = new StringContent(JsonConvert.SerializeObject(observation));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return await Client.PostAsync("/observation/add", content);
        }

        private async Task<Guid> CreateSequenceAsync()
        {
            var response = await Client.PostAsync("/sequence/create", new StringContent(""));

            response.EnsureSuccessStatusCode();

            var sequence = await FromJsonAsync<CreatedSequence>(response);

            return sequence.Sequence;
        }
    }
}
