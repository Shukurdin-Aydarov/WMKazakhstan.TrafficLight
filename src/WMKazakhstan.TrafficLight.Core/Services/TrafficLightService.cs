using System;
using System.Linq;
using System.Threading.Tasks;

using WMKazakhstan.TrafficLight.Core.Interfaces;
using WMKazakhstan.TrafficLight.Core.Models;

namespace WMKazakhstan.TrafficLight.Core.Services
{
    public class TrafficLightService : ITrafficLightService
    {
        private readonly IStorage storage;
        private readonly IBrokenSectionsExpert expert;
        private readonly ITrafficLightTimePredictor timePredictor;

        public TrafficLightService(IStorage storage, IBrokenSectionsExpert expert, ITrafficLightTimePredictor timePredictor)
        {
            this.storage = storage;
            this.expert = expert;
            this.timePredictor = timePredictor;
        }

        public async Task<Guid> CreateSequenceAsync()
        {
            return await storage.CreateSequenceAsync();
        }

        public async Task<Prediction> PredictAsync(Guid sequenceId, Models.TrafficLight trafficLight)
        {
            var observations = (await storage.FindObservationAsync(sequenceId)).ToArray();

            if (observations.Any(x => x.TrafficLight.Equals(trafficLight)))
                Throws.DuplicateObservation();

            var predictedDigits = timePredictor.Predict(trafficLight, observations);
            
            var decrementedDigits = observations.Length == 0
                ? predictedDigits
                : predictedDigits.Select(x => x - observations.Length);

            var faultySections = expert.IdentifyBrokenSections(trafficLight, decrementedDigits.ToArray(), observations);

            await storage.SaveObservationAsync(sequenceId, trafficLight, decrementedDigits);

            var starts = predictedDigits
                .Select(x => x.ToNumber())
                .ToArray();

            var missings = faultySections.ToBitString();

            return new Prediction(starts, missings);
        }
    }
}
