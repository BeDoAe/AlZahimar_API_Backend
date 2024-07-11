using ZahimarProject.DTOS.TestDTOs;
using ZahimarProject.Models;


namespace ZahimarProject.DTOS.TestDTOs
{
    public class TestDTO
    {

        public int TestId { get; set; }
        public string Title { get; set; }
        public int DegreeTest { get; set; }
        public List<TestAnswerQuestionDTO> TestAnswerQuestions { get; set; }

        public List<string>? Answers { get; set; }

    }
}
