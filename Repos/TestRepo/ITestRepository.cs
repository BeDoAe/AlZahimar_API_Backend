using System.Linq.Expressions;
using ZahimarProject.DTOS.TestDTOs;
using ZahimarProject.Models;

namespace ZahimarProject.Repos.TestRepo
{
    public interface ITestRepository:IRepository<Test>
    {
        //public Patient GetPatient(string userId);

        //public List<Test> GetTestsByPatientId(int patientId);
        //public Test GetTest(int patientId, int TestID);

        //public void AdminDeleteTest(int patientId, int TestID);

        //public void UpdateTest(Test test);

        //public Test CheckTest(int id);
        public Test Getspecific(int Testid);

        public PatientTest PatientTest(int patientID, int TestID);
        public List<Test> getTestInfo();
        public bool HasTest(int patientID, int TestID);

        public int ScoreOfPatientTest(int patientID, int TestID);
        public List<Test> GetTestsByPatientId(int patientId);
        public  List<Test> GetAll();
        public Test Get(Expression<Func<Test, bool>> filter);

    }
}