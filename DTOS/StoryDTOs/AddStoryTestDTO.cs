using ZahimarProject.DTOS.TestDTOs;

namespace ZahimarProject.DTOS.StoryDTOs
{
    public class AddStoryTestDTO
    {
        public string StoryDescription { get; set; }
        public string StoryUrlPic { get; set; }
        public string StorySoundUrl { get; set; }
        public int DegreeStoryTest { get; set; }
        public List<AddStoryQuestionAnswerDTOs> StoryTestAnswerQuestions { get; set; }
    }
}
