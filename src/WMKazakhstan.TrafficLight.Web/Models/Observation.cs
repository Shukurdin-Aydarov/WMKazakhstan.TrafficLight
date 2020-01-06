using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using WMKazakhstan.TrafficLight.Core.Models;

namespace WMKazakhstan.TrafficLight.Web.Models
{
    public class Observation : IValidatableObject
    {
        public Color? Color { get; set; }

        public string[] Numbers { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Color.HasValue)
                yield return new ValidationResult("color is required");

            if (Color.Value == Core.Models.Color.Green && Numbers?.Length != 2)
                yield return new ValidationResult("numbers is invalid");
        }
    }
}
