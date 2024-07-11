using ZahimarProject.DTOS.MemmoriesDto;
using ZahimarProject.DTOS.StoryDTOs;
using ZahimarProject.DTOS.TestDTOs;
using ZahimarProject.Models;
using ZahimarProject.Repos.StoryRepo;
using ZahimarProject.Repos.TestRepo;
using ZahimarProject.Helpers.SaveImages;
using ZahimarProject.DTOS.StoryDTOs;
using ZahimarProject.Repos.PatientTestRepo;
using ZahimarProject.Repos.PatientStoryRepo;


namespace ZahimarProject.Services.StoryServices
{
    public class StoryServices : Service<StoryTest>, IStoryServices
    {
        private readonly IStoryRepository storyRepository;

        public IPatientStoryRepository PatientStoryRepository { get; }

        public StoryServices(IStoryRepository storyRepository , IPatientStoryRepository patientStoryRepository)
        {
            this.storyRepository = storyRepository;
            PatientStoryRepository = patientStoryRepository;
        }

        public List<StoryInfoDto> GetAllStoryInfo()
        {
            var StoryTests = storyRepository
                .getStoryInfo()
                .Select(story => new StoryInfoDto
                {
                    StoryId = story.Id,
                    StoryImageUrl = story.ImageUrl,
                    StoryDescription = story.Description,
                    StoryDegree = story.Degree
                }).ToList();
            return StoryTests;
        }
        public List<StoryDTOs> GetStoryTestsDTOs(int patientId)
        {
            var StoryTests = storyRepository
            //.GetAll()
            .GetStoryTestsByPatientId(patientId)
           .Select(t => new StoryDTOs
           {
               StoryId = t.Id,
               StoryDescription = t.Description,
               StoryImageUrl = t.ImageUrl,
               StorySoundPath = t.SoundPath,
               StoryDegree = t.Degree,

               StoryQuestionAndAnswers = t.StoryQuestionAndAnswers.Select(q => new StoryTestAnswersQuestionsDTO
               {
                   StoryId = q.Id,
                   StoryQuestion = q.Question,
                   StoryAnswers = q.Answers,


               }).ToList()
           })

           .ToList();

            return StoryTests;
        }


        public StoryDTOs GetStoryTestDTO(int storytestId)
        {
            var StoryTest = storyRepository.Getspecific(storytestId);
            if (StoryTest == null)
            {
                return null;
            }
            var StorytestDTO = new StoryDTOs
            {
                StoryId = StoryTest.Id,
                StoryDescription = StoryTest.Description,
                StoryImageUrl = StoryTest.ImageUrl,
                StorySoundPath = StoryTest.SoundPath,
                StoryDegree = StoryTest.Degree,

                StoryQuestionAndAnswers = StoryTest.StoryQuestionAndAnswers.Select(q => new StoryTestAnswersQuestionsDTO
                {
                    StoryId = StoryTest.Id,
                    StoryQuestion = q.Question,
                    StoryAnswers = q.Answers,


                }).ToList()
            };

            return StorytestDTO;
        }

        public async Task<StoryTest> AddStoryTestDTO(StoryFormFileDto storyFormfileDTO)
        {
            var test = new StoryTest
            {
                Degree = storyFormfileDTO.StoryDegree,
                Description = storyFormfileDTO.StoryDescription,
                //PicURL = await ImageHelper.SaveImageAsync(addMemmoryDto.Image),
                ImageUrl = await ImageHelper.SaveImageAsync(storyFormfileDTO.StoryImageUrl),
                SoundPath = await ImageHelper.SaveImageAsync(storyFormfileDTO.StorySoundPath),
                StoryQuestionAndAnswers = new List<StoryQuestionAndAnswer>()

                //StoryQuestionAndAnswers = storyFormfileDTO.StoryTestAnswerQuestions.Select(q => new StoryQuestionAndAnswer
                //{
                //    Question = q.StoryQuestion,
                //    CorrectAnswer = q.StoryCorrectAnswer,
                //    Answers = q.StoryAnswers
                //}).ToList()
            };

            return test;
        }
        public StoryTest AddStoryQuestions(int storyTestId, List<AddStoryQuestionAnswerDTOs> questionsAndAnswers)
        {
            var storyTest = storyRepository.Getspecific(storyTestId);
            if (storyTest == null)
                return null;
            else
            {

                storyTest.StoryQuestionAndAnswers = questionsAndAnswers.Select(q => new StoryQuestionAndAnswer
                {
                    Question = q.StoryQuestion,
                    CorrectAnswer = q.StoryCorrectAnswer,
                    Answers = q.StoryAnswers
                }).ToList();

                return storyTest;
            }
        }
        public async Task<StoryTest> UpdateTestDTO(StoryTest storytest, StoryFormFileDto storyFormfileDTO)
        {

            storytest.Degree = storyFormfileDTO.StoryDegree;
            storytest.Description = storyFormfileDTO.StoryDescription;
            storytest.ImageUrl = await ImageHelper.SaveImageAsync(storyFormfileDTO.StoryImageUrl);
            storytest.SoundPath = await ImageHelper.SaveImageAsync(storyFormfileDTO.StorySoundPath);


            //storytest.StoryQuestionAndAnswers = storyFormfileDTO.StoryTestAnswerQuestions.Select(q => new StoryQuestionAndAnswer
            //{
            //    CorrectAnswer = q.StoryCorrectAnswer,
            //    Question = q.StoryQuestion,
            //    Answers = q.StoryAnswers,


            //}).ToList();


            return storytest;
        }

        public int EvaluateTest(int patientId, int StorytestId, List<PatientStoryAnswersDTO> patientStoryAnswers)
        {
            var Storytest = storyRepository.Getspecific(StorytestId);

            if (Storytest == null)
            {
                return 0;
            }
            else
            {
                int score = 0;

                foreach (var question in Storytest.StoryQuestionAndAnswers)
                {
                    foreach (var patientStoryAnswer in patientStoryAnswers)
                    {
                        //StoryQuestionAndAnswer storyAnswerQuestions = storyRepository.StoryAnswer(question.Id);

                        string patientAnswer = patientStoryAnswer.StoryAnswer;
                        string rightAnswer = question.CorrectAnswer;

                        if (patientAnswer != null && rightAnswer.ToLower().Equals(patientAnswer.ToLower()))
                        {

                            if (score < Storytest.Degree)
                            {
                                score += 1;
                                question.isCorrected = true;
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

        public List<StoryReviewDTO> ReviewStoryDTO(int patientId)
        {
            List<PatientStoryTest> patientStoryTest = PatientStoryRepository.GetAllPatientStoryTests(patientId);

            List<StoryReviewDTO> result = new List<StoryReviewDTO>();
            foreach (PatientStoryTest test in patientStoryTest)
            {
                StoryReviewDTO storyReviewDTO = new StoryReviewDTO()
                {
                    Date = test.DateTaken.ToString("ddd MMM dd hh:mm"),
                    PatientName = test.Patient.FirstName + " " + test.Patient.LastName,
                    TestDegree = test.StoryTest.Degree,
                    TestTitle = test.StoryTest.Description,
                    TestScore = test.Score
                };
                result.Add(storyReviewDTO);
            }

            return result;
        }


    }
}
