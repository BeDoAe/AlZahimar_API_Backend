using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZahimarProject.Attributes;
using ZahimarProject.Models;

namespace ZahimarProject.DTOS.AuthenticationDTOs
{
    public class RelativeRegistrationWithPatientDto
    {
        //Data for Relative
        [Required, MaxLength(25), MinLength(2)]
        public string RelativeFirstName { get; set; }


        [Required, MaxLength(25), MinLength(2)]
        public string RelativeLastName { get; set; }


        [Required, MaxLength(50), MinLength(5)]
        public string RelativeUserName { get; set; }


        [EmailAddress]
        public string RelativeEmail { get; set; }
        

        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone number must start with 010, 011, 012, or 015 and be followed by 8 digits.")]
        public string RelativePhoneNumber { get; set; }

        public string RelativeAddress { get; set; }

        [Range(0, 1, ErrorMessage = "Gender must be 0 (male) or 1 (female)")]
        public Gender RelativeGender { get; set; }

        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
        //ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string RelativePassword { get; set; }

        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
        //ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string RelativeConfirmPassword { get; set; }

        //Data for Patient  
        [Required, MaxLength(25), MinLength(2)]
        public string PatientFirstName { get; set; }


        [Required, MaxLength(25), MinLength(2)]
        public string PatientLastName { get; set; }

        public int PatientAge { get; set; }

        public string PatientAddress { get; set; }

        [Range(0, 1, ErrorMessage = "Gender must be 0 (male) or 1 (female)")]
        public Gender PatientGender { get; set; }

        public string?   PatientLocation { get; set; }
    }
}
