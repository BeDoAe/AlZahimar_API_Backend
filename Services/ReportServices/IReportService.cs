using ZahimarProject.DTOS.ReportDTOs;
using ZahimarProject.Models;

namespace ZahimarProject.Services.ReportServices
{
    public interface IReportService:IService<Report>
    {
        public Report MappDtoToReport(AddReportDTO reportDTO, int DoctorID, int PatientID);

        public Report MapToUpdateReportDTO(Report Updatedreport, UpdateReportDTO updateReportDTO);

        public GettingPatientReportDTO GettingPatientReport(Report report);
        public List<GettingAllReportsForPatient> GettingAllReports(List<Report> reportList);
        public List<FilteredReportDTO> GetFilteredReportsDTO(List<Report> reports);
    }
}