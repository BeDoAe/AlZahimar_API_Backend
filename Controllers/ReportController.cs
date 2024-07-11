using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Security.Claims;
using ZahimarProject.DTOS.DoctorDTO;
using ZahimarProject.DTOS.ReportDTOs;
using ZahimarProject.Helpers;
using ZahimarProject.Helpers.UnitOfWorkFolder;
using ZahimarProject.Models;

namespace ZahimarProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public ReportController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        Doctor GetDoctor()
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Doctor doctor = unitOfWork.DoctorRepository.Get(d => d.AppUserId == LoggedInUserId);
            return doctor;
        }
        int GetDoctorID()
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Doctor doctor = unitOfWork.DoctorRepository.Get(d => d.AppUserId == LoggedInUserId);
            return doctor.Id;
        }

        int GetPatientID()
        {
            ClaimsPrincipal user = this.User;
            string LoggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Patient patient = unitOfWork.PatientRepository.GetPatient(LoggedInUserId);
            return patient.Id;
        }

        [Authorize(Policy =UserRoles.Doctor)]
        [HttpPost("{PatientId:int}")]
        public ActionResult<dynamic> AddReport(int PatientId,AddReportDTO reportDTO)
        {

            if (ModelState.IsValid)
            {
                int doctorId = GetDoctorID();
                bool IsPatientFound = unitOfWork.DoctorRepository.IsPatientFoundInDoctorList(PatientId);
                if (IsPatientFound)
                {
                    Report report = unitOfWork.ReportService.MappDtoToReport(reportDTO, doctorId,PatientId);
                    unitOfWork.ReportRepository.Insert(report);
                    unitOfWork.Save();
                    return new GeneralResponse()
                    {
                        Data = reportDTO,
                        IsSuccess = true
                    };
                }
                else
                {
                    return new GeneralResponse() { IsSuccess = false, Data = "This Patient Not Assigned to this Doctor" };
                }

            }
            else
            {
                return new GeneralResponse() { IsSuccess = false, Data = ModelState };
            }
        }

        [Authorize(Policy = UserRoles.Doctor)]
        [HttpPut]
        [Route("reportId")]
        public  ActionResult<dynamic> UpdateReport(int reportId, UpdateReportDTO updateReportDTO)
        {
            if(ModelState.IsValid) 
            {
                Doctor doctor = GetDoctor();
                Report report = unitOfWork.ReportRepository.Get(r => r.Id == reportId);
                
                if (doctor.Id == report.DoctorId)
                {
                    Report UpdatedReport = unitOfWork.ReportService.MapToUpdateReportDTO(report, updateReportDTO);
                    if (UpdatedReport != null)
                    {
                        unitOfWork.ReportRepository.Update(UpdatedReport);
                        unitOfWork.Save();
                        return new GeneralResponse()
                        {
                            Data = "Updated Successfuly",
                            IsSuccess = true
                        };
                    }
                   
                }
               
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = "Not Found"
                };

            }
            return new GeneralResponse()
            {
                Data = ModelState,
                IsSuccess = false
            };
        }


        [Authorize(Policy = UserRoles.Doctor)]
        [HttpDelete]
        [Route("reportId")]
        public ActionResult<dynamic> DeleteReport(int reportId)
        {
            if (ModelState.IsValid)
            {
                Doctor doctor = GetDoctor();
                Report report = unitOfWork.ReportRepository.Get(r => r.Id == reportId);

                if (doctor.Id == report.DoctorId && report != null)
                {
                    report.IsDeleted = true;
                    unitOfWork.ReportRepository.Delete(report);
                    unitOfWork.Save();
                    return new GeneralResponse()
                    {
                        Data = "Deleted Successfully",
                        IsSuccess = true
                    };
                   
                }
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = "Not Found"
                };
            }
            return new GeneralResponse()
            {
                Data = ModelState,
                IsSuccess = false
            };
        }

        [Authorize(policy:UserRoles.Relative)]
        [HttpGet("ViewReport/{ReportId}")]
        public ActionResult<dynamic> ViewReport(int ReportId)
        {
            Report report = unitOfWork.ReportRepository.Get(r=>r.Id==ReportId);
            if (report == null)
            {
                return new GeneralResponse() { IsSuccess = false, Data = "Report Not Found" };
            }
            GettingPatientReportDTO reportDTO = unitOfWork.ReportService.GettingPatientReport(report);
            if (reportDTO == null)
            {
                return new GeneralResponse() { IsSuccess=false , Data="Report Not Found"};
            }
            return new GeneralResponse() { IsSuccess = true, Data = reportDTO };
        }


        [Authorize(policy:UserRoles.Relative)]
        [HttpGet("ReportsOfPatientByRelative")]
        public ActionResult<dynamic> GetAllReportOfPatient()
        {
            int PatientId=GetPatientID();
            List<Report> Reports = unitOfWork.ReportRepository.ReportsForPatient(PatientId);
            List<GettingAllReportsForPatient> GettingAllReportsForPatirnt =unitOfWork.ReportService.GettingAllReports(Reports); 

            return new GeneralResponse()
            {
                IsSuccess = true,
                Data= GettingAllReportsForPatirnt
            };
        }


        [Authorize(policy: UserRoles.Relative)]
        [HttpGet("ReportsCountOfPatient")]
        public ActionResult<dynamic> GetCountOfAllReportOfPatient()
        {
            int PatientId = GetPatientID();
            int ReportsCount = unitOfWork.ReportRepository.ReportsForPatient(PatientId).Count;
            if(ReportsCount==0)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = "Not Found"
                };
            }
            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = ReportsCount
            };
        }

        [Authorize(policy: UserRoles.Doctor)]
        [HttpGet("ReportsOfPatientByDoctor/{PatientId}")]
        public ActionResult<dynamic> GetAllReportOfPatientByDoctor(int PatientId)
        {
            List<Report> Reports = unitOfWork.ReportRepository.ReportsForPatient(PatientId);
            List<GettingAllReportsForPatient> GettingAllReportsForPatirnt = unitOfWork.ReportService.GettingAllReports(Reports);

            return new GeneralResponse()
            {
                IsSuccess = true,
                Data = GettingAllReportsForPatirnt
            };
        }




        [Authorize(policy: UserRoles.Doctor)]
        [HttpGet("ReportsOfPatientByLoggedInDoctor")]
        public ActionResult<dynamic> GetAllReportsOfSpecificDoctorFotSpecificPatient(int PatientId)
        {
            Doctor doctor = GetDoctor();

            List<Report> Reports = unitOfWork.ReportRepository.ReportsForPatientOfDoctor(PatientId, doctor.Id);
            if(Reports != null)
            {
                List<GettingAllReportsForPatient> GettingAllReportsForPatirnt = unitOfWork.ReportService.GettingAllReports(Reports);

                return new GeneralResponse()
                {
                    IsSuccess = true,
                    Data = GettingAllReportsForPatirnt
                };
            }
            return new GeneralResponse()
            {
                IsSuccess = false,
                Data = "Not Found"
            };

        }


        [Authorize(policy: UserRoles.Doctor)]
        [HttpGet("FilteredReportsByDate/{PatientId}")]
        public ActionResult<dynamic> GetFilteredReportsByDate(int PatientId)
        {
            List<Report> allReports = unitOfWork.ReportRepository.ReportsForPatient(PatientId).ToList();

            if (allReports == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }

            List<Report> filteredReports = unitOfWork.ReportRepository.GetFilteredReportsByDate(allReports);
            if (filteredReports == null)
            {
                return new GeneralResponse()
                {
                    IsSuccess = false,
                    Data = NotFound("Not Found")
                };
            }
            List<FilteredReportDTO> filteredReportDTOs = unitOfWork.ReportService.GetFilteredReportsDTO(filteredReports);
            if (filteredReportDTOs == null)
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
                Data = filteredReportDTOs
            };
        }


        [Authorize(policy: UserRoles.Doctor)]
        [HttpGet("Details/{ReportId}")]
        public ActionResult<dynamic> ViewReportOfPatient(int ReportId)
        {
            Report report = unitOfWork.ReportRepository.Get(r => r.Id == ReportId);
            if (report == null)
            {
                return new GeneralResponse() { IsSuccess = false, Data = "Report Not Found" };
            }
            GettingPatientReportDTO reportDTO = unitOfWork.ReportService.GettingPatientReport(report);
            if (reportDTO == null)
            {
                return new GeneralResponse() { IsSuccess = false, Data = "Report Not Found" };
            }
            return new GeneralResponse() { IsSuccess = true, Data = reportDTO };
        }

       

    }
}
