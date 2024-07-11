using System.Text.Json.Serialization;

namespace ZahimarProject.Models
{
    public class Test
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<TestAnswerQuestions> TestAnswerQuestions { get; set; }
        public bool IsDeleted { get; set; }
        //[JsonIgnore]
        public List<PatientTest> PatientTests { get; set; }
        public int Degree { get; set; }
    }
}