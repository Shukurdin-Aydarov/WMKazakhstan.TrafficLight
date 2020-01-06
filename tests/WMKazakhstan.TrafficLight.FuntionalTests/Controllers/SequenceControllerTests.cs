using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

using WMKazakhstan.TrafficLight.Web;
using WMKazakhstan.TrafficLight.Web.Models;
using System.Net.Http;

namespace WMKazakhstan.TrafficLight.FunctionalTests.Controllers
{
    public class SequenceControllerTests : ClassFixture
    {
        public SequenceControllerTests(WebApplicationFactory<Startup> factory) : base(factory) { }

        [Fact]
        public async Task CreateSequence()
        {
            var response = await Client.PostAsync("/sequence/create", new StringContent(""));

            response.EnsureSuccessStatusCode();

            var sequence = await FromJsonAsync<CreatedSequence>(response);

            Assert.Equal("ok", sequence.Status);
            Assert.True(sequence.Sequence != default);
        }
    }
}
