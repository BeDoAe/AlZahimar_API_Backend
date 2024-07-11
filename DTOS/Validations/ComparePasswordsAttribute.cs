using System;
using System.ComponentModel.DataAnnotations;

namespace ZahimarProject.Attributes
{
    public class ComparePasswordsAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public ComparePasswordsAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var currentValue = value as string;
            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (property == null)
            {
                throw new ArgumentException("Property with this name not found");
            }

            var comparisonValue = property.GetValue(validationContext.ObjectInstance) as string;

            if (!string.Equals(currentValue, comparisonValue))
            {
                return new ValidationResult(ErrorMessage ?? "The password and confirmation password do not match.");
            }

            return ValidationResult.Success;
        }
    }
}
