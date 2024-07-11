using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZahimarProject.Authentication;
using ZahimarProject.DTOS.DoctorDTO;
using ZahimarProject.DTOS.RelativeDTOs;
using ZahimarProject.Helpers;
using ZahimarProject.Helpers.UnitOfWorkFolder;
using ZahimarProject.Models;

namespace ZahimarProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelativeController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
       

        public RelativeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            

        }


        [Authorize(policy: UserRoles.Doctor)]
        [HttpGet("RelativePhoneNumber")]
        public ActionResult<string> RelativePhoneNumber(int PatientId)
        {      
            Relative relative = unitOfWork.RelativeRepository.Get(r => r.PatientId == PatientId);
            if (relative == null)
            {
                return "Can't Find Phone Number";
                //return new GeneralResponse { IsSuccess = false, Data = "Can't Find Phone Nuber" };
            }
            return relative.PhoneNumber;
            //return new GeneralResponse { IsSuccess = true, Data = relative.PhoneNumber };
        }


        [HttpGet("Relatives")]
        public ActionResult<dynamic> GetAllRelatives()
        {

            List<Relative> relatives = unitOfWork.RelativeRepository.GetAll();
            List<RelativeGetDTO> relativeGetDTOs  = unitOfWork.RelativeService.GetRelativeDTOs(relatives);

            if (relatives == null || relativeGetDTOs.Count == 0)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Foound")
                };
            }
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = relativeGetDTOs
            };
        }


        [HttpGet]
        [Authorize(policy: UserRoles.Relative)]
        public ActionResult<dynamic> GetRelativeForPatientProfilel()
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Relative relative = unitOfWork.RelativeRepository.Get(r => r.AppUserId == LoggedInUserId);
            RelativeGetProfileDTO relativeProfileDTO = unitOfWork.RelativeService.RelativeGetProfile(relative);
            //relativeDTO.GenderString = relativeDTO.Gender == Gender.Male ? "Male" : "Female";

            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = relativeProfileDTO
            };
        }

        [HttpGet("Visited/{PatientId}")]
        [Authorize(policy: UserRoles.Doctor)]
        public ActionResult<dynamic> GetRelativeForPatientVisitedProfilel(int PatientId)
        {
            Relative relative = unitOfWork.RelativeRepository.Get(r => r.PatientId == PatientId);
            RelativeGetProfileDTO relativeProfileDTO = unitOfWork.RelativeService.RelativeGetProfile(relative);
            //relativeDTO.GenderString = relativeDTO.Gender == Gender.Male ? "Male" : "Female";

            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = relativeProfileDTO
            };
        }




        [HttpPut]
        [Authorize(policy: UserRoles.Relative)]
        public async Task<ActionResult<dynamic>> EditReletive(RelativeDTO relativeDTO)
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Relative relative = unitOfWork.RelativeRepository.Get(r => r.AppUserId == LoggedInUserId);
            unitOfWork.RelativeService.GetUpdatedRelative(relativeDTO, relative);
            unitOfWork.RelativeRepository.Update(relative);

            AppUser appUser = await unitOfWork._userManager.FindByIdAsync(LoggedInUserId);
            unitOfWork.RelativeService.GetUpdatedAppUser(relative, appUser);
            var result = await unitOfWork._userManager.UpdateAsync(appUser);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    if (error.Code == "DuplicateUserName")
                    {
                        return new GeneralResponse()
                        {
                            IsSuccess = false,
                            Data = "DuplicateUserName"
                        };
                    }
                }
            }
            unitOfWork.Save();
          
           return new GeneralResponse()
            {
                IsSuccess = true,
                Data = "Updated Successfully"
            };

        }

        


        [HttpDelete]
        [Authorize(policy: UserRoles.Relative)]
        public async Task<ActionResult<dynamic>> DeleteRelativeAndPatient()
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Relative relative = unitOfWork.RelativeRepository.Get(r => r.AppUserId == LoggedInUserId);
            Patient patient = unitOfWork.PatientRepository.GetPatient(LoggedInUserId);
            AppUser appUser = await unitOfWork._userManager.FindByIdAsync(LoggedInUserId);
           

            relative.IsDeleted = true;
            patient.IsDeleted = true;
            await unitOfWork._signInManager.SignOutAsync();
            unitOfWork.RelativeRepository.Delete(relative);
            unitOfWork.PatientRepository.Delete(patient);
            //unitOfWork.RelativeRepository.Delete(appUser);
            unitOfWork.Save();

            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = "Deleted Successfully"
            };
        }
    }
}