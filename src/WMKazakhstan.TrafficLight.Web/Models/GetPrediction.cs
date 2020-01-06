using WMKazakhstan.TrafficLight.Core.Models;

namespace WMKazakhstan.TrafficLight.Web.Models
{
    public class GetPrediction
    {
        public GetPrediction(Prediction response)
        {
            Response = response;

            Status = Infrastructure.Status.Ok;
        }

        public string Status { get; }

        public Prediction Response { get; }
    }
}
