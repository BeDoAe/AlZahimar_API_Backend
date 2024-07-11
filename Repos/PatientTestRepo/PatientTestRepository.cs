using Microsoft.EntityFrameworkCore;
using ZahimarProject.Models;

namespace ZahimarProject.Repos.PatientTestRepo
{
    public class PatientTestRepository:Repository<PatientTest> , IPatientTestRepository
    {
        public readonly Context context;
       
        public PatientTestRepository(Context _context) : base(_context)
        {
            this.context = _context;
            
        }

        public override List<PatientTest> GetAll()
        {
            return context.PatientTests.Include(pt=>pt.Test).ToList();
        }
    }
}
