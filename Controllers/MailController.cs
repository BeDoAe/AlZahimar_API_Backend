using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZahimarProject.Authentication;
using ZahimarProject.Models;
using ZahimarProject.Services.EmailServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ZahimarProject.Helpers.UnitOfWorkFolder;

namespace ZahimarProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mailService;
        private readonly IUnitOfWork _unitOfWork;

        // Injecting the IMailService into the constructor
        public MailController(IMailService mailService, IUnitOfWork unitOfWork)
        {
            _mailService = mailService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("SendMail")]
        public bool SendMail(MailData mailData)
        {
            return _mailService.SendMail(mailData);
        }

        [HttpPost]
        [Route("CheckMail")]
        public async Task<GeneralResponse> CheckMail(string email)
        {
            AppUser appUser = await _unitOfWork._userManager.FindByEmailAsync(email);
            if (appUser != null)
            {
                string token = await _unitOfWork._userManager.GeneratePasswordResetTokenAsync(appUser);
                string resetLink = Url.Action(nameof(SetNewPassword), "Mail", new { token, email = appUser.Email }, Request.Scheme);

                MailData mailData = new MailData
                {
                    EmailToId = appUser.Email,
                    EmailToName = appUser.UserName,
                    EmailSubject = "Password Reset",
                    EmailBody = $"Please reset your password using this link: {resetLink}"
                };

                bool isMailSent = _mailService.SendMail(mailData);
                if (isMailSent)
                {
                    return new GeneralResponse { IsSuccess = true, Data = "Password reset link has been sent to your email." };
                }
                return new GeneralResponse { IsSuccess = false, Data = "Failed to send email." };
            }

            return new GeneralResponse { IsSuccess = false, Data = "This email is not registered." };
        }
        [HttpPost]
        [Route("SetNewPassword")]
        public async Task<GeneralResponse> SetNewPassword([FromQuery] string token, [FromQuery] string email, [FromBody] SetNewPasswordModel model)
        {
            AppUser appUser = await _unitOfWork._userManager.FindByEmailAsync(email);
            if (appUser != null)
            {
                IdentityResult result = await _unitOfWork._userManager.ResetPasswordAsync(appUser, token, model.NewPassword);
                if (result.Succeeded)
                {
                    return new GeneralResponse { IsSuccess = true, Data = "Password has been reset successfully." };
                }

                return new GeneralResponse { IsSuccess = false, Data = "Failed to reset password." };
            }

            return new GeneralResponse { IsSuccess = false, Data = "Invalid email or token." };
        }
    }
}
