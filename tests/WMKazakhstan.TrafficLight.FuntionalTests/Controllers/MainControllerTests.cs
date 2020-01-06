using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

using WMKazakhstan.TrafficLight.Web;
using WMKazakhstan.TrafficLight.Web.Models;

namespace WMKazakhstan.TrafficLight.FunctionalTests.Controllers
{
    public class MainControllerTests : ClassFixture
    {
        public MainControllerTests(WebApplicationFactory<Startup> factory) : base(factory) { }

        [Fact]
        public async Task Clear()
        {
            var response = await Client.GetAsync("/clear");

            response.EnsureSuccessStatusCode();

            var sequence = await FromJsonAsync<CreatedSequence>(response);

            Assert.Equal("ok", sequence.Status);
        }
    }
}
