using System.ComponentModel.DataAnnotations;

namespace ZahimarProject.Models
{
    public class SetNewPasswordModel
    {
        [Required]
        public string NewPassword { get; set; }
    }
}
