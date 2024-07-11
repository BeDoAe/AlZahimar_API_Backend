using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using ZahimarProject.Authentication;
using ZahimarProject.DTOS.AuthenticationDTOs;
using ZahimarProject.DTOS.DoctorDTO;
using ZahimarProject.Helpers;
using ZahimarProject.Helpers.Enums;
using ZahimarProject.Helpers.UnitOfWorkFolder;
using ZahimarProject.Models;
using ZahimarProject.Services.EmailServices;

namespace ZahimarProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration config;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMailService mailService;

        public AccountController
        (RoleManager<IdentityRole> roleManager, IConfiguration _config, IUnitOfWork unitOfWork, IMailService mailService)
        {
            this.roleManager = roleManager;
                 config = _config;
            this.unitOfWork = unitOfWork;
            this.mailService = mailService;
        }


        [HttpPost("CheckUsername")]
        public async Task<ActionResult<GeneralResponse>> CheckUsername(CheckUsernameDto checkUsernameDto)
        {
            if (string.IsNullOrEmpty(checkUsernameDto.UserName))
            {
                return new GeneralResponse() { IsSuccess = false, Data = "Username cannot be null or empty" };
            }

            var user = await unitOfWork._userManager.FindByNameAsync(checkUsernameDto.UserName);

            if (user != null)
            {
                return new GeneralResponse() { IsSuccess = false, Data = "Username is already taken" };
            }

            return new GeneralResponse() { IsSuccess = true, Data = "Username is available" };
        }



        [HttpPost("Register/Doctor")]
        public async Task<ActionResult<GeneralResponse>> Register(DoctorRegisterDto registerDto)
        {
            if (ModelState.IsValid)
            {
                AppUser appuser = new AppUser()
                {
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                    PasswordHash = registerDto.Password,
                    PhoneNumber=registerDto.Phone,

                };
                IdentityResult result =
                    await unitOfWork._userManager.CreateAsync(appuser, registerDto.Password);
                if (result.Succeeded)
                {
                    if (!await roleManager.RoleExistsAsync("Doctor"))
                    {
                        var roleResult = await roleManager.CreateAsync(new IdentityRole("Doctor"));
                        if (!roleResult.Succeeded)
                        {
                            return new GeneralResponse() { IsSuccess  = false, Data = "Failed to create Doctor role" };
                        }
                    }

                    // Assign the Doctor role to the user
                    var addToRoleResult = await unitOfWork._userManager.AddToRoleAsync(appuser, "Doctor");
                    if (!addToRoleResult.Succeeded)
                    {
                        return new GeneralResponse() { IsSuccess = false, Data = addToRoleResult.Errors };
                    }


                    Doctor doctor = new Doctor()
                    {
                        //Name = $"{registerDto.FirstName} {registerDto.LastName}",
                        FirstName= registerDto.FirstName,
                        LastName = registerDto.LastName,
                        UserName = registerDto.UserName,
                        Address = registerDto.Address,
                        CardNumber = registerDto.CardNumber,
                        Age = registerDto.Age,
                        AppUser = appuser,
                        Gender = registerDto.Gender,
                        AppUserId = appuser.Id,
                        Phone = registerDto.Phone,
                        Price=registerDto.Price,
                        PicURL= "/images/default-avatar-profile-icon-vector-social-media-user-photo-concept-285140929.webp",
                        Status= DoctorAccountStatus.Pending,
                    };

                    unitOfWork.DoctorRepository.Insert(doctor);
                    unitOfWork.Save();
                    MailData mailData = new MailData()
                    {
                        EmailToId = doctor.AppUser.Email,
                        EmailToName = doctor.FirstName + " " + doctor.LastName,
                        EmailBody = $"" +
                        $"Dear {doctor.AppUser.Email} Your Join Request has been sent \n" +
                        $"Wait For Approval Mail .\n" +
                        $"Thank you for choosing our services.\n" +
                        $"Best regards,\n\n" +
                        $"Your RemindMe Team\n"
                        ,
                        EmailSubject = "Successful Join Request"
                    };

                    mailService.SendMail(mailData);
                    return new GeneralResponse { IsSuccess = true, Data = "Account Created" }; 
                }
                return new GeneralResponse() { IsSuccess = false, Data = result.Errors };
            }
            return new GeneralResponse() { IsSuccess = false, Data = ModelState };
        }

        [HttpPost("Login")]
        public async Task<ActionResult<GeneralResponse>> Login(LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                AppUser? userFromDb =
                     await unitOfWork._userManager.FindByNameAsync(loginDto.UserName);
                      
                if (userFromDb != null)
                {
                    bool found = await unitOfWork._userManager.CheckPasswordAsync(userFromDb, loginDto.Password);
                    if (found)
                    {
                        List<Claim> myclaims = new List<Claim>();
                        myclaims.Add(new Claim(ClaimTypes.Name, userFromDb.UserName));
                        myclaims.Add(new Claim(ClaimTypes.NameIdentifier, userFromDb.Id));
                        myclaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                        var roles = await unitOfWork._userManager.GetRolesAsync(userFromDb);
                        var LoggedInRole=roles.FirstOrDefault();
                        if (LoggedInRole == UserRoles.Doctor)
                        {
                            Doctor doctor = unitOfWork.DoctorRepository.GetDoctorToHandleLogin(d => d.AppUserId == userFromDb.Id);
                            if (doctor != null)
                            {
                                if (doctor.Status==DoctorAccountStatus.Pending)
                                {
                                    return new GeneralResponse() { IsSuccess = false, Data = "In Progress" };
                                }
                                if (doctor.Status == DoctorAccountStatus.Rejected)
                                {
                                    return new GeneralResponse() { IsSuccess = false, Data = "Your Data That You Entered isn't True Please Register Again" };
                                }
                            }
                        }
                        foreach (var role in roles)
                        {
                            myclaims.Add(new Claim(ClaimTypes.Role, role));
                        }


                        var SignKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(config["JWT:SecritKey"]));

                        SigningCredentials signingCredentials =
                            new SigningCredentials(SignKey, SecurityAlgorithms.HmacSha256);



                        JwtSecurityToken mytoken = new JwtSecurityToken(
                            issuer: config["JWT:ValidIss"],
                            audience: config["JWT:ValidAud"],
                            expires: DateTime.Now.AddHours(1),
                            claims: myclaims,
                            signingCredentials: signingCredentials);
                        return new GeneralResponse() { IsSuccess = true, Data = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(mytoken),
                            expired = mytoken.ValidTo,
                            roles= roles
                            
                            

                        }
                        };
                    }
                }
                return new GeneralResponse() { IsSuccess = false, Data = "Invalid account" };
            }
            return new GeneralResponse() { IsSuccess = false, Data = ModelState };
        }

        ////////////////////////////Relative Auth
        [HttpPost("RegisterRelative")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> RegisterRelative( RelativeRegistrationWithPatientDto dto)
        {
            var user = new AppUser
            {
                UserName = dto.RelativeUserName,
                Email = dto.RelativeEmail,
                FirstName = dto.RelativeFirstName,
                LastName = dto.RelativeLastName,
                PhoneNumber = dto.RelativePhoneNumber,
            };

            var result = await unitOfWork._userManager.CreateAsync(user, dto.RelativePassword);

            if (result.Succeeded)
            {
                if (!await roleManager.RoleExistsAsync(UserRoles.Relative))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Relative));

                await unitOfWork._userManager.AddToRoleAsync(user, UserRoles.Relative);
                Patient patient = new Patient()
                {
                    Address = dto.PatientAddress,
                    Age = dto.PatientAge,
                    Gender = dto.PatientGender,
                    FirstName= dto.PatientFirstName,
                    LastName= dto.PatientLastName,
                };
                unitOfWork.Save();
                var relative = new Relative
                {
                    FirstName = dto.RelativeFirstName,
                    LastName= dto.RelativeLastName,
                    UserName = dto.RelativeUserName,
                    PhoneNumber = dto.RelativePhoneNumber,
                    Address = dto.RelativeAddress,
                    Gender = dto.RelativeGender,
                    PatientId = patient.Id,
                    Patient = patient,
                    AppUserId = user.Id,
                    AppUser = user,
                    PicURL= "/images/default-avatar-profile-icon-vector-social-media-user-photo-concept-285140929.webp"
                };

                unitOfWork.RelativeRepository.Insert(relative);
                unitOfWork.Save();
                MailData mailData = new MailData()
                {
                    EmailToId = relative.AppUser.Email,
                    EmailToName = relative.FirstName + " " + relative.LastName,
                    EmailBody = $"" +
                    $"Dear {relative.AppUser.Email} Welcome You have joined us \n" +
                    $"Thank you for choosing our services.\n" +
                    $"Best regards,\n\n" +
                    $"RemindMe Team\n"
                    ,
                    EmailSubject = "Successful Registeration"
                };

                mailService.SendMail(mailData);
                return new GeneralResponse() { IsSuccess = true, Data = "Register Successfully" };
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return new GeneralResponse() { IsSuccess = false, Data = ModelState };

        }


        [HttpPost("logout")]
    
        public async Task<ActionResult<GeneralResponse>> Logout()
        {
            // Extract token from the Authorization header
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Add token to blacklist
            var blacklistedToken = new TokenBlacklist
            {

                Token = token,
                ExpirationDate = DateTime.UtcNow.AddHours(1) // Set expiration based on token expiration
            };

      
            unitOfWork.TokenBlackListRepository.Insert(blacklistedToken);
            unitOfWork.Save();

            // Clear authentication cookies
            await HttpContext.SignOutAsync();

            return new GeneralResponse { IsSuccess = true, Data = "Logged out successfully" };
        }
        //End Points For Admin
        [HttpGet]
        [Authorize(policy: UserRoles.Admin)]
        public ActionResult<GeneralResponse> GetAllPendingDoctorRequests()
        {
            List<DoctorPendingRequest> doctorPendingRequests = unitOfWork.DoctorService.GetAllPendingDoctorRequests();
            return new GeneralResponse()
            {
                Data = doctorPendingRequests,
                IsSuccess = true,
            };
        }

        [HttpPost("accept/{DoctorId}")]
        [Authorize(policy: UserRoles.Admin)]
        public ActionResult<GeneralResponse> AcceptPendingDoctorRequest(int DoctorId)
        {
            GeneralResponse IsAcceptedRequest = unitOfWork.DoctorRepository.AcceptPendingDoctorRequest(DoctorId);
            return IsAcceptedRequest;
        }

        [HttpPost("reject/{DoctorId}")]
        [Authorize(policy: UserRoles.Admin)]
        public ActionResult<GeneralResponse> RejectPendingDoctorRequest(int DoctorId)
        {
            GeneralResponse IsRejectedRequest = unitOfWork.DoctorRepository.RejectPendingDoctorRequest(DoctorId);
            return IsRejectedRequest;

        }

        [HttpGet("GetRole")]
        public async Task<ActionResult<string>> GetUserRoleAsync()
        {
            ClaimsPrincipal user = this.User;
            if(user==null)
            {
                return "Not Found Role";
            }
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(LoggedInUserId==null)
            {
                return "Not Found Role";
            }
            AppUser userFromDB = unitOfWork._userManager.Users.FirstOrDefault(u=>u.Id == LoggedInUserId); 
            if(userFromDB == null)
            {
                return "Not Found Role";
            }
            var roles = await unitOfWork._userManager.GetRolesAsync(userFromDB);
            var LoggedInRole = roles.FirstOrDefault();
            if (LoggedInRole != null)
            {
                return LoggedInRole;
            }
            return "Not Found Role";
        }
    }
}
