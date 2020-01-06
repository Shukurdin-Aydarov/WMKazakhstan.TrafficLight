using System;
using System.Threading.Tasks;

using WMKazakhstan.TrafficLight.Core.Models;

namespace WMKazakhstan.TrafficLight.Core.Interfaces
{
    public interface ITrafficLightService
    {
        Task<Guid> CreateSequenceAsync();
        
        Task<Prediction> PredictAsync(Guid sequenceId, Models.TrafficLight trafficLight);
    }
}
