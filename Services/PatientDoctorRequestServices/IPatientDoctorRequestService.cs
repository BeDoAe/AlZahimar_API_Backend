using ZahimarProject.DTOS.DoctorDTO;
using ZahimarProject.DTOS.PatientDoctorRequestDTOs;
using ZahimarProject.DTOS.PatientDTOs;
using ZahimarProject.Models;
using ZahimarProject.Repos.PatientDoctorRequestRepo;

namespace ZahimarProject.Services.PatientDoctorServices
{
    public interface IPatientDoctorRequestService:IService<PatientDoctorRequest>
    {
        public IPatientDoctorRequestRepository PatientDoctorRequestRepository { get; }
        public List<PatientNameDTO> GetPendingRequestsPatientNames(int doctorId);
        public List<PatientNameDTO> GetAllPatientsOfDoctor(int doctorId);
        public List<PatientNameDTO> GetHistoryPatientRequest(int doctorId);
        public PatientDoctorRequest GetPatientDoctorRequest(PatientDoctorRequestDTO request);
        // public List<DoctorOfPatientDTO> GetDoctorOfPatient(Patient patient);
    }
}