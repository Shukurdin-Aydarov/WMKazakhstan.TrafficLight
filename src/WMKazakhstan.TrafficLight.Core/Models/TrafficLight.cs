namespace WMKazakhstan.TrafficLight.Core.Models
{
    public struct TrafficLight
    {
        public TrafficLight(Color color) : this(color, new TrafficLightDigits()) { }

        public TrafficLight(Color color, TrafficLightDigits digits)
        {
            Color = color;
            Digits = digits;
        }

        public Color Color { get; }
        public TrafficLightDigits Digits { get; }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 12;

                hash = hash * 25 + Color.GetHashCode();
                hash = hash * 25 + Digits.GetHashCode();

                return hash;
            }
        }
    }
}
