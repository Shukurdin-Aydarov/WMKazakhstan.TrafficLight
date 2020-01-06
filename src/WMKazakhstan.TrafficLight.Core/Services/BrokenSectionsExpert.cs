using System.Linq;
using System.Collections.Generic;

using WMKazakhstan.TrafficLight.Core.Interfaces;
using WMKazakhstan.TrafficLight.Core.Models;

namespace WMKazakhstan.TrafficLight.Core.Services
{
    public class BrokenSectionsExpert : IBrokenSectionsExpert
    {
        public TrafficLightDigits IdentifyBrokenSections(Models.TrafficLight currentTrafficLight, TrafficLightDigits[] currentPredictedDigits, Observation[] previousObservations)
        {
            var current = IdentifyBrokenSectionsInternal(currentTrafficLight.Digits, currentPredictedDigits);

            var digits = currentPredictedDigits;

            for (var i = previousObservations.Length - 1; i >= 0; i--)
            {
                for(var j = 0; j < digits.Length; j++)
                    digits[j]++;

                var observation = previousObservations[i];
                var trafficLightDigits = observation.TrafficLight.Digits;

                current = Join(current, IdentifyBrokenSectionsInternal(trafficLightDigits, digits));
                
                current = JoinBrokenSections(observation.TrafficLight.Digits, current);
            }

            return JoinBrokenSections(currentTrafficLight.Digits, current);
        }

        public TrafficLightDigits GetSectionsThatWasTurnedOff(Models.TrafficLight trafficLight, Observation[] observations)
        {
            // all sections are true
            var current = new TrafficLightDigits(88);

            foreach (var o in observations)
            {
                // all sections are false
                if (current.Equals(TrafficLightDigits.Default))
                    return current;

                current = JoinBrokenSections(o.TrafficLight.Digits, current);
            }

            return JoinBrokenSections(trafficLight.Digits, current);
        }

        private TrafficLightDigits IdentifyBrokenSectionsInternal(TrafficLightDigits broken, IEnumerable<TrafficLightDigits> digits)
        {
            var high = GetBrokenSections(broken.High, digits.Select(x => x.High).ToArray());
            var low = GetBrokenSections(broken.Low, digits.Select(x => x.Low).ToArray());

            return new TrafficLightDigits(high, low);
        }

        private TrafficLightDigits Join(TrafficLightDigits current, TrafficLightDigits next)
        {
            return new TrafficLightDigits(join(current.High, next.High), join(current.Low, next.Low));

            static Digit join(Digit current, Digit next)
            {
                current.Section0 |= next.Section0;
                current.Section1 |= next.Section1;
                current.Section2 |= next.Section2;
                current.Section3 |= next.Section3;
                current.Section4 |= next.Section4;
                current.Section5 |= next.Section5;
                current.Section6 |= next.Section6;

                return current;
            }
        }

        private Digit GetBrokenSections(Digit brokenDigit, Digit[] digits)
        {
            var sections = brokenDigit;

            var swichedOnSection = sections;

            for (var i = 0; i < 7; i++)
            {
                var current = sections;

                if (!swichedOnSection.Section0) swichedOnSection.Section0 = current.Section0 = true;
                else if (!swichedOnSection.Section1) swichedOnSection.Section1 = current.Section1 = true;
                else if (!swichedOnSection.Section2) swichedOnSection.Section2 = current.Section2 = true;
                else if (!swichedOnSection.Section3) swichedOnSection.Section3 = current.Section3 = true;
                else if (!swichedOnSection.Section4) swichedOnSection.Section4 = current.Section4 = true;
                else if (!swichedOnSection.Section5) swichedOnSection.Section5 = current.Section5 = true;
                else if (!swichedOnSection.Section6) swichedOnSection.Section6 = current.Section6 = true;
                else break;

                if (digits.All(x => x.MaybeEquals(current)))
                    sections = current;
            }

            return JoinBrokenSections(brokenDigit, sections);
        }

        private TrafficLightDigits JoinBrokenSections(TrafficLightDigits faulty, TrafficLightDigits sections)
        {
            return new TrafficLightDigits(
                JoinBrokenSections(faulty.High, sections.High),
                JoinBrokenSections(faulty.Low, sections.Low));
        }

        private Digit JoinBrokenSections(Digit faulty, Digit sections)
        {
            return new Digit
            {
                Section0 = !faulty.Section0 && sections.Section0,
                Section1 = !faulty.Section1 && sections.Section1,
                Section2 = !faulty.Section2 && sections.Section2,
                Section3 = !faulty.Section3 && sections.Section3,
                Section4 = !faulty.Section4 && sections.Section4,
                Section5 = !faulty.Section5 && sections.Section5,
                Section6 = !faulty.Section6 && sections.Section6
            };
        }
    }
}
