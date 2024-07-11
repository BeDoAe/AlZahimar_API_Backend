namespace ZahimarProject.DTOS.TestDTOs
{
    public class AddTestAnswerQuestionDTO
    {
        public string Question { get; set; }
        public List<string> Answers { get; set; }
        public string CorrectAnswer { get; set; }

    }
}
