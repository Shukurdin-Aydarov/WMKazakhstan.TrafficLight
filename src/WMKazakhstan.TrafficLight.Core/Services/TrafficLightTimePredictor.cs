using System.Linq;
using System.Collections.Generic;

using WMKazakhstan.TrafficLight.Core.Models;
using WMKazakhstan.TrafficLight.Core.Interfaces;

namespace WMKazakhstan.TrafficLight.Core.Services
{
    public class TrafficLightTimePredictor : ITrafficLightTimePredictor
    {
        private readonly IDigitPredictor predictor;

        public TrafficLightTimePredictor(IDigitPredictor predictor)
        {
            this.predictor = predictor;
        }

        public TrafficLightDigits[] Predict(Models.TrafficLight trafficLight, Observation[] previous)
        {
            Validate(trafficLight, previous);

            var predictedDigits = predictor
                .Predict(trafficLight, previous)
                .ToArray();

            var solutions = predictedDigits;

            if (previous.Length == 0)
                return predictedDigits;

            if (IsSolutionFound(solutions))
            {
                solutions[0] += previous.Length;

                if (!previous[0].PredictedDigits.Contains(solutions[0]))
                    Throws.NoSolutionFound();

                return solutions;
            }

            for (var i = previous.Length - 1; i >= 0; i--)
            {
                var prev = previous[i];
                var incremented = IncrementAllDigits(solutions);
                
                solutions = prev.PredictedDigits.Intersect(incremented).ToArray();
                                    
                if (solutions.Length == 0)
                    Throws.NoSolutionFound();
            }
            
            return solutions;
        }
        
        private void Validate(Models.TrafficLight trafficLight, Observation[] previous)
        {
            if (previous.Length == 0 && trafficLight.Color == Color.Red)
                Throws.NotEnoughData();

            if (previous.Length > 0 && previous.Last().TrafficLight.Color == Color.Red)
                Throws.RedShouldBeTheLast();
        }

        private static bool IsSolutionFound(TrafficLightDigits[] solutions)
        {
            return solutions.Length == 1;
        }

        private static IEnumerable<TrafficLightDigits> IncrementAllDigits(IEnumerable<TrafficLightDigits> digits)
        {
            foreach (var digit in digits)
            {
                var incremented = digit;
                incremented++;

                yield return incremented;
            }
        }
    }
}
