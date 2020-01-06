using System.Collections.Generic;

using WMKazakhstan.TrafficLight.Core.Models;

namespace WMKazakhstan.TrafficLight.Core.Interfaces
{
    /// <summary>
    ///     Provides solution area.
    /// </summary>
    public interface IDigitPredictor
    {
        IEnumerable<TrafficLightDigits> Predict(Models.TrafficLight trafficLight, Observation[] previousObservations);
    }
}
