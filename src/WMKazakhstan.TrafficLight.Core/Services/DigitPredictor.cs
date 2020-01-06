using System.Linq;
using System.Collections.Generic;

using WMKazakhstan.TrafficLight.Core.Models;
using WMKazakhstan.TrafficLight.Core.Interfaces;

namespace WMKazakhstan.TrafficLight.Core.Services
{
    public class DigitPredictor : IDigitPredictor
    {
        private readonly IBrokenSectionsExpert expert;

        public DigitPredictor(IBrokenSectionsExpert expert)
        {
            this.expert = expert;
        }

        public IEnumerable<TrafficLightDigits> Predict(Models.TrafficLight trafficLight, Observation[] previousObservations)
        {
            if (trafficLight.Color == Color.Red)
            {
                yield return new TrafficLightDigits(0);

                yield break;
            }

            var sectionsThatWasTurnedOff = expert.GetSectionsThatWasTurnedOff(trafficLight, previousObservations);

            if (AreAllSectionsServiceable(sectionsThatWasTurnedOff))
            {
                yield return trafficLight.Digits;

                yield break;
            }

            var digit = trafficLight.Digits;
            
            var hightLevel = Digits
                .Where(x => x.MaybeEquals(digit.High));
            
            if (AreAllSectionsServiceable(sectionsThatWasTurnedOff.Low))
            {
                foreach (var high in hightLevel)
                    yield return new TrafficLightDigits(high, digit.Low);

                yield break;
            }

            var lowLevel = Digits.Where(x => x.MaybeEquals(digit.Low)).ToArray();
            
            foreach (var high in hightLevel)
                for (var i = 0; i < lowLevel.Length; i++)
                    yield return new TrafficLightDigits(high, lowLevel[i]);
        }

        private static bool AreAllSectionsServiceable(TrafficLightDigits digits)
        {
            return digits.Equals(TrafficLightDigits.Default);
        }

        private static bool AreAllSectionsServiceable(Digit digit)
        {
            return digit.Equals(Digit.Default);
        }

        private static readonly HashSet<Digit> Digits = new HashSet<Digit>
        {
            Constants.Zero,
            Constants.One,
            Constants.Two,
            Constants.Three,
            Constants.Four,
            Constants.Five,
            Constants.Six,
            Constants.Seven,
            Constants.Eight,
            Constants.Nine
        };
    }
}
