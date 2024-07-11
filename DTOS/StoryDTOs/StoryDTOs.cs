using ZahimarProject.Models;

namespace ZahimarProject.DTOS.StoryDTOs
{
    public class StoryDTOs
    {
        public int StoryId { get; set; }
        public string StoryImageUrl { get; set; }
        public string StoryDescription { get; set; }
        public string StorySoundPath { get; set; }
        public int StoryDegree { get; set; }


        //  public List<StoryQuestionAndAnswer> StoryQuestionAndAnswers { get; set; }
        public List<StoryTestAnswersQuestionsDTO> StoryQuestionAndAnswers { get; set; }
    }
}
