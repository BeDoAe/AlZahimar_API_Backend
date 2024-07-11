using ZahimarProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using NuGet.Protocol;
using ZahimarProject.DTOS.StoryDTOs;

namespace ZahimarProject.Repos.StoryRepo
{
    public class StoryRepository : Repository<StoryTest>, IStoryRepository
    {
        private readonly Context context;
        public StoryRepository(Context _context) : base(_context)
        {
            this.context = _context;
        }

        public override List<StoryTest> GetAll()
        {
            return context.StoryTests.Include(s => s.PatientStoryTests).Include(r => r.StoryQuestionAndAnswers).ToList();
        }
        public List<StoryTest> getStoryInfo()
        {
            return context.StoryTests.ToList();
        }

        public List<StoryTest> GetStoryTestsByPatientId(int patientId)
        {
            return context.PatientStoryTests
                .Where(pt => pt.PatientId == patientId)
                .Include(pt => pt.StoryTest.StoryQuestionAndAnswers)
                .Select(pt => pt.StoryTest)
                .ToList();
        }

        public StoryTest Getspecific(int storyid)
        {
            return context.StoryTests
                .Include(t => t.StoryQuestionAndAnswers)
                .FirstOrDefault(s => s.Id == storyid);
        }

        public PatientStoryTest PatientStoryTest(int patientID, int StorytestID)
        {
            return context.PatientStoryTests.FirstOrDefault(ps => ps.PatientId == patientID && ps.StoryTestId == StorytestID);
        }
        public bool HasTest(int patientID, int StorytestID)
        {
            bool patientStory = context.PatientStoryTests.Any(ps => ps.PatientId == patientID && ps.StoryTestId == StorytestID);

            if (patientStory == true)
            {
                return true;
            }
            else
                return false;
        }

        public int ScoreOfPatientStory(int patientID, int StorytestID)
        {
            PatientStoryTest patientStory = context.PatientStoryTests.FirstOrDefault(ps => ps.PatientId == patientID && ps.StoryTestId == StorytestID);

            if (patientStory == null)
            {
                return 0;
            }
            else
                return patientStory.Score;
        }
        public StoryQuestionAndAnswer StoryAnswer(int QuestionID)
        {
            return context.StoryQuestionAndAnswers.FirstOrDefault(q => q.Id == QuestionID);
        }

        public override void Delete(StoryTest storyTest)
        {
            string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            // Combine the web root path with the relative path from the database
            string fullPath = Path.Combine(webRootPath, storyTest.ImageUrl.TrimStart('/'));
            // Check if the file exists before attempting to delete it
            // Check if the file exists before attempting to delete it
            if (storyTest.ImageUrl != null)
            {
                File.Delete(fullPath);
                context.StoryTests.Remove(storyTest);
            }
        }
    }

}
