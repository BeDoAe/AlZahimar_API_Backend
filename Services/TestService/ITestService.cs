using ZahimarProject.DTOS.StoryDTOs;
using ZahimarProject.DTOS.TestDTOs;
using ZahimarProject.Models;
using ZahimarProject.Repos.PatientRepo;
using ZahimarProject.Repos.TestRepo;
namespace ZahimarProject.Services.TestService
{
    public interface ITestService
    {
        public List<TestInfoDto> GetAllTestInfo();
        public List<TestDTO> GetTestDTOs(int patientId);
        public TestDTO GetTestDTO(int testId);
        public Test UpdateTestDTO(Test test, AddTestDTO testDTO);
        public Test AddTestDTO(AddTestDTO testDTO);
        public ResultOfTestDTO EvaluateTest(int patientId, int testId, List<PatientAnswerDTO> patientAnswers, string PatientName);

        public int EvaluateTest_Score(int patientId, int testId, List<PatientAnswerDTO> patientTestAnswers);
        public List<TestReviewDTO> ReviewTestDTO(int PatientId);

        //public ResultOfTestDTO EvaluateTest(int patientId, int testId, List<PatientAnswerDTO> patientAnswers);
        //public PatientTest EvaluateTest(int patientId, int testId, List<PatientAnswerDTO> patientAnswers);  
    }
}