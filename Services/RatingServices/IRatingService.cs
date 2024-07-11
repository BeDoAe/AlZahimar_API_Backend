using ZahimarProject.DTOS.RatingDTOs;
using ZahimarProject.Models;
using ZahimarProject.Repos.PatientRepo;
using ZahimarProject.Repos.RatingRepo;

namespace ZahimarProject.Services.RatingServices
{
    public interface IRatingService:IService<Rating>
    {
        public IRatingRepository RatingRepository { get; }
        public Rating AddRaiting(AddRatingDTO addRatingDTO, int relativeId);
        public double GetAverageRatingForDoctor(List<Rating> ratings);
        public List<RatingDTO> GetRatingsForDoctor(List<Rating> ratings);
    }
}
