namespace ZahimarProject.DTOS.StoryDTOs
{
    public class StoryTestAnswersQuestionsDTO
    {
        public int StoryId { get; set; }
        public string StoryQuestion { get; set; }

        public List<string> StoryAnswers { get; set; }
    }
}
