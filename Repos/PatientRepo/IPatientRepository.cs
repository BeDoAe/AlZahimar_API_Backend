using ZahimarProject.DTOS.RelativeDTOs;
using ZahimarProject.Models;

namespace ZahimarProject.Repos.PatientRepo
{
    public interface IPatientRepository:IRepository<Patient>
    {
        public Patient GetPatient(string RelativeUserId);

        //public Patient GetPatientById(int id);
        public Task<string> UpdatePatientPhoto(UploadPatientImageDTO uploadPatientImageDTO, Patient patient);

    }
}