using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ZahimarProject.Attributes
{
    public class ValidPhoneNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Phone number is required.");
            }

            var phoneNumber = value.ToString();

            // Check if phone number matches the required pattern
            var regex = new Regex(@"^(010|012|011|015)\d{8}$");
            if (!regex.IsMatch(phoneNumber))
            {
                return new ValidationResult("Phone number must start with 010, 012, 011, or 015 and be followed by 8 digits.");
            }

            return ValidationResult.Success;
        }
    }
}
