using ZahimarProject.DTOS.PatientDTOs;
using ZahimarProject.Models;
using ZahimarProject.Repos.PatientRepo;

namespace ZahimarProject.Services.PatientServices
{
    public interface IPatientService:IService<Patient>
    {
        public IPatientRepository patientRepository { get; }
        //public PatientGetDTO PatientForGetDTO(Patient patient);
        public PatientDTO PatientForGetDTO(Patient patient);
        public void GetUpdetedPatient(PatientDTO patientDTO, Patient UpdatedPatient);
    }
}