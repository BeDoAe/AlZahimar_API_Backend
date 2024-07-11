using ZahimarProject.DTOS.RatingDTOs;
using ZahimarProject.Models;
using ZahimarProject.Repos.RatingRepo;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZahimarProject.Services.RatingServices
{
    public class RatingService:Service<Rating> , IRatingService
    {
        public IRatingRepository RatingRepository  { get; }

        public RatingService(IRatingRepository RatingRepository)
        {
            this.RatingRepository = RatingRepository;
        }

        public Rating AddRaiting(AddRatingDTO addRatingDTO , int relativeId)
        {
            Rating rating = new Rating()
            {
                Comment = addRatingDTO.Comment,
                RelativeId = relativeId,
                RatingValue = addRatingDTO.RatingValue,
                DoctorId= addRatingDTO.DoctorId,
                CreatedAt = addRatingDTO.CreatedAt             

        };
            
            return rating;
        }

        public List<RatingDTO> GetRatingsForDoctor(List<Rating> ratings)
        {   
            return ratings.Select(r => new RatingDTO
            {
                RelativeId = r.RelativeId,
                DoctorId = r.DoctorId,
                RatingValue = r.RatingValue,
                Comment = r.Comment,
                //CreatedAt = r.CreatedAt,
                Date = r.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
            }).ToList();
        }

        public double GetAverageRatingForDoctor(List<Rating> ratings)
        {
            if (!ratings.Any())
            {
                return 0; // No ratings available
            }
            double averageRating = ratings.Average(r => (double)r.RatingValue);
            return Math.Round(averageRating, 1);
        }
    
    }
}
