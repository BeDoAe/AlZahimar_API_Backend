using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ZahimarProject.Models
{
    public class PatientTest
    {
        public int Id { get; set; }

        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        [ForeignKey("Test")]
        public int TestId { get; set; }
        public Test Test { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateTaken { get; set; }
        public int Score { get; set; }
    }
}