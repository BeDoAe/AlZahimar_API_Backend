using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Net;
using System.Security.Claims;
using ZahimarProject.Authentication;
using ZahimarProject.DTOS.PatientDTOs;
using ZahimarProject.DTOS.RelativeDTOs;
using ZahimarProject.Helpers;
using ZahimarProject.Helpers.UnitOfWorkFolder;
using ZahimarProject.Models;
using ZahimarProject.Repos.PatientRepo;
using ZahimarProject.Services.PatientServices;

namespace ZahimarProject.Controllers
{
    //[Authorize(Roles = UserRoles.Relative)]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        public PatientController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;

        }
        Patient GetPatient()
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Patient patient = unitOfWork.PatientRepository.GetPatient(LoggedInUserId);
            return patient;
        }

        [HttpGet]
        [Authorize(Policy = UserRoles.Relative)]
        public ActionResult<dynamic> Get()
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Patient patient = GetPatient();

            if (LoggedInUserId == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = "Invalid User"
                };
            }
            PatientDTO PatientForGetDTO = unitOfWork.patientService.PatientForGetDTO(patient);


            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = PatientForGetDTO
            };
        }

        //[HttpGet("GetForPatientProfile")]
        //[Authorize(Policy = UserRoles.Relative)]
        //public ActionResult<dynamic> GetForPatientProfile()
        //{
        //    ClaimsPrincipal user = this.User;
        //    string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    Patient patient = GetPatient();

        //    if (LoggedInUserId == null)
        //    {
        //        return new GeneralResponse()
        //        {
        //            IsSuccess = false,
        //            Data = "Invalid User"
        //        };
        //    }
        //    PatientProfileDTO patientProfileDTO = unitOfWork.patientService.PatientForProfiletDTO(patient);


        //    return new GeneralResponse()
        //    {
        //        IsSuccess = true,
        //        Data = PatientForGetDTO
        //    };
        //}


        [HttpPut]
        [Authorize(Policy = UserRoles.Relative)]
        public ActionResult<dynamic> Edit(PatientDTO patientDTO)
        {
            if (ModelState.IsValid)
            {
                Patient patient = GetPatient();

                unitOfWork.patientService.GetUpdetedPatient(patientDTO, patient);
                unitOfWork.PatientRepository.Update(patient);
                unitOfWork.Save();

                return new GeneralResponse()
                {
                    IsSuccess = true,
                    Data = "patient Updated Successfully"
                };
            }
            else
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = ModelState
                };
            }
        }

        [HttpPut("UpdatePhoto")]
        [Authorize(policy: UserRoles.Relative)]
        public async Task<ActionResult<GeneralResponse>> UpdatePhoto([FromForm] UploadPatientImageDTO uploadPatientImageDTO)
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Relative relative = unitOfWork.RelativeRepository.Get(r => r.AppUserId == LoggedInUserId);
            Patient patient = relative.Patient;
            string PatientPicURL = await unitOfWork.PatientRepository.UpdatePatientPhoto(uploadPatientImageDTO, patient);

            if (string.IsNullOrEmpty(PatientPicURL))
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = "Failed to update photo"
                };
            }

            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = PatientPicURL
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
            unitOfWork.Save();

            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = "Deleted Successfully"
            };

        }

        // TODO : i want make this endpoint to access by admin
        [HttpGet("PatientsCount")]
        [Authorize(Roles = "Admin")]
        public ActionResult<GeneralResponse> GetPatientsCount()
        {
            int? count = unitOfWork.PatientRepository.GetAll().Count;
            if (count == null)
            {
                return new GeneralResponse() { IsSuccess = false, Data = "No patients yet" };
            }
            return new GeneralResponse() { IsSuccess = true, Data = count };

        }


    }
}