using System;
using System.ComponentModel.DataAnnotations;

namespace WMKazakhstan.TrafficLight.Web.Models
{
    public class AddObservation
    {
        [Required(ErrorMessage = "sequence is required")]
        public Guid? Sequence { get; set; }

        [Required(ErrorMessage = "observation is required")]
        public Observation Observation { get; set; }
    }
}
