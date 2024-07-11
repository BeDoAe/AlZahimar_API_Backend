using ZahimarProject.Models;

namespace ZahimarProject.Repos.RatingRepo
{
    public interface IRatingRepository:IRepository<Rating>
    {
        public List<Rating> GetRatingsOfDoctor(int doctorId);

        public List<Doctor> GetTop3AverageRatingDoctors();
    }

}
