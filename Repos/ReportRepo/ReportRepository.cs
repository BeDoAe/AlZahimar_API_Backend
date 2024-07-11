using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ZahimarProject.Models;

namespace ZahimarProject.Repos.ReportRepo
{
    public class ReportRepository : Repository<Report>, IReportRepository
    {
        private readonly Context context;
        public ReportRepository(Context _context) : base(_context)
        {
            this.context = _context;
        }

        public override Report Get(Expression<Func<Report, bool>> filter)
        {

            return context.Reports.Include(r=>r.Doctor).Include(r=>r.Patient).FirstOrDefault(filter);
        }

        public List<Report> ReportsForPatient(int PatientId)
        {
            return context.Reports.Where(r=>r.PatientId==PatientId).ToList();
        }

        public List<Report> ReportsForPatientOfDoctor(int PatientId , int DoctorId)
        {
            return context.Reports.Where(r => r.PatientId == PatientId && r.DoctorId == DoctorId).ToList();
        }

        public List<Report> GetFilteredReportsByDate(List<Report> allReports)
        {
            List<Report> filteredReports = (
                from report in allReports
                orderby report.CreatedDate
                select report
            ).ToList();

            return filteredReports;
        }


    }
}
