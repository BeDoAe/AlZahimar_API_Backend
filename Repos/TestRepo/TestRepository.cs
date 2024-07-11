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

        //public Patient GetPatient(string userId)
        //{
        //    Relative relative = context.Relatives
        //        .FirstOrDefault(p => p.AppUserId == userId);

        //    if (relative != null)
        //    {
        //        Patient patient = context.Patients.FirstOrDefault(p => p.Id == relative.PatientId);
        //        return patient;
        //    }

        //    return null;

        //}

        //public List<Test> GetTestsByPatientId(int patientId)
        //{
        //List<Test> tests = Context.PatientTests
        //    .Where(pt => pt.PatientId == patientId)
        //    //.Include(pt => pt.Test.TestAnswerQuestions)
        //    .Select(pt => pt.Test)
        //    .ToList();

        //return tests;
        //}
        //getTestInfo
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

       


        //public Test GetTest(int patientId, int TestID)
        //{
        //    PatientTest pt = context.PatientTests.FirstOrDefault(p => p.PatientId == patientId && p.TestId == TestID);
        //    if (pt != null)
        //    {
        //        Test test = pt.Test;
        //        return test;
        //    }
        //    else
        //    {
        //        throw new Exception("Not found");
        //    }
        //}


        //public void AdminDeleteTest(int patientId, int TestID)
        //{
        //    PatientTest pt = context.PatientTests.FirstOrDefault(p => p.PatientId == patientId && p.TestId == TestID);
        //    if (pt != null)
        //    {
        //        context.Tests.Remove(pt.Test);
        //    }
        //    else
        //        throw new Exception("No Test Found");


        //}
        //public void UpdateTest(Test test)
        //{
        //    Test t = context.Tests.FirstOrDefault(p => p.Id == test.Id);
        //    if (t != null)
        //    {
        //        context.Tests.Update(test);


        //    }
        //    else
        //        throw new Exception("Test is Already Taken");
        //}

        //public Test CheckTest(int id)
        //{
        //    // PatientTest patientTest = Context.PatientTests.FirstOrDefault(p => p.Id == id);

        //    Test Test = context.Tests.FirstOrDefault(t => t.Id == id);

        //    if (Test != null)
        //    {
        //        return Test;
        //    }
        //    else
        //        throw new Exception("Not Found");
        //}



    }
}
