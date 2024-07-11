using System.Linq.Expressions;
using ZahimarProject.DTOS.DoctorDTO;
using ZahimarProject.Models;

namespace ZahimarProject.Repos.DoctorRepo
{
    public interface IDoctorRepository:IRepository<Doctor>
    {
        public bool IsPatientFoundInDoctorList(int PatientId);
        public List<Doctor> SearchForDoctor(string Name);
        public List<Doctor> GetRandomDoctors(List<Doctor> allDoctors, int numberOfDoctors = 5);
        public List<Doctor> GetFilteredDoctorsByPrice(List<Doctor> allDoctors);
        public List<Doctor> GetFilteredDoctorsByAverageRating(List<Doctor> allDoctors);
        public List<Doctor> GetFilteredMaleDoctors(List<Doctor> allDoctors);
        public List<Doctor> GetFilteredFemaleDoctors(List<Doctor> allDoctors);
        public List<Doctor> GetFilteredDoctorsByAge(List<Doctor> allDoctors);
        public List<DoctorPendingRequest> GetAllPendingDoctorRequests();
        public GeneralResponse AcceptPendingDoctorRequest(int doctorId);
        public GeneralResponse RejectPendingDoctorRequest(int doctorId);
        public List<Doctor> GetDoctorsByAgeGreaterThan(List<Doctor> allDoctors, int Age);
        public List<Doctor> GetDoctorsByAgeSmallerThan(List<Doctor> allDoctors, int Age);
        public List<Doctor> GetDoctorsByPriceRange(List<Doctor> allDoctors, int EndRange = 500);

        public List<Doctor> GetFilteredDoctorsBySpecificAverageRating(List<Doctor> allDoctors, int AvgRate);
        public List<Doctor> GetTopRatedDoctors(List<Doctor> allDoctors, int TopNumber);
        public List<Doctor> GetAllPendingDoctors();
        public Task<string> UpdateDoctorPhoto(UploadImageDTO uploadImageDTO, Doctor doctor);

        public Doctor GetDoctorToHandleLogin(Expression<Func<Doctor, bool>> filter);
        public bool EditStartEndWork(int doctorId, DoctorToEditStartEndAppointmentDTO doctorToEdit);

        public int GetAcceptedDoctorsCount();
    }

}
