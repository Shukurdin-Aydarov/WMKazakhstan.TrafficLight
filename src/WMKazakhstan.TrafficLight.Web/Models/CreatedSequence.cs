using System;

namespace WMKazakhstan.TrafficLight.Web.Models
{
    public class CreatedSequence
    {
        public CreatedSequence(Guid sequence)
        {
            Sequence = sequence;
            Status = Infrastructure.Status.Ok;
        }

        public string Status { get; }

        public Guid Sequence { get; }
    }
}
