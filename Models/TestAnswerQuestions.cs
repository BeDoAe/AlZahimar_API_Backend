using System.ComponentModel.DataAnnotations.Schema;

namespace ZahimarProject.Models
{
    public class TestAnswerQuestions
    {
       public int ID { get; set; }
        public string Question { get; set; }
        public string CorrectAnswer { get; set; }

        [ForeignKey("Test")]
        public int TestId { get; set; }
        public Test Test { get; set; }
        public List<string> Answers { get; set; }
        public bool IsDeleted { get; set; }
    }
}
