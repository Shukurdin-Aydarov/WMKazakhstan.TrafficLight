using WMKazakhstan.TrafficLight.Core.Models;

namespace WMKazakhstan.TrafficLight.Core.Interfaces
{
    public interface IBrokenSectionsExpert
    {
        TrafficLightDigits IdentifyBrokenSections(
            Models.TrafficLight currentTrafficLight,
            TrafficLightDigits[] currentPredictedDigits,
            Observation[] previousObservations);

        TrafficLightDigits GetSectionsThatWasTurnedOff(Models.TrafficLight trafficLight, Observation[] previousObservations);
    }
}
