using System;

namespace WMKazakhstan.TrafficLight.Core.Models
{
    public struct TrafficLightDigits
    {
        public static readonly TrafficLightDigits Default = new TrafficLightDigits();

        public TrafficLightDigits(Digit high, Digit low) : this()
        {
            High = high;
            Low = low;
        }

        public TrafficLightDigits(int state): this()
        {
            ThrowIfOutOfRange(state);

            High = new Digit(state / 10);
            Low = new Digit(state % 10);
        }

        public TrafficLightDigits(string high, string low) : this()
        {
            High = new Digit(high);
            Low = new Digit(low);
        }

        public Digit High { get; }
        public Digit Low { get; }

        public override string ToString()
        {
            return $"{High}{Low}";
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 12;

                hash = hash * 32 + High.GetHashCode();
                hash = hash * 32 + Low.GetHashCode();

                return hash;
            }
        }

        public static TrafficLightDigits operator ++(TrafficLightDigits digits)
        {
            if (digits.High.Equals(Constants.Nine) && digits.Low.Equals(Constants.Nine))
                return new TrafficLightDigits(Constants.Nine, Constants.Nine);

            if (digits.Low.Equals(Constants.Nine))
            {
                var high = digits.High;
                high++;

                return new TrafficLightDigits(high, Constants.Zero);
            }

            var low = digits.Low;
            low++;

            return new TrafficLightDigits(digits.High, low);
        }

        public static TrafficLightDigits operator -(TrafficLightDigits digits, int val)
        {
            var number = digits.ToNumber();

            number -= val;

            return new TrafficLightDigits(number < 0 ? 0 : number);
        }

        public static TrafficLightDigits operator +(TrafficLightDigits digits, int val)
        {
            var number = digits.ToNumber();

            number += val;

            return new TrafficLightDigits(number > 99 ? 99 : number);
        }

        public int ToNumber()
        {
            return High.ToNumber() * 10 + Low.ToNumber();
        }

        public string[] ToBitString()
        {
            return new[] { High.ToBitString(), Low.ToBitString() };
        }

        private static void ThrowIfOutOfRange(int state)
        {
            if (state < 0 || state > 99)
                throw new ArgumentOutOfRangeException(nameof(state));
        }
    }
}
