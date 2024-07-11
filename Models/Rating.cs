using System.ComponentModel.DataAnnotations.Schema;

namespace ZahimarProject.Models
{
    public class Rating
    {
        public int Id { get; set; }
        //only 1-5
        public int? RatingValue { get; set; } 
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey("Relative")]
        public int RelativeId { get; set; }
        public Relative Relative { get; set; }

        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public bool IsDeleted { get; set; }

        //public string? Date { get; set; }  

    }
}
