namespace ZahimarProject.DTOS.TestDTOs
{
    public class AddTestDTO
    {
        public string Title { get; set; }
        public int DegreeTest { get; set; }
        public List<AddTestAnswerQuestionDTO> TestAnswerQuestions { get; set; }
    }
}
