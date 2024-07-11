using System.ComponentModel.DataAnnotations.Schema;

namespace ZahimarProject.Models
{
    public class StoryQuestionAndAnswer
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string CorrectAnswer { get; set; }

        [ForeignKey("StoryTest")]
        public int StoryTestId { get; set; }
        public StoryTest StoryTest  { get; set; }


        public List<string> Answers { get; set; }

        public bool isCorrected { get; set; } = false;
        public bool IsDeleted { get; set; }

    }
}
