using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WMKazakhstan.TrafficLight.Core.Models;
using Light = WMKazakhstan.TrafficLight.Core.Models.TrafficLight;

namespace WMKazakhstan.TrafficLight.Core.Interfaces
{
    public interface IStorage
    {
        Task<Guid> CreateSequenceAsync();

        Task ClearAsync();

        ValueTask<IEnumerable<Observation>> FindObservationAsync(Guid sequenceId);

        Task SaveObservationAsync(Guid sequenceId, Light trafficLight, IEnumerable<TrafficLightDigits> digits);
    }
}
