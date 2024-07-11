using System.ComponentModel.DataAnnotations;
using ZahimarProject.Attributes;

namespace ZahimarProject.DTOS.AuthenticationDTOs
{
    public class LoginDto
    {
        [Required, MaxLength(50), MinLength(5)]
        public string UserName { get; set; }

        //[StrongPassword]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
        //ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string Password { get; set; }
    }
}
