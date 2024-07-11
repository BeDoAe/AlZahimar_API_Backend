using ZahimarProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using System;
using ZahimarProject.DTOS.TestDTOs;
using Microsoft.AspNetCore.Http.HttpResults;
namespace ZahimarProject.Repos.TestRepo
{
    public class TestRepository : Repository<Test>, ITestRepository
    {
        private readonly Context context;


        public TestRepository(Context _context) : base(_context)
        {
            context = _context;
        }

   
        public List<Test> getTestInfo()
        {
            return context.Tests.ToList();
        }

        public PatientTest PatientTest(int patientID, int TestID)
        {
            return context.PatientTests.FirstOrDefault(ps => ps.PatientId == patientID && ps.TestId == TestID);
        }

        public bool HasTest(int patientID, int TestID)
        {
            bool patientTest = context.PatientTests.Any(ps => ps.PatientId == patientID && ps.TestId == TestID);

            if (patientTest == true)
            {
                return true;
            }
            else
                return false;
        }
        public Test Getspecific(int Testid)
        {
            return context.Tests
                .Include(t => t.TestAnswerQuestions)
                .FirstOrDefault(t => t.Id == Testid);
        }
        public int ScoreOfPatientTest(int patientID, int TestID)
        {
            PatientTest patientTest = context.PatientTests.FirstOrDefault(ps => ps.PatientId == patientID && ps.TestId == TestID);

            if (patientTest == null)
            {
                return 0;
            }
            else
                return patientTest.Score;
        }
        public override List<Test> GetAll()
        {
            return context.Tests.Include(r=>r.PatientTests).Include(r => r.TestAnswerQuestions).ToList();
        }

        public List<Test> GetTestsByPatientId(int patientId)
        {
            return context.PatientTests
                .Where(pt => pt.PatientId == patientId)
                .Include(pt=>pt.Test.TestAnswerQuestions)
                .Select(pt => pt.Test)
                .ToList();
        }

        public override Test Get(Expression<Func<Test, bool>> filter)
        {
            return context.Tests
                .Include(t => t.TestAnswerQuestions)
                .FirstOrDefault(filter);
        }

       


        



    }
}
