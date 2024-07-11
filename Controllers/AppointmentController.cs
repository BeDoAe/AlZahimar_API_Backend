using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZahimarProject.DTOS.AppointmentDTOs;
using ZahimarProject.Helpers;
using ZahimarProject.Helpers.UnitOfWorkFolder;
using ZahimarProject.Models;

namespace ZahimarProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public AppointmentController(IUnitOfWork unitOfWork )
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

        Patient GetPatient()
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Patient patient = unitOfWork.PatientRepository.GetPatient(LoggedInUserId);
            return patient;
        }
        int GetDoctorId()
        {
            Patient patient = GetPatient();
            return (int)patient.DoctorId;
        }
        Doctor GetLoggedInDoctor()
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Doctor doctor = unitOfWork.DoctorRepository.Get(r => r.AppUserId == LoggedInUserId);
            return doctor;
        }

        [HttpPost]
        [Authorize(policy: UserRoles.Relative)]
        public ActionResult<dynamic> AddRequest(RequestAppointmentDTO requestAppointmentDTO)
        {
            if (ModelState.IsValid)
            {
                Patient patient = GetPatient();
                GeneralResponse response = unitOfWork.AppointmentService.AddAppointmentRequest(requestAppointmentDTO, patient.Id, patient.DoctorId);
                if (response.IsSuccess)
                {
                    unitOfWork.Save();
                    return response;
                }
                return response;
            }
            else
            {
                return new GeneralResponse { IsSuccess = false, Data = ModelState };
            }
        }

        [Authorize(Policy = UserRoles.Doctor)]
        [HttpPost("accept/{appointmentId}")]
        public ActionResult<dynamic> AcceptAppointment(int appointmentId)
        {
            bool IsAccepted = unitOfWork.AppointmentService.AcceptAppointment(appointmentId);
            if (IsAccepted)
            {
                unitOfWork.Save();
                

                return new GeneralResponse { IsSuccess = true, Data = "Appointment accepted successfully." };
            }
            else
            {
                return new GeneralResponse { IsSuccess = false, Data = "Failed to accept the appointment." };
            }
        }

        [Authorize(Policy = UserRoles.Doctor)]
        [HttpPost("reject/{appointmentId}")]
        public ActionResult<dynamic> RejectAppointment(int appointmentId)
        {
            var IsRejected = unitOfWork.AppointmentService.RejectAppointment(appointmentId);
            if (IsRejected)
            {
                unitOfWork.Save();
                return new GeneralResponse { IsSuccess = true, Data = "Opps.. Appointment rejected" };

            }
            else
            {
                return new GeneralResponse { IsSuccess = false, Data = "Failed to reject the appointment." };
            }
        }

        [Authorize(Policy = UserRoles.Doctor)]
        [HttpPost("complete/{appointmentId}")]
        public ActionResult<dynamic> CompleteAppointment(int appointmentId)
        {
            bool IsCompleted = unitOfWork.AppointmentService.CompleteAppointment(appointmentId);
            if (IsCompleted)
            {
                unitOfWork.Save();
                return new GeneralResponse { IsSuccess = true, Data = "Appointment completed successfully." };
            }
            else
            {
                return new GeneralResponse { IsSuccess = true, Data = "Failed to complete the appointment." };
            }
        }

        [HttpGet("AppointmentsCount")]
        [Authorize(Roles = "Admin")]
        public ActionResult<GeneralResponse> GetAppiontmentsCount()
        {
            int? count = unitOfWork.AppointmentRepository.GetAll().Count;
            if (count == null)
            {
                return new GeneralResponse() { IsSuccess = false, Data = "No Appointments yet" };
            }
            return new GeneralResponse() { IsSuccess = true, Data = count };
        }

        [Authorize(Policy = UserRoles.Doctor)]
        [HttpGet("Pending")]
        public ActionResult<GeneralResponse> GetAllPending()
        {
            Doctor doctor=GetLoggedInDoctor();
            GeneralResponse generalResponse = unitOfWork.AppointmentService.GetAllPending(doctor.Id);
            return generalResponse;
        }

        [Authorize(Policy = UserRoles.Doctor)]
        [HttpGet("Accepted")]
        public ActionResult<GeneralResponse> GetAllAccepted()
        {
            Doctor doctor = GetLoggedInDoctor();
            GeneralResponse generalResponse = unitOfWork.AppointmentService.GetAllAccepted(doctor.Id);
            return generalResponse;
        }   

        [Authorize(Policy = UserRoles.Doctor)]
        [HttpGet("Rejected")]
        public ActionResult<GeneralResponse> GetAllRejected()
        {
            Doctor doctor = GetLoggedInDoctor();
            GeneralResponse generalResponse = unitOfWork.AppointmentService.GetAllRejected(doctor.Id);
            return generalResponse;
        }

        [Authorize(Policy = UserRoles.Doctor)]
        [HttpGet("Completed")]
        public ActionResult<GeneralResponse> GetAllCompleted()
        {
            Doctor doctor = GetLoggedInDoctor();
            GeneralResponse generalResponse = unitOfWork.AppointmentService.GetAllCompleted(doctor.Id);
            return generalResponse;
        }

        [Authorize(Policy = UserRoles.Doctor)]
        [HttpGet("Deleted")]
        public ActionResult<GeneralResponse> GetAllDeleted()
        {
            Doctor doctor = GetLoggedInDoctor();
            GeneralResponse generalResponse = unitOfWork.AppointmentService.GetAllDeleted(doctor.Id);
            return generalResponse;
        }


        [Authorize(Policy = UserRoles.Doctor)]
        [HttpGet("AppointmentsCountOfDoctor")]
        public ActionResult<GeneralResponse> GetAllAppointmentsCountOfDoctor()
        {
            Doctor doctor = GetLoggedInDoctor();
            GeneralResponse generalResponse = unitOfWork.AppointmentService.GetAllAppointmentsCountOfDoctor(doctor.Id);
            return generalResponse;
        }

        [Authorize(Policy = UserRoles.Doctor)]
        [HttpGet("PendingAppointmentsCountOfDoctor")]
        public ActionResult<GeneralResponse> GetPendingAppointmentsCountOfDoctor()
        {
            Doctor doctor = GetLoggedInDoctor();
            GeneralResponse generalResponse = unitOfWork.AppointmentService.GetPendingAppointmentsCountOfDoctor(doctor.Id);
            return generalResponse;
        }


        [Authorize(Policy = UserRoles.Doctor)]
        [HttpGet("AcceptedAppointmentsCountOfDoctor")]
        public ActionResult<GeneralResponse> GetAcceptedAppointmentsCountOfDoctor()
        {
            Doctor doctor = GetLoggedInDoctor();
            GeneralResponse generalResponse = unitOfWork.AppointmentService.GetAcceptedAppointmentsCountOfDoctor(doctor.Id);
            return generalResponse;
        }


        [Authorize(Policy = UserRoles.Doctor)]
        [HttpGet("RejectedAppointmentsCountOfDoctor")]
        public ActionResult<GeneralResponse> GetRejctedAppointmentsCountOfDoctor()
        {
            Doctor doctor = GetLoggedInDoctor();
            GeneralResponse generalResponse = unitOfWork.AppointmentService.GetRejectedAppointmentsCountOfDoctor(doctor.Id);
            return generalResponse;
        }


        [Authorize(Policy = UserRoles.Doctor)]
        [HttpGet("CompletedAppointmentsCountOfDoctor")]
        public ActionResult<GeneralResponse> GetCompletedAppointmentsCountOfDoctor()
        {
            Doctor doctor = GetLoggedInDoctor();
            GeneralResponse generalResponse = unitOfWork.AppointmentService.GetCompletedAppointmentsCountOfDoctor(doctor.Id);
            return generalResponse;
        }


        [Authorize(Policy = UserRoles.Doctor)]
        [HttpGet("DeletedAppointmentsCountOfDoctor")]
        public ActionResult<GeneralResponse> GetDeletedAppointmentsCountOfDoctor()
        {
            Doctor doctor = GetLoggedInDoctor();
            GeneralResponse generalResponse = unitOfWork.AppointmentService.GetDeletedAppointmentsCountOfDoctor(doctor.Id);
            return generalResponse;
        }

        [Authorize(Policy = UserRoles.Doctor)]
        [HttpGet("PreviousAppointmentsOfToday")]
        public ActionResult<GeneralResponse> GetPreviousAppointmentsOfToday()
        {
            GeneralResponse generalResponse = unitOfWork.AppointmentService.GetPreviousAppointmentsOfToday();
            return generalResponse;
        }

        [Authorize(Policy = UserRoles.Doctor)]
        [HttpGet("AfterAppointmentsOfToday")]
        public ActionResult<GeneralResponse> GetAfterAppointmentsOfToday()
        {
            GeneralResponse generalResponse = unitOfWork.AppointmentService.GetAfterAppointmentsOfToday();
            return generalResponse;
        }

        [Authorize(Policy = UserRoles.Doctor)]
        [HttpGet("AppointmentsOfToday")]
        public ActionResult<GeneralResponse> GetAppointmentsOfToday()
        {
            GeneralResponse generalResponse = unitOfWork.AppointmentService.GetAppointmentsOfToday();
            return generalResponse;
        }

        [Authorize(Policy = UserRoles.Relative)]
        [HttpGet("AfterAppointmentsOfTodayOfPatient")]
        public ActionResult<GeneralResponse> GetAfterAppointmentsOfTodayOfPatient()
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Relative relative = unitOfWork.RelativeRepository.Get(r => r.AppUserId == LoggedInUserId);
            GeneralResponse generalResponse = unitOfWork.AppointmentService.GetAfterAppointmentsOfTodayOfPatient(relative.PatientId);
            return generalResponse;
        }


        [Authorize(Policy = UserRoles.Doctor)]
        [HttpDelete("DeleteAppointment")]
        public ActionResult<GeneralResponse> DeleteAppointment(string appointmentDate, string appointmentTime)
        {
            if (ModelState.IsValid)
            {
                if (DateTime.TryParse(appointmentDate, out DateTime parsedDate) && TimeSpan.TryParse(appointmentTime, out TimeSpan parsedTime))
                {
                    Doctor doctor = GetLoggedInDoctor();
                    AppointmentDateAndTimeDTO appointmentDateAndTimeDTO = new AppointmentDateAndTimeDTO
                    {
                        date = parsedDate,
                        time = parsedTime
                    };
                    GeneralResponse generalResponse = unitOfWork.AppointmentService.DeleteAppointment(doctor.Id, appointmentDateAndTimeDTO);
                    if (generalResponse.IsSuccess)
                    {
                        unitOfWork.Save();
                        return generalResponse;
                    }
                    return generalResponse;
                }
                else
                {
                    return new GeneralResponse()
                    {
                        IsSuccess = false,
                        Data = "Invalid date or time format."
                    };
                }
            }

            return new GeneralResponse()
            {
                IsSuccess = false,
                Data = ModelState,
            };
        }



        [Authorize(Policy = UserRoles.Doctor)]
        [HttpDelete("DeleteAppointmentById")]
        public ActionResult<GeneralResponse> DeleteAppointmentById(int appointmentId)
        {
            if (ModelState.IsValid)
            {
                Doctor doctor = GetLoggedInDoctor();
                GeneralResponse generalResponse = unitOfWork.AppointmentService.DeleteAppointmentById(appointmentId);
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
                Data = ModelState,
            };

        }




        [Authorize(Policy = UserRoles.Doctor)]
        [HttpGet("UnAvailableAppointment")]
        public ActionResult<GeneralResponse> GetUnavailableAppointments()
        {
            Doctor doctor= GetLoggedInDoctor();
            GeneralResponse generalResponse=unitOfWork.AppointmentService.GetUnavailableAppointmentOfDoctor(doctor.Id);
            return generalResponse;
        }

        [Authorize(Policy = UserRoles.Relative)]
        [HttpGet("AvailableAppointments")]
        public ActionResult<GeneralResponse> GetAvailableAppointments()
        {
            Patient patient = GetPatient();
            Doctor doctor = unitOfWork.DoctorRepository.Get(d => d.Id == patient.Id);
            GeneralResponse generalResponse = unitOfWork.AppointmentService.GetAvailableAppointmentsOfDoctor(doctor);
            return generalResponse;
        }


        //Habeba

        [Authorize(Policy = UserRoles.Relative)]
        [HttpGet("AvailableAppointmentsToReserve")]
        public ActionResult<GeneralResponse> GetAvailableAppointmentsToReserve()
        {
            Patient patient = GetPatient();
            Doctor doctor = unitOfWork.DoctorRepository.Get(d=>d.Id==patient.Id);
            GeneralResponse generalResponse = unitOfWork.AppointmentService.GetAvailableAppointmentsOfDoctor(doctor);
            return generalResponse;
        }

        [HttpGet("AuthorizedToReserve")]
        [Authorize(policy: UserRoles.Relative)]
        public ActionResult<GeneralResponse> AuthorizedToReserve(int doctorId)
        {
            int relativeId = GetLoggedInRelativeId();
            Relative relative = GetLoggedInRelative();
            if (relative.Patient.DoctorId != doctorId)
            {
                return new GeneralResponse
                {
                    IsSuccess = false,
                    Data = "Unauthorized To Reserve Appointment"
                };
            }
            return new GeneralResponse
            {
                IsSuccess = true,
                Data = "Authorized To Reserve Appointment"
            };
        }

        
    }
}