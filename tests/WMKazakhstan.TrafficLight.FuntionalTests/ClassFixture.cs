using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

using WMKazakhstan.TrafficLight.Web;

namespace WMKazakhstan.TrafficLight.FunctionalTests
{
    public class ClassFixture : IClassFixture<WebApplicationFactory<Startup>>
    {
        public ClassFixture(WebApplicationFactory<Startup> factory)
        {
            Client = factory.CreateClient();
        }

        public HttpClient Client { get; }

        public async Task<T> FromJsonAsync<T>(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
