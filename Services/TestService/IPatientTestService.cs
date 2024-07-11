using ZahimarProject.DTOS.TestDTOs;
using ZahimarProject.Models;
using ZahimarProject.Repos.PatientRepo;
using ZahimarProject.Repos.TestRepo;
namespace ZahimarProject.Services.TestService
{
    public interface IPatientTestService
    {
        public void UpdateTask(TestDTO testDTO);
       // public Test GradeTest(TestResultDto testResultDTO);
        public List<TestDTO> GetTestDTOsByPatientId(int patientId);



    }
}