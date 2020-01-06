using System;

namespace WMKazakhstan.TrafficLight.Core.Models
{
    public struct Observation
    {
        public Observation(Guid sequence, TrafficLight trafficLight, TrafficLightDigits[] predictedDigits)
        {
            Sequence = sequence;
            TrafficLight = trafficLight;
            PredictedDigits = predictedDigits;
        }

        public Guid Sequence { get; }

        public TrafficLight TrafficLight { get; }

        public TrafficLightDigits[] PredictedDigits { get; }
    }
}
