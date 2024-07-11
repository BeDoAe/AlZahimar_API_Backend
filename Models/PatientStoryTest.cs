using System.ComponentModel.DataAnnotations.Schema;

namespace ZahimarProject.Models
{
    public class PatientStoryTest
    {
        public int Id { get; set; }
        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }


        [ForeignKey("StoryTest")]
        public int StoryTestId { get; set; }
        public StoryTest? StoryTest { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime DateTaken { get; set; } = DateTime.Now;


        public int Score { get; set; }
    }
}
