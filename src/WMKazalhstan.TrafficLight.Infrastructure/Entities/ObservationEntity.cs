using System;

using WMKazakhstan.TrafficLight.Core.Models;

namespace WMKazakhstan.TrafficLight.Infrastructure.Entities
{
    internal class ObservationEntity
    {
        public int Id { get; set; }

        public DateTimeOffset Date { get; set; }
        public Color Color { get; set; }
        
        public string High { get; set; }
        public string Low { get; set; }

        public int[] Digits { get; set; }

        public Guid SequenceId { get; set; }
        public SequenceEntity Sequence { get; set; }
    }
}
