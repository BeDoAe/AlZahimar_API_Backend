using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZahimarProject.Authentication;
using ZahimarProject.DTOS.DoctorDTO;
using ZahimarProject.DTOS.RatingDTOs;
using ZahimarProject.Helpers;
using ZahimarProject.Helpers.UnitOfWorkFolder;
using ZahimarProject.Models;

namespace ZahimarProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public RatingController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        private int GetLoggedInRelativeId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var relative = unitOfWork.RelativeRepository.Get(r => r.AppUserId == userId);
            return relative.Id;
        }

        private Relative GetLoggedInRelative()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var relative = unitOfWork.RelativeRepository.Get(r => r.AppUserId == userId);
            return relative;
        }

        [HttpPost]
        [Authorize(policy: UserRoles.Relative)]
        public ActionResult<dynamic> AddRating(AddRatingDTO addRatingDTO)
        {
            if (ModelState.IsValid)
            {
                int relativeId = GetLoggedInRelativeId();
                Relative relative = GetLoggedInRelative();
                if (relative.Patient.DoctorId == addRatingDTO.DoctorId)
                {
                    Rating rating = unitOfWork.RatingService.AddRaiting(addRatingDTO, relativeId);
                    if (rating == null)
                    {
                        return new GeneralResponse
                        {
                            IsSuccess = false,
                            Data = NotFound("Not Found")
                        };
                    }
                    unitOfWork.RatingRepository.Insert(rating);
                    unitOfWork.Save();

                    // Update average rating of the doctor
                    var ratings = unitOfWork.RatingRepository.GetRatingsOfDoctor(addRatingDTO.DoctorId);
                    double averageRating = unitOfWork.RatingService.GetAverageRatingForDoctor(ratings);
                    var doctor = unitOfWork.DoctorRepository.Get(d => d.Id == addRatingDTO.DoctorId);
                    doctor.AverageRating = averageRating;
                    unitOfWork.DoctorRepository.Update(doctor);
                    unitOfWork.Save();

                    return new GeneralResponse
                    {
                        IsSuccess = true,
                        Data = "Rating Added Successfully"
                    };
                }
                return new GeneralResponse
                {
                    IsSuccess = false,
                    Data = "Unauthorized To Give Rate"
                };

            }
            return new GeneralResponse
            {
                IsSuccess = false,
                Data = ModelState
            };


        }

        [HttpGet("AuthorizedToRate")]
        [Authorize(policy: UserRoles.Relative)]
        public ActionResult<GeneralResponse> AuthorizedToRate(int doctorId )
        {
            int relativeId = GetLoggedInRelativeId();
            Relative relative = GetLoggedInRelative();
            if (relative.Patient.DoctorId != doctorId)
            {
                return new GeneralResponse
                {
                    IsSuccess = false,
                    Data = "Unauthorized To Give Rate"
                };
            }
            return new GeneralResponse
            {
                IsSuccess = true,
                Data = "Authorized To Give Rate"
            };
        }


        [HttpGet("DoctorRatings/{doctorId}")]
        public ActionResult<GeneralResponse> GetRatingsForDoctor(int doctorId)
        {
            var ratings = unitOfWork.RatingRepository.GetRatingsOfDoctor(doctorId);
            var AllRatings = unitOfWork.RatingService.GetRatingsForDoctor(ratings);
            return new GeneralResponse { IsSuccess = true, Data = AllRatings };
        }


        [HttpGet("Average/{doctorId}")]
        public ActionResult<GeneralResponse> GetAverageRatingForDoctor(int doctorId)
        {
            Doctor doctor = unitOfWork.DoctorRepository.Get(d => d.Id == doctorId);
            if (doctor == null)
            {
                return new GeneralResponse
                {
                    IsSuccess = false,
                    Data = NotFound("Doctor not found")
                };
            }

            double? averageRating = doctor.AverageRating;
            if (averageRating.HasValue)
            {
                averageRating = Math.Round(averageRating.Value, 1);
            }

            return new GeneralResponse
            {
                IsSuccess = true,
                Data = averageRating
            };
        }

        //[HttpGet("maxRatings")]
        //public ActionResult<GeneralResponse> GetMaxAverageRating(int TopAverages)
        //{
        //    List<Doctor> doctors = unitOfWork.RatingRepository.GetTopAverageRatingDoctors();
        //    if(doctors == null)
        //    {
        //        return new GeneralResponse() { IsSuccess = false, Data = "No Doctors Have Rating" };
        //    }
        //    List<DoctorGetDTO> doctorsDto = unitOfWork.DoctorService.GetDoctorDTOs(doctors);
        //    return new GeneralResponse() { IsSuccess = true, Data =  doctorsDto };  
        //}
    }
}
