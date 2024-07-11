using ZahimarProject.Authentication;
using ZahimarProject.DTOS.AppointmentDTOs;
using ZahimarProject.DTOS.DoctorDTO;
using ZahimarProject.Models;
using ZahimarProject.Repos.DoctorRepo;

namespace ZahimarProject.Services.DoctorServices
{
    public interface IDoctorService : IService<Doctor>
    {
        public IDoctorRepository DoctorRepository { get; }
        public DoctorGetDTO GetDoctorDTO(Doctor doctor);
        public void GetUpdatedDoctor(DoctorDTO doctorDTO, Doctor UpdatedDoctor);
        public void GetUpdatedAppUser(Doctor doctor, AppUser appUser);
        public List<DoctorGetDTO> GetDoctorDTOs(List<Doctor> doctors);
        public List<FilterDoctorDTO> GetSearchResult(List<Doctor> doctors);
        public List<RandomDoctorDTO> GetRandomDoctorsDTO(List<Doctor> doctors);
        public List<FilterDoctorDTO> GetFilteredDoctorsDTO(List<Doctor> doctors);
        public List<DoctorPendingRequest> GetAllPendingDoctorRequests();
        public List<SearchDoctorDTO> GetPendingDoctorsDTO(List<Doctor> doctors);
        public GeneralResponse EditStartEndWorkOfDoctor(int DoctorId, DoctorToEditStartEndAppointmentDTO doctorToEdit);
        //public List<RandomDoctorDTO> GetRandomDoctorDTOs(List<Doctor> doctors);
        //public List<FilterDoctorDTO> GetFilteredDoctorsByAverageRateDTO(List<Doctor> doctors);
        public GeneralResponse EditDotorStartEndDuration(int DoctorId, AppointmentStartEndDurationDTO appointmentStartEndDurationDTO);
        public GeneralResponse GetDotorStartEndDuration(int DoctorId);
    }
}
