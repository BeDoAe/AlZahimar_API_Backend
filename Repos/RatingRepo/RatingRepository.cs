using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using ZahimarProject.Models;
using ZahimarProject.Services.RatingServices;

namespace ZahimarProject.Repos.RatingRepo
{
    public class RatingRepository:Repository<Rating> , IRatingRepository
    {
        private readonly Context context;
        internal DbSet<Rating> Ratings;
        public RatingRepository(Context _context) : base(_context)
        {
            this.context = _context;
            this.Ratings = context.Ratings;

        }
       public List<Rating> GetRatingsOfDoctor(int doctorId)
        {
            return Ratings.Where(r => r.DoctorId == doctorId).ToList();
        }

        public List<Doctor> GetTop3AverageRatingDoctors()
        {
            // Step 1: Group ratings by DoctorId and calculate average rating for each doctor
            var doctorAverageRatings = Ratings
                .GroupBy(r => r.DoctorId)
                .Select(g => new
                {
                    DoctorId = g.Key,
                    AverageRating = g.Average(r => r.RatingValue)
                })
                .OrderBy(d => d.AverageRating)
                .Take(3)
                .ToList();

            // Step 2: Get the list of top 5 DoctorIds by average rating
            var top5DoctorIds = doctorAverageRatings.Select(d => d.DoctorId).ToList();

            // Step 3: Fetch and return the top 5 doctors by average rating
            var top5Doctors = context.Doctors
                .Where(d => top5DoctorIds.Contains(d.Id))
                .ToList();

            return top5Doctors;
        }


    }
}
