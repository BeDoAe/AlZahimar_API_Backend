using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZahimarProject.DTOS.DoctorDTO;
using ZahimarProject.DTOS.TestDTOs;
using ZahimarProject.Helpers;
using ZahimarProject.Helpers.UnitOfWorkFolder;
using ZahimarProject.Models;

namespace ZahimarProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public TestController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        [HttpGet("AllTest")]
        //[Authorize(policy: UserRoles.Relative)]
        [Authorize(Policy = "AdminOrRelative")] // Use the custom policy name here
        public ActionResult<dynamic> GetAllTests()
        {
            ClaimsPrincipal user = this.User;
            string loggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //Patient patient = unitOfWork.PatientRepository.GetPatient(loggedInUserId);


            //if (patient == null)
            //{
            //    return new GeneralResponse()
            //    {
            //        IsSuccess = false,
            //        Data = NotFound("Not user Found")
            //    };
            //}
            List<TestInfoDto> TestinfoDTOs = unitOfWork.TestService.GetAllTestInfo();


            if (TestinfoDTOs == null)
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
                Data = TestinfoDTOs
            };


        }


        [HttpGet]
        [Authorize(policy: UserRoles.Relative)]
        public ActionResult<dynamic>  GetTestsOfPatient()
        {
            ClaimsPrincipal user = this.User;
            string loggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Patient patient = unitOfWork.PatientRepository.GetPatient(loggedInUserId);


            if (patient == null)
            {
               return new GeneralResponse(){
                   IsSuccess = false,
                   Data = NotFound("Not Found")
                };
            }

            List<TestDTO> testDTOs = unitOfWork.TestService.GetTestDTOs(patient.Id);

            if (testDTOs == null)
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
                Data = testDTOs
            };

        }

        //[Authorize(policy: UserRoles.Relative)]
        [Authorize(Policy = "AdminOrRelative")] // Use the custom policy name here
        [HttpGet("test/{testId}")]
        public ActionResult<dynamic> GetTest(int testId)
        {
            // Test test = unitOfWork.TestRepository.Get(t=>t.Id==testId);
            TestDTO testDTO = unitOfWork.TestService.GetTestDTO(testId);
            if (testDTO == null)
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
                Data = testDTO
            };

        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<dynamic> AddTest(AddTestDTO testDTO)
        {
           Test test= unitOfWork.TestService.AddTestDTO(testDTO);
            if(testDTO == null)
            {
                return new GeneralResponse() { Data = NotFound("Not Found"), IsSuccess=false };
            }
            if (test == null)
            {
                return new GeneralResponse() { Data = NotFound("Not Found"), IsSuccess = false };
            }
            unitOfWork.TestRepository.Insert(test);
            unitOfWork.Save();
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = test
            };
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<dynamic> UpdateTest(int testId, AddTestDTO testDTO)
        {
            Test test = unitOfWork.TestRepository.Get(t=>t.Id== testId);
            if (test == null)
            {
                return new GeneralResponse() { Data = NotFound("Not Found"), IsSuccess = false };
            }
            Test Updatedtest = unitOfWork.TestService.UpdateTestDTO(test,testDTO);
            if (testDTO == null)
            {
                return new GeneralResponse() { Data = NotFound("Not Found"), IsSuccess = false };
            }
         
            unitOfWork.TestRepository.Update(test);
            unitOfWork.Save();
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = "Updated Successfully"
            };
        }

        [HttpDelete("{testId}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<dynamic> DeleteTest(int testId)
        {

            Test test = unitOfWork.TestRepository.Get(t=>t.Id== testId);
            if(test == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }

           
            test.IsDeleted = true;
            unitOfWork.TestRepository.Delete(test);
            unitOfWork.Save();
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = "Deleted Successfully"
            };

        }

        [Authorize(policy: UserRoles.Relative)]
        [HttpPost("submitTest")]
        public ActionResult<dynamic> SubmitTest( int testId, List<PatientAnswerDTO> patientAnswers)
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Patient patient = unitOfWork.PatientRepository.GetPatient(LoggedInUserId);

            ResultOfTestDTO result = unitOfWork.TestService.EvaluateTest(patient.Id, testId, patientAnswers,patient.FirstName+" "+patient.LastName);

            if(result == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            
            unitOfWork.Save();
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = result
            };
        }

        [HttpGet("ReviewTest")]

        public ActionResult<GeneralResponse> TestReview()
        {

            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Patient patient = unitOfWork.PatientRepository.GetPatient(LoggedInUserId);
            List<TestReviewDTO> testReviewDTOs = unitOfWork.TestService.ReviewTestDTO(patient.Id);
            if (testReviewDTOs.Count == 0)
            {
                return new GeneralResponse() { Data = NotFound("Not Fount any tests"), IsSuccess = false };
            }
            unitOfWork.Save();
            return new GeneralResponse() { IsSuccess = true, Data = testReviewDTOs };
        }


        [Authorize(policy: UserRoles.Relative)]
        [HttpPost("SubmitTest_Score")]
        public ActionResult<dynamic> SubmitTest_Score(int testId, List<PatientAnswerDTO> patientAnswers)
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Patient patient = unitOfWork.PatientRepository.GetPatient(LoggedInUserId);
            int score = unitOfWork.TestService.EvaluateTest_Score(patient.Id, testId, patientAnswers);

            if (score == -1)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            PatientTest patientTest = unitOfWork.TestRepository.PatientTest(patient.Id, testId);
            patientTest.Score = score;
            unitOfWork.Save();
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = patientTest
            };


        }


        [Authorize(policy: UserRoles.Relative)]
        [HttpPost("HasTest")]
        public ActionResult<dynamic> HasTest(int TestId)
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Patient patient = unitOfWork.PatientRepository.GetPatient(LoggedInUserId);

            bool hasTestAlready = unitOfWork.TestRepository.HasTest(patient.Id, TestId);
            if (hasTestAlready)
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
        [HttpPost("AssignPatientTest")]
        public ActionResult<dynamic> AssignTest(bool hasTest, int TestId)
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Patient patient = unitOfWork.PatientRepository.GetPatient(LoggedInUserId);

            if (hasTest)
            {
                int score = unitOfWork.TestRepository.ScoreOfPatientTest(patient.Id, TestId);
                return new GeneralResponse()
                {
                    IsSuccess = true,
                    Data = score
                };
            }
            else
            {

                var newRelation = new PatientTest()
                {
                    TestId = TestId,
                    PatientId = patient.Id,
                    Score = 0,
                    IsDeleted = false
                };
                unitOfWork.PatientTestRepository.Insert(newRelation);
                unitOfWork.Save();
                return new GeneralResponse()
                {
                    IsSuccess = true,
                    Data = unitOfWork.TestService.GetTestDTO(TestId)
                };
            }
        }
    }
}
