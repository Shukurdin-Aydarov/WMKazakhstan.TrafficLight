
namespace WMKazakhstan.TrafficLight.Core.Models
{
    public class Prediction
    {
        public Prediction(int[] start, string[] missing)
        {
            Start = start;
            Missing = missing;
        }

        public int[] Start { get; }

        public string[] Missing { get; }
    }
}
