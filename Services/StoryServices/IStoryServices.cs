using ZahimarProject.DTOS.StoryDTOs;
using ZahimarProject.Models;

namespace ZahimarProject.Services.StoryServices
{
    public interface IStoryServices
    {
        public List<StoryDTOs> GetStoryTestsDTOs(int patientId);

        public StoryDTOs GetStoryTestDTO(int storytestId);

        //public StoryTest AddStoryTestDTO(AddStoryTestDTO storytestDTO);
        //public StoryTest UpdateTestDTO(StoryTest storytest, AddStoryTestDTO storytestDTO);
        public Task<StoryTest> AddStoryTestDTO(StoryFormFileDto storyFormfileDTO);

        public StoryTest AddStoryQuestions(int storyTestId, List<AddStoryQuestionAnswerDTOs> questionsAndAnswers);


        public Task<StoryTest> UpdateTestDTO(StoryTest storytest, StoryFormFileDto storyFormfileDTO);


        public List<StoryInfoDto> GetAllStoryInfo();

        //public PatientStoryTest EvaluateTest(int patientId, int StorytestId, List<PatientStoryAnswersDTO> patientStoryAnswers);

        public int EvaluateTest(int patientId, int StorytestId, List<PatientStoryAnswersDTO> patientStoryAnswers);

        public List<StoryReviewDTO> ReviewStoryDTO(int PatientId);



    }
}