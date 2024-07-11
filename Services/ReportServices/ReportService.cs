using System.Numerics;
using ZahimarProject.DTOS.DoctorDTO;
using ZahimarProject.DTOS.ReportDTOs;
using ZahimarProject.Helpers.Enums;
using ZahimarProject.Models;

namespace ZahimarProject.Services.ReportServices
{
    public class ReportService:Service<Report>, IReportService
    {
        public Report MappDtoToReport(AddReportDTO reportDTO , int DoctorID,int PatientID)
        {
            Report report = new Report() {
                PatientId=PatientID,
                Description=reportDTO.Description,
                Title=reportDTO.Title,  
                DoctorId = DoctorID,
                Ranking=reportDTO.Ranking,
            };   

            return report;
        }


        public Report MapToUpdateReportDTO(Report Updatedreport, UpdateReportDTO updateReportDTO)
        {

            Updatedreport.Description = updateReportDTO.Description;
            Updatedreport.Title = updateReportDTO.Title;
            Updatedreport.Ranking = updateReportDTO.Ranking;

            return Updatedreport;
        }
        public GettingPatientReportDTO GettingPatientReport(Report report)
        {
            return new GettingPatientReportDTO() {
                Description=report.Description,
                DoctorID=report.DoctorId,
                DoctorName=report.Doctor.FirstName+" "+ report.Doctor.LastName,
                PatientName=report.Patient.FirstName+" "+report.Patient.LastName,
                Title = report.Title,
                DateTime=report.CreatedDate,
                PatientID=report.PatientId,
                Ranking = report.Ranking,
                ReportStatus = report.Ranking == ReportRanking.Important ? "Important" : "Normal",
            };
        }

        public List<GettingAllReportsForPatient> GettingAllReports(List<Report> reportList)
        {
            List<GettingAllReportsForPatient> reportsForPatirnts = new List<GettingAllReportsForPatient>();
            foreach (Report report in reportList)
            {
                reportsForPatirnts.Add(new GettingAllReportsForPatient()
                {
                    DateTime = report.CreatedDate,
                    ReportID=report.Id,
                    Title=report.Title,
                    Ranking=report.Ranking,
                    ReportStatus = report.Ranking == ReportRanking.Important ? "Important" : "Normal",

                });
            }
            return reportsForPatirnts;

        }

        public List<FilteredReportDTO> GetFilteredReportsDTO(List<Report> reports )
        {
            return reports.Select(report => new FilteredReportDTO
            {
                DateTime = report.CreatedDate,
                Title= report.Title,
                ReportID= report.Id,
                Ranking= report.Ranking,
                ReportStatus = report.Ranking == ReportRanking.Important ? "Important" : "Normal",

            }).ToList();
        }
    }
}
