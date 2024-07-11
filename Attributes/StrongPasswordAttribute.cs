using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ZahimarProject.Attributes
{
    public class StrongPasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Password is required.");
            }

            var password = value.ToString();

            // Check if password contains at least one uppercase character
            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                return new ValidationResult("Password must contain at least one uppercase character.");
            }

            // Check if password contains at least one number
            if (!Regex.IsMatch(password, @"\d"))
            {
                return new ValidationResult("Password must contain at least one number.");
            }

            return ValidationResult.Success;
        }
    }
}
