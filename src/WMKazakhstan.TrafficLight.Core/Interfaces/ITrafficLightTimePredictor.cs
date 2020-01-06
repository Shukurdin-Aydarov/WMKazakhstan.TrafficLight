using System.Collections.Generic;

using WMKazakhstan.TrafficLight.Core.Models;

namespace WMKazakhstan.TrafficLight.Core.Interfaces
{
    public interface ITrafficLightTimePredictor
    {
        TrafficLightDigits[] Predict(Models.TrafficLight trafficLight, Observation[] observations);
    }
}
