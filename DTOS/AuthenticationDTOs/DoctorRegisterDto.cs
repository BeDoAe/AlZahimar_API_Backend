using System.ComponentModel.DataAnnotations;
using ZahimarProject.Attributes;
using ZahimarProject.Models;

namespace ZahimarProject.DTOS.AuthenticationDTOs
{
    public class DoctorRegisterDto
    {
        [Required, MaxLength(25), MinLength(2)]
        public string FirstName { get; set; }


        [Required, MaxLength(25), MinLength(2)]
        public string LastName { get; set; }


        [Required, MaxLength(50), MinLength(5)]
        public string UserName { get; set; }


        [EmailAddress]
        public string Email { get; set; }

        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
        //ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string Password { get; set; }

        [ComparePasswords("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Address { get; set; }


        [Range(0, 1, ErrorMessage = "Gender must be 0 (male) or 1 (female)")]
        public Gender Gender { get; set; }

        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone number must start with 010, 011, 012, or 015 and be followed by 8 digits.")]
        public string Phone { get; set; }

        public int CardNumber { get; set; }

        [Range(25, 90, ErrorMessage = "Age must be between 25 and 90")]
        public int Age { get; set; }


        [Range(100, 500, ErrorMessage = "Price must be between 100 and 500.")]
        public double Price { get; set; }
    }
}
