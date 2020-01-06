namespace WMKazakhstan.TrafficLight.Web.Models
{
    public class Error
    {
        public Error(string msg)
        {
            Msg = msg;
            Status = Infrastructure.Status.Error;
        }

        public string Status { get; }

        public string Msg { get; }
    }
}
