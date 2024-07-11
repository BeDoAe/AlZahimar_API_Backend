using Microsoft.EntityFrameworkCore;
using ZahimarProject.DTOS.RelativeDTOs;
using ZahimarProject.Helpers.SaveImages;
using ZahimarProject.Models;

namespace ZahimarProject.Repos.PatientRepo
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        private readonly Context context;
        internal DbSet<Patient> patients;
        public PatientRepository(Context _context) : base(_context)
        {
            this.context = _context;
            this.patients = context.Patients;

        }

        public Patient GetPatient(string RelativeUserId)
        {
            Relative relative = context.Relatives.FirstOrDefault(r=>r.AppUserId== RelativeUserId);
            return Get(p=>p.Id==relative.PatientId);
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
        public async Task<string> UpdatePatientPhoto(UploadPatientImageDTO uploadPatientImageDTO, Patient patient)
        {
            if (uploadPatientImageDTO.Image != null)
            {
                if (!string.IsNullOrEmpty(patient.PicURL))
                {
                    DeleteImage(patient.PicURL);
                }

                patient.PicURL = await ImageHelper.SaveImageAsync(uploadPatientImageDTO.Image);
                Update(patient);
                await context.SaveChangesAsync();
            }
            return patient.PicURL;
        }


        

    }
}
