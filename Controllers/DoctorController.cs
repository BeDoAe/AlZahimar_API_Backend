using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using ZahimarProject.Authentication;
using ZahimarProject.DTOS.AppointmentDTOs;
using ZahimarProject.DTOS.DoctorDTO;
using ZahimarProject.DTOS.PatientDTOs;
using ZahimarProject.DTOS.RelativeDTOs;
using ZahimarProject.Helpers;
using ZahimarProject.Helpers.UnitOfWorkFolder;
using ZahimarProject.Models;
using ZahimarProject.Services.DoctorServices;

namespace ZahimarProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public DoctorController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

      private Doctor GetLogedInDoctor()
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Doctor doctor = unitOfWork.DoctorRepository.Get(r => r.AppUserId == LoggedInUserId);
            return doctor;
        }
        [HttpGet("DoctorPhoneNumber")]
        [Authorize(policy: UserRoles.Relative)]
        public ActionResult<string> DoctorPhoneNumber(int DoctorId)
        {
           
            Doctor doctor = unitOfWork.DoctorRepository.Get(r => r.Id == DoctorId);
           
            if (doctor == null)
            {
                return "Can't Find Phone Number";
                //return new GeneralResponse { IsSuccess = false, Data = "Can't Find Phone Nuber" };
            }
            return doctor.Phone;
            //return new GeneralResponse { IsSuccess = true, Data = doctor.Phone };
        }

        [HttpGet("MyDoctorPhoneNumber")]
        [Authorize(policy: UserRoles.Relative)]
        public ActionResult<string> MyDoctorPhoneNumber()
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Relative relative = unitOfWork.RelativeRepository.Get(r => r.AppUserId == LoggedInUserId);
            Doctor doctor = unitOfWork.DoctorRepository.Get(r => r.Id == relative.Patient.DoctorId);

            if (doctor == null)
            {
                return "Can't Find Phone Number";
                //return new GeneralResponse { IsSuccess = false, Data = "Can't Find Phone Nuber" };
            }
            return doctor.Phone;
            //return new GeneralResponse { IsSuccess = true, Data = doctor.Phone };
        }


        [HttpGet]
        [Authorize(policy: UserRoles.Doctor)]
        public ActionResult<dynamic> GetDoctor()
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Doctor doctor = unitOfWork.DoctorRepository.Get(r => r.AppUserId == LoggedInUserId);
            DoctorGetDTO doctorDTO  = unitOfWork.DoctorService.GetDoctorDTO(doctor);
            doctorDTO.GenderString = doctorDTO.Gender == Gender.Male ? "Male" : "Female";

            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = doctorDTO
            };
        }


        [HttpGet("GetDoctor")]
        [Authorize(policy: UserRoles.Doctor)]
        public ActionResult<dynamic> GetDoctorId()
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Doctor doctor = unitOfWork.DoctorRepository.Get(r => r.AppUserId == LoggedInUserId);         

            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = doctor.Id
            };
        }

        [HttpGet("doctorId")]
        //[Authorize(policy: UserRoles.Relative)]
        public ActionResult<dynamic> GetDoctorById(int doctorId)
        {
           
            Doctor doctor = unitOfWork.DoctorRepository.Get(r => r.Id == doctorId);
            if(doctor==null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            DoctorGetDTO doctorDTO = unitOfWork.DoctorService.GetDoctorDTO(doctor);
            doctorDTO.GenderString = doctorDTO.Gender == Gender.Male ? "Male" : "Female";

            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = doctorDTO
            };
        }




        [HttpGet("Doctors")]
        [Authorize(policy: UserRoles.Relative)]
        public ActionResult<dynamic> GetAllDoctors()
        {

            List<Doctor> doctors = unitOfWork.DoctorRepository.GetAll();
            List<FilterDoctorDTO> doctorDTOs = unitOfWork.DoctorService.GetFilteredDoctorsDTO(doctors);

            if(doctors == null|| doctorDTOs.Count==0)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = doctorDTOs
            };
        }


        [HttpGet("PendingDoctors")]
        [Authorize(policy: UserRoles.Admin)]
        public ActionResult<dynamic> GetPendingDoctors()
        {

            List<Doctor> doctors = unitOfWork.DoctorRepository.GetAllPendingDoctors();
            List<SearchDoctorDTO> doctorDTOs = unitOfWork.DoctorService.GetPendingDoctorsDTO(doctors);

            if (doctors == null || doctorDTOs.Count == 0)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = doctorDTOs
            };
        }

        [HttpGet("Doctor/{Name}")]
        [Authorize(policy: UserRoles.Relative)]

        public ActionResult<dynamic> SearchDoctor(string Name)
        {
            List<Doctor> doctors = unitOfWork.DoctorRepository.SearchForDoctor(Name);
            if (doctors == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            List<FilterDoctorDTO> ResultSearchDoctors = unitOfWork.DoctorService.GetSearchResult(doctors);
            if (ResultSearchDoctors == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
          
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = ResultSearchDoctors
            };
        }

        [Authorize(policy: UserRoles.Relative)]
        [HttpGet("RandomDoctor")]
        public ActionResult<dynamic> GetRandomDoctors()
        {
            int NumberOfDctors = 2;
            List<Doctor> allDoctors = unitOfWork.DoctorRepository.GetAll().ToList();

            if (allDoctors == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            List<Doctor> randomDoctors = unitOfWork.DoctorRepository.GetRandomDoctors(allDoctors , NumberOfDctors);
            if (randomDoctors == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            List<RandomDoctorDTO> randomDoctorDTOs = unitOfWork.DoctorService.GetRandomDoctorsDTO(randomDoctors);
            if (randomDoctors == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = randomDoctorDTOs
            };
        }

        [HttpGet("FilteredDoctorByPrice")]
        [Authorize(policy: UserRoles.Relative)]
        public ActionResult<dynamic> GetFilteredDoctorsByPrice()
        {
            List<Doctor> allDoctors = unitOfWork.DoctorRepository.GetAll().ToList();

            if (allDoctors == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            List<Doctor> filteredDoctors = unitOfWork.DoctorRepository.GetFilteredDoctorsByPrice(allDoctors);
            if (filteredDoctors == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            List<FilterDoctorDTO> filteredDoctorsDTOs = unitOfWork.DoctorService.GetFilteredDoctorsDTO(filteredDoctors);
            if (filteredDoctorsDTOs == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = filteredDoctorsDTOs
            };
        }


        [Authorize(policy: UserRoles.Relative)]
        [HttpGet("FilteredDoctorByAverageRate")]
        public ActionResult<dynamic> GetFilteredDoctorsByAverargeRate()
        {
            List<Doctor> allDoctors = unitOfWork.DoctorRepository.GetAll().ToList();

            if (allDoctors == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            List<Doctor> filteredDoctors = unitOfWork.DoctorRepository.GetFilteredDoctorsByAverageRating(allDoctors);
            if (filteredDoctors == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            List<FilterDoctorDTO> filteredDoctorsDTOs = unitOfWork.DoctorService.GetFilteredDoctorsDTO(filteredDoctors);
            if (filteredDoctorsDTOs == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = filteredDoctorsDTOs
            };
        }


        [Authorize(policy: UserRoles.Relative)]
        [HttpGet("FilteredDoctorBySpecificAverageRate")]
        public ActionResult<dynamic> GetFilteredDoctorsBySpecificAverargeRate(int AvgRate=5)
        {
            List<Doctor> allDoctors = unitOfWork.DoctorRepository.GetAll().ToList();

            if (allDoctors == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            List<Doctor> filteredDoctors = unitOfWork.DoctorRepository.GetFilteredDoctorsBySpecificAverageRating(allDoctors, AvgRate);
            if (filteredDoctors == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            List<FilterDoctorDTO> filteredDoctorsDTOs = unitOfWork.DoctorService.GetFilteredDoctorsDTO(filteredDoctors);
            if (filteredDoctorsDTOs == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = filteredDoctorsDTOs
            };
        }


        //[Authorize(policy: UserRoles.Relative)]
        [HttpGet("TopRated")]
        public ActionResult<dynamic> GetTopAverageRateDoctors()
        {
            List<Doctor> allDoctors = unitOfWork.DoctorRepository.GetAll().ToList();

            if (allDoctors == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            List<Doctor> filteredDoctors = unitOfWork.DoctorRepository.GetTopRatedDoctors(allDoctors, 9);
            if (filteredDoctors == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            List<FilterDoctorDTO> filteredDoctorsDTOs = unitOfWork.DoctorService.GetFilteredDoctorsDTO(filteredDoctors);
            if (filteredDoctorsDTOs == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = filteredDoctorsDTOs
            };
        }



        [HttpGet("FilteredMaleDoctor")]
        [Authorize(policy: UserRoles.Relative)]
        public ActionResult<dynamic> GetFilteredMaleDoctors()
        {
            List<Doctor> allDoctors = unitOfWork.DoctorRepository.GetAll().ToList();

            if (allDoctors == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            List<Doctor> filteredDoctors = unitOfWork.DoctorRepository.GetFilteredMaleDoctors(allDoctors);
            if (filteredDoctors == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            List<FilterDoctorDTO> filteredDoctorsDTOs = unitOfWork.DoctorService.GetFilteredDoctorsDTO(filteredDoctors);
            if (filteredDoctorsDTOs == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = filteredDoctorsDTOs
            };
        }

        [Authorize(policy: UserRoles.Relative)]
        [HttpGet("FilteredFemaleDoctor")]
        public ActionResult<dynamic> GetFilteredFemaleDoctors()
        {
            List<Doctor> allDoctors = unitOfWork.DoctorRepository.GetAll().ToList();

            if (allDoctors == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            List<Doctor> filteredDoctors = unitOfWork.DoctorRepository.GetFilteredFemaleDoctors(allDoctors);
            if (filteredDoctors == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            List<FilterDoctorDTO> filteredDoctorsDTOs = unitOfWork.DoctorService.GetFilteredDoctorsDTO(filteredDoctors);
            if (filteredDoctorsDTOs == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = filteredDoctorsDTOs
            };
        }

        [HttpGet("FilteredDoctorsByAge")]
        [Authorize(policy: UserRoles.Relative)]
        public ActionResult<dynamic> GetFilteredDoctorsByAge()
        {
            List<Doctor> allDoctors = unitOfWork.DoctorRepository.GetAll().ToList();

            if (allDoctors == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            List<Doctor> filteredDoctors = unitOfWork.DoctorRepository.GetFilteredDoctorsByAge(allDoctors);
            if (filteredDoctors == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            List<FilterDoctorDTO> filteredDoctorsDTOs = unitOfWork.DoctorService.GetFilteredDoctorsDTO(filteredDoctors);
            if (filteredDoctorsDTOs == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = filteredDoctorsDTOs
            };
        }

        [HttpGet("DoctorsByAgeGreaterThan{Age}")]
        [Authorize(policy: UserRoles.Relative)]
        public ActionResult<dynamic> GetDoctorsByAgeGreaterThan(int Age)
        {
            List<Doctor> allDoctors = unitOfWork.DoctorRepository.GetAll().ToList();

            if (allDoctors == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            List<Doctor> filteredDoctors = unitOfWork.DoctorRepository.GetDoctorsByAgeGreaterThan(allDoctors,Age);
            if (filteredDoctors == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            List<FilterDoctorDTO> filteredDoctorsDTOs = unitOfWork.DoctorService.GetFilteredDoctorsDTO(filteredDoctors);
            if (filteredDoctorsDTOs == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = filteredDoctorsDTOs
            };
        }

        [HttpGet("DoctorsByPriceRange")]
        [Authorize(policy: UserRoles.Relative)]
        public ActionResult<dynamic> GetDoctorsByPriceRange(int EndRange=500)
        {
            List<Doctor> allDoctors = unitOfWork.DoctorRepository.GetAll().ToList();
            if (allDoctors == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            List<Doctor> filteredDoctors = unitOfWork.DoctorRepository.GetDoctorsByPriceRange(allDoctors, EndRange );
            if (filteredDoctors == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            List<FilterDoctorDTO> filteredDoctorsDTOs = unitOfWork.DoctorService.GetFilteredDoctorsDTO(filteredDoctors);
            if (filteredDoctorsDTOs == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = filteredDoctorsDTOs
            };
        }


        [HttpGet("DoctorsByAgeSmallerThan{Age}")]
        [Authorize(policy: UserRoles.Relative)]
        public ActionResult<dynamic> GetDoctorsByAgeSmallerThan(int Age)
        {
            List<Doctor> allDoctors = unitOfWork.DoctorRepository.GetAll().ToList();

            if (allDoctors == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            List<Doctor> filteredDoctors = unitOfWork.DoctorRepository.GetDoctorsByAgeSmallerThan(allDoctors, Age);
            if (filteredDoctors == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            List<FilterDoctorDTO> filteredDoctorsDTOs = unitOfWork.DoctorService.GetFilteredDoctorsDTO(filteredDoctors);
            if (filteredDoctorsDTOs == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = filteredDoctorsDTOs
            };
        }






        Doctor GetLoggedInDoctor()
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Doctor doctor = unitOfWork.DoctorRepository.Get(r => r.AppUserId == LoggedInUserId);
            return doctor;
        }

        [HttpPut]
        [Authorize(policy: UserRoles.Doctor)]
        public async Task<ActionResult<dynamic>> EditDoctor(DoctorDTO doctorDTO)
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Doctor doctor = GetLoggedInDoctor();//unitOfWork.DoctorRepository.Get(r => r.AppUserId == LoggedInUserId);
            unitOfWork.DoctorService.GetUpdatedDoctor(doctorDTO, doctor);
            unitOfWork.DoctorRepository.Update(doctor);

            AppUser appUser = await unitOfWork._userManager.FindByIdAsync(LoggedInUserId);
            unitOfWork.DoctorService.GetUpdatedAppUser(doctor, appUser);
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


        [HttpPut("UpdatePhoto")]
        [Authorize(policy: UserRoles.Doctor)]
        public async Task<ActionResult<GeneralResponse>> UpdatePhoto([FromForm] UploadImageDTO uploadImageDTO)
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Doctor doctor = GetLoggedInDoctor(); // unitOfWork.DoctorRepository.Get(r => r.AppUserId == LoggedInUserId);

            string picURL = await unitOfWork.DoctorRepository.UpdateDoctorPhoto(uploadImageDTO, doctor);

            if (string.IsNullOrEmpty(picURL))
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
                Data = picURL
            };
        }



        [HttpGet("DoctorsCount")]
        [Authorize(Roles = "Admin")]
        public ActionResult<GeneralResponse> GetDoctorsCount()
        {
            int? count = unitOfWork.DoctorRepository.GetAcceptedDoctorsCount();
            if (count == null)
            {
                return new GeneralResponse() { IsSuccess = false, Data = "No Doctors yet" };
            }
            return new GeneralResponse() { IsSuccess = true, Data = count };

        }


        [Authorize(policy: UserRoles.Doctor)]
        [HttpGet("DoctorStatistics")]
        public ActionResult<dynamic> GetPatientsOfDoctor()
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Doctor doctor = unitOfWork.DoctorRepository.Get(r => r.AppUserId == LoggedInUserId);
            if (doctor != null)
            {
               // DoctorDashBoardDto doctorDashBoardDto = new DoctorDashBoardDto();
                 int NumberOfPatients  = unitOfWork.PatientDoctorRequestService.GetAllPatientsOfDoctor(doctor.Id).Count;
                 //doctorDashBoardDto.NumberOfAvailableAppointments    

                return new GeneralResponse()
                {
                    IsSuccess = true,
                    Data = NumberOfPatients,
                };
            }
            else
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Request Not Found"),
                };
            }

        }

        [Authorize(policy: UserRoles.Doctor)]
        [HttpPut("EditStartEndWork")]
        public ActionResult<GeneralResponse>  EditStartEndWork(DoctorToEditStartEndAppointmentDTO doctorToEdit)
        {
            if (ModelState.IsValid)
            {
                Doctor doctor = GetLogedInDoctor();
                GeneralResponse generalResponse = unitOfWork.DoctorService.EditStartEndWorkOfDoctor(doctor.Id, doctorToEdit);
                if (generalResponse.IsSuccess)
                {
                    unitOfWork.Save();
                    return generalResponse;
                }
                return generalResponse;
            }

            return new GeneralResponse()
            {
                IsSuccess = false,
                Data = ModelState
            };

        }

        [Authorize(policy: UserRoles.Doctor)]
        [HttpPut("StartEndDuratin")]
        public ActionResult<GeneralResponse> EditStartEndDuratin(AppointmentStartEndDurationDTO appointmentStartEndDurationDTO)
        {
            GeneralResponse generalResponse = new GeneralResponse();
            if (ModelState.IsValid)
            {
                Doctor doctor = GetLogedInDoctor();
                 generalResponse = unitOfWork.DoctorService.EditDotorStartEndDuration(doctor.Id, appointmentStartEndDurationDTO);
                if (generalResponse.IsSuccess)
                {
                    unitOfWork.Save();
                    return generalResponse;
                }
                return generalResponse;
            }
            generalResponse.Data = ModelState;
            generalResponse.IsSuccess = false;
            return generalResponse;
         
        }

        [Authorize(policy: UserRoles.Relative)]
        [HttpGet("WorkAppointmentOfDoctor")]
        public ActionResult<GeneralResponse> GetWorkAppointmentOfDoctor(int DoctorId)
        {
            GeneralResponse generalResponse = unitOfWork.DoctorService.GetDotorStartEndDuration(DoctorId);
            if (generalResponse.IsSuccess)
            {
                unitOfWork.Save();
                return generalResponse;
            }


            return new GeneralResponse()
            {
                IsSuccess = false,
                Data = "Not Found"
            };

        }

        [Authorize(policy: UserRoles.Doctor)]
        [HttpGet("WorkAppointmentOfLoggedInDoctor")]
        public ActionResult<GeneralResponse> GetWorkAppointmentOfLoggedInDoctor()
        {
            Doctor doctor = GetLogedInDoctor();

            GeneralResponse generalResponse = unitOfWork.DoctorService.GetDotorStartEndDuration(doctor.Id);
            if (generalResponse.IsSuccess)
            {
                unitOfWork.Save();
                return generalResponse;
            }


            return new GeneralResponse()
            {
                IsSuccess = false,
                Data = "Not Found"
            };

        }

        [Authorize(policy: UserRoles.Relative)]
        [HttpGet("DoctorEmail")]
        public ActionResult<GeneralResponse> GetDoctorEmail(int DoctorId)
        {
            Doctor AccessedDoctor = unitOfWork.DoctorRepository.Get(d => d.Id == DoctorId);
            if (AccessedDoctor == null)
            {

                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = "Not Found"
                };
            }
            Doctor Doctor = unitOfWork.DoctorRepository.Get(d => d.AppUser.Email == AccessedDoctor.AppUser.Email);
            if (Doctor == null)
            {

                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = "Not Found"
                };
            }
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = Doctor.AppUser.Email
            };

        }
        

    }
}
