using System.Linq.Expressions;
using ZahimarProject.Models;

namespace ZahimarProject.Repos.ReportRepo
{
    public interface IReportRepository:IRepository<Report>
    {
        public Report Get(Expression<Func<Report, bool>> filter);
        public List<Report> ReportsForPatient(int PatientId);
        public List<Report> ReportsForPatientOfDoctor(int PatientId, int DoctorId);
        public List<Report> GetFilteredReportsByDate(List<Report> allReports);

    }
}