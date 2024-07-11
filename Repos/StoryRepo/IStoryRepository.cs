using System.Linq.Expressions;
using ZahimarProject.Models;

namespace ZahimarProject.Repos.StoryRepo
{
    public interface IStoryRepository : IRepository<StoryTest>
    {

        public List<StoryTest> GetAll();

        public List<StoryTest> GetStoryTestsByPatientId(int patientId);

        //public  StoryTest Get(Expression<Func<StoryTest, bool>> filter);

        public void Delete(StoryTest storyTest);

        public StoryTest Getspecific(int storyid);

        public bool HasTest(int patientID, int StorytestID);

        public StoryQuestionAndAnswer StoryAnswer(int QuestionID);

        public PatientStoryTest PatientStoryTest(int patientID, int StorytestID);

        public int ScoreOfPatientStory(int patientID, int StorytestID);
        public List<StoryTest> getStoryInfo();

    }
}
