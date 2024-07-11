using ZahimarProject.Models;

namespace ZahimarProject.Repos.PatientStoryRepo
{
    public interface IPatientStoryRepository : IRepository<PatientStoryTest>
    {
        public List<PatientStoryTest> GetAllPatientStoryTests(int patientId);

    }
}