namespace ZahimarProject.Models
{
    public class StoryTest
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string SoundPath { get; set; }

       public List<StoryQuestionAndAnswer> StoryQuestionAndAnswers { get; set; }
      public  List<PatientStoryTest> PatientStoryTests { get; set; }

        public bool IsDeleted { get; set; }

        public int Degree { get; set; }

    }
}
