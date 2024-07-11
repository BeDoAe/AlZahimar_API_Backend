using System.Numerics;
using ZahimarProject.DTOS.DoctorDTO;
using ZahimarProject.DTOS.PatientDoctorRequestDTOs;
using ZahimarProject.DTOS.PatientDTOs;
using ZahimarProject.DTOS.RelativeDTOs;
using ZahimarProject.Helpers.Enums;
using ZahimarProject.Models;
using ZahimarProject.Repos.DoctorRepo;
using ZahimarProject.Repos.PatientDoctorRequestRepo;

namespace ZahimarProject.Services.PatientDoctorServices
{
    public class PatientDoctorRequestService:Service<PatientDoctorRequest> , IPatientDoctorRequestService
    {
        public IPatientDoctorRequestRepository PatientDoctorRequestRepository { get; }

        public PatientDoctorRequestService(IPatientDoctorRequestRepository PatientDoctorRequestRepository)
        {
            this.PatientDoctorRequestRepository = PatientDoctorRequestRepository;
        }

        public PatientDoctorRequest GetPatientDoctorRequest(PatientDoctorRequestDTO request)
        {
            PatientDoctorRequest patientDoctorRequest = new PatientDoctorRequest() {
                PatientId = request.PatientId,
                DoctorId = request.DoctorId,
                Status= RequestStatus.Pending,
            };

            return patientDoctorRequest;
        }


        public List<PatientNameDTO> GetPendingRequestsPatientNames(int doctorId)
        {
            var PendingRequests =
                   PatientDoctorRequestRepository
                    .GetAll()
                   .Where(r => r.DoctorId == doctorId && r.Status == RequestStatus.Pending)
                   .Select(r => new PatientNameDTO
                   {
                       FullName = $"{r.Patient.FirstName} {r.Patient.LastName}",
                       Id= r.Id,
                       Age = r.Patient.Age,
                       RequestStatus = RequestStatus.Pending,
                       PicUrl = r.Patient.PicURL,
                       Gender = r.Patient.Gender,
                       Time = DateTime.Now
                   })
                   .ToList();

            return PendingRequests;
        }

        public List<PatientNameDTO> GetHistoryPatientRequest(int doctorId)
        {
            var PendingRequests = PatientDoctorRequestRepository
            .GetAll()
            .Where(r => r.DoctorId == doctorId)
            .Select(r => new PatientNameDTO
            {
                FullName = $"{r.Patient.FirstName} {r.Patient.LastName}",
                Id = r.PatientId,
                RequestStatus = r.Status,
                Status = r.Status == RequestStatus.Pending ? "Pending" :
                            r.Status == RequestStatus.Accepted ? "Accepted" :
                            r.Status == RequestStatus.Rejected ? "Rejected" : "Unknown"
            })
            .ToList();

                return PendingRequests;
        }


        public List<PatientNameDTO> GetAllPatientsOfDoctor(int doctorId)
        {
            var PendingRequests =
                   PatientDoctorRequestRepository
                    .GetAll()
                   .Where(r => r.DoctorId == doctorId && r.Status== RequestStatus.Accepted)
                   .Select(r => new PatientNameDTO
                   {
                       FullName = $"{r.Patient.FirstName} {r.Patient.LastName}",
                       Id = r.PatientId
                   })
                   .ToList();

            return PendingRequests;
        }

        


    }
}
