using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Claims;
using ZahimarProject.Authentication;
using ZahimarProject.DTOS.StoryDTOs;
using ZahimarProject.DTOS.TestDTOs;
using ZahimarProject.Helpers.UnitOfWorkFolder;
using ZahimarProject.Helpers;
using ZahimarProject.Models;
using ZahimarProject.DTOS.StoryDTOs;

namespace ZahimarProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoryController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public StoryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [HttpGet("AllStorytest")]
        [Authorize(policy: UserRoles.Relative)]
        public ActionResult<dynamic> GetAllStoryTests()
        {
            ClaimsPrincipal user = this.User;
            string loggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Patient patient = unitOfWork.PatientRepository.GetPatient(loggedInUserId);


            if (patient == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not user Found")
                };
            }
            List<StoryInfoDto> storyinfoDTOs = unitOfWork.StoryServices.GetAllStoryInfo();


            if (storyinfoDTOs == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }

            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = storyinfoDTOs
            };


        }
        [HttpGet]
        [Authorize(policy: UserRoles.Relative)]
        public ActionResult<dynamic> GetStoryTestsOfPatient()
        {
            ClaimsPrincipal user = this.User;
            string loggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Patient patient = unitOfWork.PatientRepository.GetPatient(loggedInUserId);


            if (patient == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }

            List<StoryDTOs> storyDTOs = unitOfWork.StoryServices.GetStoryTestsDTOs(patient.Id);

            if (storyDTOs == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }

            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = storyDTOs
            };

        }

        [Authorize(policy: UserRoles.Relative)]
        [HttpGet("Storytest/{StorytestId}")]
        public ActionResult<dynamic> GetStoryTest(int StorytestId)
        {
            // Test test = unitOfWork.TestRepository.Get(t=>t.Id==testId);
            StoryDTOs storytestDTO = unitOfWork.StoryServices.GetStoryTestDTO(StorytestId);
            if (storytestDTO == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = storytestDTO
            };

        }

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<dynamic>> AddStoryTest([FromForm] StoryFormFileDto storyformfileDTO)
        {
            StoryTest storytest = await unitOfWork.StoryServices.AddStoryTestDTO(storyformfileDTO);
            if (storyformfileDTO == null)
            {
                return new GeneralResponse() { Data = NotFound("Not Found"), IsSuccess = false };
            }
            //if (storytestDTO == null)
            //{
            //    return new GeneralResponse() { Data = NotFound("Not Found"), IsSuccess = false };
            //}
            unitOfWork.StoryRepository.Insert(storytest);
            unitOfWork.Save();
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = storytest
            };
        }

        [HttpPost("AddStoryQuestions/{storyTestId}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<dynamic> AddStoryQuestions(int storyTestId, [FromBody] List<AddStoryQuestionAnswerDTOs> questionsAndAnswers)
        {
            var storyTest = unitOfWork.StoryRepository.Getspecific(storyTestId);
            if (storyTest == null)
            {
                return new GeneralResponse { Data = NotFound("Not Found"), IsSuccess = false };
            }

            storyTest.StoryQuestionAndAnswers = questionsAndAnswers.Select(q => new StoryQuestionAndAnswer
            {
                Question = q.StoryQuestion,
                CorrectAnswer = q.StoryCorrectAnswer,
                Answers = q.StoryAnswers
            }).ToList();

            unitOfWork.StoryRepository.Update(storyTest);
            unitOfWork.Save();

            return new GeneralResponse
            {
                IsSuccess = true,
                Data = storyTest
            };
        }
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<dynamic>> UpdateStoryTest(int storytestId, StoryFormFileDto storyformfileDTO)
        {
            StoryTest storytest = unitOfWork.StoryRepository.Get(t => t.Id == storytestId);
            if (storytest == null)
            {
                return new GeneralResponse() { Data = NotFound("Not Found"), IsSuccess = false };
            }
            StoryTest UpdatedStorytest = await unitOfWork.StoryServices.UpdateTestDTO(storytest, storyformfileDTO);
            if (storyformfileDTO == null)
            {
                return new GeneralResponse() { Data = NotFound("Not Found"), IsSuccess = false };
            }

            unitOfWork.StoryRepository.Update(storytest);
            unitOfWork.Save();
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = "Updated Successfully"
            };
        }
        [HttpPost("UpdateStoryQuestions/{storyTestId}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<dynamic> UpdateStoryQuestions(int storyTestId, [FromBody] List<AddStoryQuestionAnswerDTOs> questionsAndAnswers)
        {
            var storyTest = unitOfWork.StoryRepository.Getspecific(storyTestId);
            if (storyTest == null)
            {
                return new GeneralResponse { Data = NotFound("Not Found"), IsSuccess = false };
            }

            storyTest.StoryQuestionAndAnswers = questionsAndAnswers.Select(q => new StoryQuestionAndAnswer
            {
                Question = q.StoryQuestion,
                CorrectAnswer = q.StoryCorrectAnswer,
                Answers = q.StoryAnswers
            }).ToList();

            unitOfWork.StoryRepository.Update(storyTest);
            unitOfWork.Save();

            return new GeneralResponse
            {
                IsSuccess = true,
                Data = storyTest
            };
        }


        [HttpDelete("{storytestId}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<dynamic> DeleteStoryTest(int storytestId)
        {

            StoryTest storytest = unitOfWork.StoryRepository.Getspecific(storytestId);
            if (storytest == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }


            storytest.IsDeleted = true;
            unitOfWork.StoryRepository.Delete(storytest);
            unitOfWork.Save();
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = "Deleted Successfully"
            };

        }

        [Authorize(policy: UserRoles.Relative)]
        [HttpPost("submitStoryTest")]
        public ActionResult<dynamic> SubmitStoryTest(int storytestId, List<PatientStoryAnswersDTO> patientStoryAnswers)
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Patient patient = unitOfWork?.PatientRepository.GetPatient(LoggedInUserId);
            int score = unitOfWork.StoryServices.EvaluateTest(patient.Id, storytestId, patientStoryAnswers);

            if (score == -1)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            PatientStoryTest patientStoryTest = unitOfWork.StoryRepository.PatientStoryTest(patient.Id, storytestId);
            patientStoryTest.Score = score;
            patientStoryTest.DateTaken = DateTime.Now;
            unitOfWork.Save();
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = patientStoryTest
            };

        }

        [Authorize(policy: UserRoles.Relative)]
        [HttpPost("HasStoryTest")]
        public ActionResult<dynamic> HasStoryTest(int storytestId)
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Patient patient = unitOfWork.PatientRepository.GetPatient(LoggedInUserId);

            bool hasStoryAlready = unitOfWork.StoryRepository.HasTest(patient.Id, storytestId);
            if (hasStoryAlready)
            {
                //return true;
                return new GeneralResponse()
                {
                    IsSuccess = true,
                    Data = true
                };

            }
            else
            {
                //return false;
                return new GeneralResponse()
                {
                    IsSuccess = true,
                    Data = false
                };
            }
        }


        [Authorize(policy: UserRoles.Relative)]
        [HttpPost("AssignPatientStoryTest")]
        public ActionResult<dynamic> AssignStoryTest(bool hasStoryTest, int storytestId)
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Patient patient = unitOfWork.PatientRepository.GetPatient(LoggedInUserId);

            if (hasStoryTest)
            {
                int score = unitOfWork.StoryRepository.ScoreOfPatientStory(patient.Id, storytestId);
                return new GeneralResponse()
                {
                    IsSuccess = true,
                    Data = score
                };
            }
            else
            {

                var newRelation = new PatientStoryTest()
                {
                    StoryTestId = storytestId,
                    PatientId = patient.Id,
                    Score = 0,
                    IsDeleted = false
                };
                unitOfWork.PatientStoryRepository.Insert(newRelation);
                unitOfWork.Save();
                return new GeneralResponse()
                {
                    IsSuccess = true,
                    Data = unitOfWork.StoryServices.GetStoryTestDTO(storytestId)
                };
            }
        }


        [HttpGet("ReviewTest")]
        public ActionResult<GeneralResponse> StoryReview()
        {

            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Patient patient = unitOfWork.PatientRepository.GetPatient(LoggedInUserId);
            List<StoryReviewDTO> StoryReviewDTOs = unitOfWork.StoryServices.ReviewStoryDTO(patient.Id);
            if (StoryReviewDTOs.Count == 0)
            {
                return new GeneralResponse() { Data = NotFound("Not Fount any tests"), IsSuccess = false };
            }
            unitOfWork.Save();
            return new GeneralResponse() { IsSuccess = true, Data = StoryReviewDTOs };
        }
    }
}
