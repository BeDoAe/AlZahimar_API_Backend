namespace ZahimarProject.DTOS.StoryDTOs
{
    public class StoryFormFileDto
    {
       
        public IFormFile StoryImageUrl { get; set; }
        public string StoryDescription { get; set; }
        public IFormFile StorySoundPath { get; set; }
        public int StoryDegree { get; set; }


        //public List<AddStoryQuestionAnswerDTOs> StoryTestAnswerQuestions { get; set; }
    }
}
