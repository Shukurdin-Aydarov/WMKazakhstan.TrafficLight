using System;
using System.Collections.Generic;

namespace WMKazakhstan.TrafficLight.Infrastructure.Entities
{
    internal class SequenceEntity
    {
        public Guid Id { get; set; }

        public ICollection<ObservationEntity> Observations { get; set; }
    }
}
