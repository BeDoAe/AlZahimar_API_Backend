using System.ComponentModel.DataAnnotations;

namespace ZahimarProject.DTOS.RatingDTOs
{
    public class RatingDTO
    {
        public int RelativeId { get; set; }
        public int DoctorId { get; set; }

        //only 1-5
        [Range(1, 5, ErrorMessage = "Rating value must be between 1 and 5.")]
        public int? RatingValue { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Date { get; set; }

    }
}
