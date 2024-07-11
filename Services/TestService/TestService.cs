using ZahimarProject.DTOS.PatientDTOs;
using ZahimarProject.DTOS.TestDTOs;
using ZahimarProject.Models;
using ZahimarProject.Repos.PatientDoctorRequestRepo;
using ZahimarProject.Repos.PatientRepo;
using ZahimarProject.Repos.PatientTestRepo;
using ZahimarProject.Repos.TestRepo;
using ZahimarProject.DTOS.TestDTOs;
using static System.Net.Mime.MediaTypeNames;
using ZahimarProject.DTOS.StoryDTOs;
using ZahimarProject.Repos.StoryRepo;


namespace ZahimarProject.Services.TestService
{
    public class TestService : ITestService
    {
        private readonly ITestRepository testRepository;
        private readonly IPatientTestRepository patientTestRepository;



        public TestService(ITestRepository testRepository, IPatientTestRepository patientTestRepository)
        {
            this.testRepository = testRepository;
            this.patientTestRepository = patientTestRepository;
        }



        public List<TestInfoDto> GetAllTestInfo()
        {
            var StoryTests = testRepository
                .getTestInfo()
                .Select(test => new TestInfoDto
                {
                    TestId = test.Id,
                    TestDegree = test.Degree,
                    Title = test.Title,
                }).ToList();
            return StoryTests;
        }
        public List<TestDTO> GetTestDTOs(int patientId)
        {
            var Tests = testRepository
            //.GetAll()
            .GetTestsByPatientId(patientId)
           .Select(t => new TestDTO
           {
               TestId = t.Id,
               Title = t.Title,
               DegreeTest = t.Degree,
               
               TestAnswerQuestions = t.TestAnswerQuestions.Select(q => new TestAnswerQuestionDTO
               {
                   ID = q.ID,
                   Question = q.Question,
                   Answers = q.Answers,
                  
                  
               }).ToList()
           })
           
           .ToList();

          return Tests;
        }


        public TestDTO GetTestDTO(int testId)
        {
            var Test = testRepository.Get(t => t.Id == testId);
            if(Test == null)
            {
                return null;
            }
            var testDTO = new TestDTO
            {
                TestId = Test.Id,
                Title = Test.Title,
                DegreeTest = Test.Degree,

                TestAnswerQuestions = Test.TestAnswerQuestions.Select(q => new TestAnswerQuestionDTO
                {
                    ID = q.ID,
                    Question = q.Question,
                    Answers = q.Answers,
                  

                }).ToList()
            };
         
            return testDTO;
        }

        public Test AddTestDTO(AddTestDTO testDTO)
        {
            var test = new Test
            {
                Degree = testDTO.DegreeTest,
                Title = testDTO.Title,
                TestAnswerQuestions = testDTO.TestAnswerQuestions.Select(q => new TestAnswerQuestions
                {
                    CorrectAnswer = q.CorrectAnswer,
                    Question = q.Question,
                    Answers = q.Answers,
                  
                   
                }).ToList()  
            };

            return test;
        }

        public Test UpdateTestDTO(Test test, AddTestDTO testDTO)
        {

            test.Degree = testDTO.DegreeTest;
            test.Title = testDTO.Title;
            test.TestAnswerQuestions = testDTO.TestAnswerQuestions.Select(q => new TestAnswerQuestions
            {
                CorrectAnswer = q.CorrectAnswer,
                Question = q.Question,
                Answers = q.Answers,
            }).ToList();
            

            return test;
        }


        //public ResultOfTestDTO EvaluateTest(int patientId, int testId, List<PatientAnswerDTO> patientAnswers)
        //{
        //    Test test = testRepository.Get(t => t.Id == testId);

        //    if (test == null)
        //    {
        //        return null;
        //    }

        //    int score = 0;

        //    foreach (var question in test.TestAnswerQuestions)
        //    {
        //        var patientAnswer = patientAnswers.FirstOrDefault(a => a.QuestionId == question.ID);

        //        if (patientAnswer != null&& patientAnswer.Answer == question.CorrectAnswer)
        //        {
        //           score++;
        //        }
        //    }

        //    PatientTest patientTest = new PatientTest()
        //    {
        //        DateTaken = DateTime.Now,
        //        PatientId = patientId,
        //        Score = score,
        //        TestId = testId,

        //    };
        //    patientTestRepository.Insert(patientTest);

        //    var result = new ResultOfTestDTO
        //    {
        //        TestScore = test.Degree,
        //        PatientScore=score,
        //        TestTitle = test.Title,
        //        DateTaken = DateTime.Now,
        //        PatientName="Mohammed Ali",
        //        TestAnswers=test.TestAnswerQuestions,
        //        patientAnswers=patientAnswers
        //    };
        //    return result;
        //}

        public ResultOfTestDTO EvaluateTest(int patientId, int testId, List<PatientAnswerDTO> patientAnswers,string PatientName)
        {
            Test test = testRepository.Get(t => t.Id == testId);

            if (test == null)
            {
                return null;
            }

            int score = 0;
            Dictionary<string, string> patientAnswerMap = new Dictionary<string, string>();
            Dictionary<string, string> testAnswerMap = new Dictionary<string, string>();

            foreach (var question in test.TestAnswerQuestions)
            {
                var patientAnswer = patientAnswers.FirstOrDefault(a => a.QuestionId == question.ID);

                if (patientAnswer != null)
                {
                    patientAnswerMap[question.Question] = patientAnswer.Answer;
                    testAnswerMap[question.Question] = question.CorrectAnswer;

                    if (patientAnswer.Answer == question.CorrectAnswer)
                    {
                        score++;
                    }
                }
                else
                {
                    patientAnswerMap[question.Question] = "No Answer";
                    testAnswerMap[question.Question] = question.CorrectAnswer;
                }
            }

            PatientTest patientTest = new PatientTest()
            {
                DateTaken = DateTime.Now,
                PatientId = patientId,
                Score = score,
                TestId = testId,
            };
            patientTestRepository.Insert(patientTest);

            var result = new ResultOfTestDTO
            {
                TestScore = test.Degree,
                PatientScore = score,
                TestTitle = test.Title,
                DateTaken = DateTime.Now,
                PatientName = PatientName, 
                TestAnswers = testAnswerMap,
                patientAnswers = patientAnswerMap
            };
            return result;
        }

        public int EvaluateTest_Score(int patientId, int testId, List<PatientAnswerDTO> patientTestAnswers)

        {
            var test = testRepository.Getspecific(testId);

            if (test == null)
            {
                return 0;
            }
            else
            {
                int score = 0;

                foreach (var question in test.TestAnswerQuestions)
                {
                    foreach (var patientTestAnswer in patientTestAnswers)
                    {
                        //StoryQuestionAndAnswer storyAnswerQuestions = storyRepository.StoryAnswer(question.Id);

                        string patientAnswer = patientTestAnswer.Answer;
                        string rightAnswer = question.CorrectAnswer;

                        if (patientAnswer != null && rightAnswer.ToLower().Equals(patientAnswer.ToLower()))
                        {

                            if (score <= test.Degree)
                            {
                                score += 10;
                                //question.isCorrected = true;
                            }
                        }
                    }
                }
                return score;


                //var result = new PatientStoryTest
                //{
                //    PatientId = patientId,
                //    StoryTestId = StorytestId,
                //    Score = score,
                //};


                //return result;
            }
        }

        public List<TestReviewDTO> ReviewTestDTO(int PatientId)
        {
            List<PatientTest> patientTest = patientTestRepository.GetAll().Where(pt => pt.PatientId == PatientId).ToList();
            List<TestReviewDTO> result = new List<TestReviewDTO>();
            foreach(PatientTest test in patientTest)
            {
                TestReviewDTO testReviewDTO = new TestReviewDTO()
                {
                    Date = test.DateTaken.ToString("ddd MMM dd hh:mm"),
                    PatientName = test.Patient.FirstName+" "+test.Patient.LastName,
                    TestDegree = test.Test.Degree,
                    TestTitle = test.Test.Title,
                    TestScore = test.Score
                };
                result.Add(testReviewDTO);
            }
           
            return result;

        }

    }
}
