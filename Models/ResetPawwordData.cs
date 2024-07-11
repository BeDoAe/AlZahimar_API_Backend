namespace ZahimarProject.Models
{
    public class ResetPasswordData
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string VerificationCode { get; set; }
    }
}
