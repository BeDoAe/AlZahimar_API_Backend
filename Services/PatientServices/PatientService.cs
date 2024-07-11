using ZahimarProject.DTOS.DoctorDTO;
using ZahimarProject.DTOS.PatientDTOs;
using ZahimarProject.Helpers.SaveImages;
using ZahimarProject.Models;
using ZahimarProject.Repos.PatientRepo;

namespace ZahimarProject.Services.PatientServices
{
    public class PatientService : Service<Patient>, IPatientService
    {

        public IPatientRepository patientRepository { get; }

        public PatientService(IPatientRepository patientRepository)
        {
            this.patientRepository = patientRepository;
        }

        private void DeleteImage(string imageUrl)
        {
            string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string fullPath = Path.Combine(webRootPath, imageUrl.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        public PatientDTO PatientForGetDTO(Patient patient)
        {
            //here
            PatientDTO PatientForGetDTO = new()
            {
                Address = patient.Address,
                Age = patient.Age,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Gender = patient.Gender,
            };
            PatientForGetDTO.GenderString = PatientForGetDTO.Gender == Gender.Male ? "Male" : "Female";

            return PatientForGetDTO;
        }

        public async void GetUpdetedPatient(PatientDTO patientDTO, Patient UpdatedPatient)
        {

            UpdatedPatient.Address = patientDTO.Address;
            UpdatedPatient.Age = patientDTO.Age;
            UpdatedPatient.FirstName = patientDTO.FirstName;
            UpdatedPatient.LastName = patientDTO.LastName;
            UpdatedPatient.Gender = patientDTO.Gender;

            if (patientDTO.Image != null)
            {

                if (!string.IsNullOrEmpty(UpdatedPatient.PicURL))
                {
                    DeleteImage(UpdatedPatient.PicURL);
                }

                UpdatedPatient.PicURL = await ImageHelper.SaveImageAsync(patientDTO.Image);
            }

        }

       
    }
}