using System.ComponentModel.DataAnnotations;

namespace ZahimarProject.DTOS.RatingDTOs
{
    public class AddRatingDTO
    {

        //only 1-5
        [Range(1, 5, ErrorMessage = "Rating value must be between 1 and 5.")]
        public int? RatingValue { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public int DoctorId { get; set; }

    }
}
