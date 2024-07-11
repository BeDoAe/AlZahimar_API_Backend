using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ZahimarProject.DTOS.DoctorDTO;
using ZahimarProject.DTOS.PatientDoctorRequestDTOs;
using ZahimarProject.DTOS.PatientDTOs;
using ZahimarProject.Helpers;
using ZahimarProject.Helpers.Enums;
using ZahimarProject.Helpers.UnitOfWorkFolder;
using ZahimarProject.Models;

namespace ZahimarProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientDoctorController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public PatientDoctorController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        Doctor GetLoggedInDoctor()
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Doctor doctor = unitOfWork.DoctorRepository.Get(r => r.AppUserId == LoggedInUserId);
            return doctor;
        }
        Patient GetPatient()
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Patient patient = unitOfWork.PatientRepository.GetPatient(LoggedInUserId);
            return patient;
        }

        [Authorize(Policy = UserRoles.Relative)]
        [HttpPost("request/{DoctorId:int}")]
        public ActionResult<dynamic> RequestDoctorAssignment(int DoctorId )
        {
            Patient patientLogin=GetPatient();
            //var patient = unitOfWork.PatientRepository.Get(p=>p.Id== patientLogin.Id);
            var doctor = unitOfWork.DoctorRepository.Get(d=>d.Id== DoctorId);

            if ( doctor == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Doctor Not Found Plz Try Again"),
                };
            }

            var existingRequest = unitOfWork.PatientDoctorRequestRepository
                .Get(r => r.PatientId == patientLogin.Id && r.DoctorId == DoctorId);

            if (existingRequest != null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = BadRequest("Request already exists."),
                };
            }
            PatientDoctorRequestDTO dTO=new PatientDoctorRequestDTO() { DoctorId=DoctorId,PatientId=patientLogin.Id};
            var request = unitOfWork.PatientDoctorRequestService.GetPatientDoctorRequest(dTO);
            unitOfWork.PatientDoctorRequestRepository.Insert(request);
            unitOfWork.Save();
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = Ok("Request Added Successfully"),
            };
            
        }

        [Authorize(policy: UserRoles.Doctor)]
        [HttpPost("accept")]
        public ActionResult<dynamic> AcceptPatientAssignment(int requestId)
        {
            PatientDoctorRequest request = unitOfWork.PatientDoctorRequestRepository
                .Get(r => r.Id == requestId);

            if (request == null || request.Status != RequestStatus.Pending)
            {
                return new GeneralResponse() {
                IsSuccess=false,
                Data = NotFound("Request Not Found"),
                };
            }

            request.Status = RequestStatus.Accepted;
            request.Patient.DoctorId = request.DoctorId;
            request.Doctor.Patients.Add(request.Patient);

            unitOfWork.Save();

            return new GeneralResponse() { IsSuccess=true , Data=Ok("Request Accepted Successfully") };
        }


        [Authorize(policy: UserRoles.Doctor)]
        [HttpPost("reject")]
        public ActionResult<dynamic> RejectPatientAssignment(int requestId)
        {
            var request = unitOfWork.PatientDoctorRequestRepository
                .Get(r => r.Id == requestId);

            if (request == null || request.Status != RequestStatus.Pending)
            {

                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Request Not Found"),
                };
            }
            request.Status = RequestStatus.Rejected;
            request.IsDeleted = true;
            unitOfWork.PatientDoctorRequestRepository.Update(request);
            //request.Patient.DoctorId = null;
            //request.Patient.Doctor = null;
            //request.Doctor.Patients.Remove(request.Patient);
            unitOfWork.Save();

            return new GeneralResponse() { IsSuccess = true, Data = Ok("Request Rejected Successfully") };
        }   


        [Authorize(policy: UserRoles.Doctor)]
        [HttpGet("Requests")]
        public ActionResult<dynamic> GetRequsts()
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Doctor doctor = unitOfWork.DoctorRepository.Get(r => r.AppUserId == LoggedInUserId);
            if (doctor != null)
            {
                List<PatientNameDTO> patientNameDTO = unitOfWork.PatientDoctorRequestService.GetPendingRequestsPatientNames(doctor.Id);

                return new GeneralResponse()
                {
                    IsSuccess = true,
                    Data = patientNameDTO,
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
        [HttpGet("HistoryRequests")]
        public ActionResult<dynamic> GetHistoryRequsts()
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Doctor doctor = unitOfWork.DoctorRepository.Get(r => r.AppUserId == LoggedInUserId);
            if (doctor != null)
            {
                List<PatientNameDTO> patientNameDTO = unitOfWork.PatientDoctorRequestService.GetHistoryPatientRequest(doctor.Id);

                return new GeneralResponse()
                {
                    IsSuccess = true,
                    Data = patientNameDTO,
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
        [HttpGet("DoctorPatients")]
        public ActionResult<dynamic> GetPatientsOfDoctor()
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Doctor doctor = unitOfWork.DoctorRepository.Get(r => r.AppUserId == LoggedInUserId);
            if (doctor != null)
            {
                List<PatientNameDTO> patientNameDTO = unitOfWork.PatientDoctorRequestService.GetAllPatientsOfDoctor(doctor.Id);

                return new GeneralResponse()
                {
                    IsSuccess = true,
                    Data = patientNameDTO,
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


        [Authorize(policy: UserRoles.Relative)]
        [HttpGet("DoctorOfPatient")]
        public ActionResult<dynamic> GetDoctorOfPatient()
        {
            ClaimsPrincipal user = this.User;
            string loggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Patient patient = unitOfWork.PatientRepository.GetPatient(loggedInUserId);
            if (patient != null)
            {
                Doctor doctor = unitOfWork.DoctorRepository.Get(d=>d.Id == patient.DoctorId);
                if (doctor != null)
                {
                    DoctorOfPatientDTO doctorDTO = new DoctorOfPatientDTO
                    {
                        FullName = $"{doctor.FirstName} {doctor.LastName}",
                        Id = doctor.Id
                    };

                    return new GeneralResponse()
                    {
                        IsSuccess = true,
                        Data = doctorDTO,
                    };
                }
                else
                {
                    return new GeneralResponse()
                    {
                        IsSuccess = false,
                        Data = NotFound("Doctor Not Found"),
                    };
                }
            }
            else
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Patient Not Found"),
                };
            }
        }

        [Authorize(policy: UserRoles.Relative)]
        [HttpDelete("DoctorOfPatient")]
        public ActionResult<dynamic> DeleteDoctorOfPatient()
        {
            ClaimsPrincipal user = this.User;
            string loggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Patient patient = unitOfWork.PatientRepository.GetPatient(loggedInUserId);
            if (patient != null)
            {
                Doctor doctor = unitOfWork.DoctorRepository.Get(p => p.Id == patient.DoctorId);
                if (doctor != null)
                {
                    patient.DoctorId = null;
                    PatientDoctorRequest patientDoctorRequest = unitOfWork.PatientDoctorRequestRepository.Get(p => p.PatientId == patient.Id&& p.Status==RequestStatus.Accepted);
                    unitOfWork.PatientDoctorRequestRepository.DeleteDoctorOfPatient(patientDoctorRequest);
                    doctor.Patients.Remove(patient);
                }
                else
                {
                    return new GeneralResponse()
                    {
                        IsSuccess = false,
                        Data = NotFound("Doctor Not Found"),
                    };
                }
            }
            else
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Patient Not Found"),
                };
            }
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = "Deleted Successfully",
            };
        }


        [Authorize(policy: UserRoles.Relative)]
        [HttpGet("IsPatientAssignedToDoctor")]
        public ActionResult<GeneralResponse> IsPatientAssignedToDoctor()
        {
            Patient patient = GetPatient();
            bool isAssigned = unitOfWork.PatientDoctorRequestRepository.IsDoctorAssignedToDoctor(patient.Id);
            if (isAssigned)
            {
                return new GeneralResponse()
                {
                    IsSuccess = true,
                    Data = "this Patient is Assigned To Doctor"
                };
            }
            else
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = "this Patient is Not Assigned To Doctor"
                };
            }
        }



    }
}
